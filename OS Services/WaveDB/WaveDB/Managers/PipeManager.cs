using System.IO.Pipes;
using System.Text;

namespace WaveDB
{
    public class DatabasePipeServer
    {
        private NamedPipeServerStream _pipeServer;
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
            byte[] buffer = new byte[1024];
            int bytesRead = await _pipeServer.ReadAsync(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Pipe Manager: Received request: {request}");

            // Parse the request (you can use JSON or simple format)
            string response = ProcessRequest(request);

            // Send response back
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await _pipeServer.WriteAsync(responseBytes, 0, responseBytes.Length);
            await _pipeServer.FlushAsync();
        }

        private string ProcessRequest(string request)
        {
            try
            {
                // Simple format: "WRITE|database_name|table_name|column=value,column=value"
                string[] parts = request.Split('|');

                if (parts.Length < 3)
                    return "ERROR: Invalid request format";

                string action = parts[0];
                string databaseName = parts[1];
                string tableName = parts[2];
                string data = parts[3];

                switch (action.ToUpper())
                {
                    case "WRITE":
                        return HandleWriteRequest(databaseName, tableName, data);
                    case "READ":
                        var readResult = HandleReadRequest(databaseName, tableName, data);
                        return readResult != null ? System.Text.Json.JsonSerializer.Serialize(readResult) : "ERROR: No data found";
                    default:
                        return "ERROR: Unknown action";
                }
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        private string HandleWriteRequest(string databaseName, string tableName, string data)
        {
            try
            {
                // Parse data: "property_name=os_version,property_value=1.0.0"
                var dataDict = new Dictionary<string, string>();
                string[] pairs = data.Split(',');

                foreach (string pair in pairs)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        dataDict[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }

                // Use your existing database connection
                //(Production) var connection = SQLite_Manager.OpenConnection($"/var/lib/wavedb/{databaseName}.wvdb");
                var connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                if (connection == null)
                {
                    Console.WriteLine("Database does not exist. Creating a new database...");
                    // (Production) SQLite_Manager.CreateDatabase($"/var/lib/wavedb/{databaseName}.wvdb");
                    // (Production) connection = SQLite_Manager.OpenConnection($"/var/lib/wavedb/{databaseName}.wvdb");
                    SQLite_Manager.CreateDatabase($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                    connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{databaseName}.wvdb");
                }
                foreach (var property in dataDict)
                {
                    Console.WriteLine($"Key: {property.Key}, Value: {property.Value}");
                    SQLite_Manager.InsertOrReplaceData(connection, tableName, property.Key, property.Value);
                }
                


                return "SUCCESS: Data written";
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        private List<Dictionary<string, object>> HandleReadRequest(string database_name, string tableName, string data)
        {
            try
            {
                // Parse data: "property_name=os_version,property_name=os_name"
                string[] pairs = data.Split(',');
                var propertyNames = new List<string>();

                foreach (string pair in pairs)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2 && keyValue[0].Trim() == "property_name")
                    {
                        propertyNames.Add(keyValue[1].Trim());
                    }
                }

                // Use your existing database connection
                //(Production) var connection = SQLite_Manager.OpenConnection($"/var/lib/wavedb/{database_name}.wvdb");
                var connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{database_name}.wvdb");
                if (connection == null)
                {
                    Console.WriteLine("Database does not exist. Creating a new database...");
                    // (Production) SQLite_Manager.CreateDatabase($"/var/lib/wavedb/{database_name}.wvdb");
                    // (Production) connection = SQLite_Manager.OpenConnection($"/var/lib/wavedb/{database_name}.wvdb");
                    SQLite_Manager.CreateDatabase($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{database_name}.wvdb");
                    connection = SQLite_Manager.OpenConnection($"C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\{database_name}.wvdb");
                }
                // Get data from the database
                return SQLite_Manager.ExecuteReader(connection, tableName, propertyNames.ToArray());
                // Todo: Implement more arguments for the read request (WHERE, ORDER BY, etc.)
                
            }
            catch
            {
                return null;
            }
        }

        public void StopServer()
        {
            _isRunning = false;
            _pipeServer?.Dispose();
        }
    }
}