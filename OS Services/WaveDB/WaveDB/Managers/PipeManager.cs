using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace WaveDB
{
    public class DatabasePipeServer
    {
        private NamedPipeServerStream? _pipeServer;
        private bool _isRunning = false;
        private readonly string _pipeName = "WaveDB_Pipe";

        public async Task StartServer()
        {
            _isRunning = true;
            Console.WriteLine("Pipe Manager: Starting pipe server...");

            while (_isRunning)
            {
                _pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.InOut);

                try
                {
                    Console.WriteLine("Pipe Manager: Waiting for client connection...");
                    await _pipeServer.WaitForConnectionAsync();
                    Console.WriteLine("Pipe Manager: Client connected!");

                    await HandleClientRequest();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Pipe Manager: Pipe server error: {ex.Message}");
                }
                finally
                {
                    _pipeServer?.Dispose();
                }
            }
        }

        private async Task HandleClientRequest()
        {
            if (_pipeServer == null) throw new ArgumentNullException(nameof(_pipeServer));

            var buffer = new byte[4096];
            int bytesRead = await _pipeServer.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Pipe Manager: Received request: {request}");

                string response = ProcessRequest(request);

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await _pipeServer.WriteAsync(responseBytes, 0, responseBytes.Length);
                await _pipeServer.FlushAsync();

                Console.WriteLine($"Pipe Manager: Sent response: {response}");
            }
        }

        private string ProcessRequest(string request)
        {
            try
            {
                // Parse JSON request instead of pipe-delimited string
                var requestObj = JsonSerializer.Deserialize<JsonElement>(request);

                string action = requestObj.GetProperty("action").GetString() ?? string.Empty;
                string databaseName = requestObj.GetProperty("database").GetString() ?? string.Empty;
                string tableName = requestObj.GetProperty("table").GetString() ?? string.Empty;

                switch (action.ToUpper())
                {
                    case "WRITE":
                        var writeData = requestObj.GetProperty("data");
                        return HandleWriteRequest(databaseName, tableName, writeData);
                    case "READ":
                        var readItems = requestObj.TryGetProperty("items", out var items) ? items : (JsonElement?)null;
                        var where = requestObj.TryGetProperty("where", out var whereElement) ? whereElement : (JsonElement?)null;
                        var order = requestObj.TryGetProperty("order", out var orderByElement) ? orderByElement : (JsonElement?)null;

                        return HandleReadRequest(databaseName, tableName, readItems,where, order);
                    default:
                        return CreateErrorResponse($"Unknown action: {action}");
                }
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Request processing failed: {ex.Message}");
            }
        }

        private string CreateSuccessResponse(object data)
        {
            var response = new
            {
                success = true,
                error = (string?)null,
                result = data
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private string CreateErrorResponse(string errorMessage)
        {
            var response = new
            {
                success = false,
                error = errorMessage,
                result = (object?)null,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private string HandleWriteRequest(string databaseName, string tableName, JsonElement writeData)
        {
            try
            {
                var dataDict = new Dictionary<string, string>();

                // Parse JSON data object
                foreach (var property in writeData.EnumerateObject())
                {
                    dataDict[property.Name] = property.Value.GetString() ?? string.Empty;
                }

                //var connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                // Production
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    Console.WriteLine("Database does not exist. Creating a new database...");
                    //SQLite_Manager.CreateDatabase($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                    //connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                    SQLite_Manager.CreateDatabase($"/var/lib/WaveOS/{databaseName}.wvdb");
                    connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                    SQLite_Manager.CreateTable(connection, tableName,
                        TableBuilder.Id(),
                        TableBuilder.Text("property_name").NotNull().Unique(),
                        TableBuilder.Text("property_value").NotNull(),
                        TableBuilder.Timestamp("last_updated")
                    );
                    
                }

                int recordsWritten = 0;
                foreach (var property in dataDict)
                {
                    Console.WriteLine($"Key: {property.Key}, Value: {property.Value}");
                    SQLite_Manager.InsertOrReplaceData(connection, tableName, property.Key, property.Value);
                    recordsWritten++;
                }

                // Return JSON success response
                return CreateSuccessResponse(new
                {
                    action = "WRITE",
                    database = databaseName,
                    table = tableName,
                    records_written = recordsWritten,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Write operation failed: {ex.Message}");
            }
        }

        private string HandleReadRequest(string databaseName, string tableName, JsonElement? readItmes, JsonElement? whereClause, JsonElement? order)
        {
            try
            {
                var propertyNames = new List<string>();
                string where_clause = string.Empty;
                string order_clause = string.Empty;
                // Parse JSON filters if provided
                if (readItmes.HasValue && readItmes.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var filter in readItmes.Value.EnumerateArray())
                    {
                        propertyNames.Add(filter.GetString() ?? string.Empty);
                    }
                }
                if (whereClause.HasValue && whereClause.Value.ValueKind == JsonValueKind.String)
                {
                    where_clause = whereClause.Value.GetString() ?? string.Empty;
                }
                if (order.HasValue && order.Value.ValueKind == JsonValueKind.String)
                {
                    order_clause = order.Value.GetString() ?? string.Empty;
                }

                //var connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                // Production
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    throw new Exception($"Database '{databaseName}' does not exist.");
                    
                }

                var results = SQLite_Manager.ExecuteReader(connection, tableName, propertyNames.Count > 0 ? propertyNames.ToArray() : null!, where_clause, order_clause);

                // Return JSON success response
                return CreateSuccessResponse(new
                {
                    action = "READ",
                    database = databaseName,
                    table = tableName,
                    records_found = results.Count,
                    data = results,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Read operation failed: {ex.Message}");
            }
        }

        public void StopServer()
        {
            _isRunning = false;
            _pipeServer?.Dispose();
        }
    }
}