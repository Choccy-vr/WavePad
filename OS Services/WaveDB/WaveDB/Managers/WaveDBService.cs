using Tmds.DBus;
using System.Text.Json;

namespace WaveDB
{
    public class WaveDBService : IWaveDBService
    {
        public ObjectPath ObjectPath => "/org/waveOS/WaveDB";

        public Task<string> WriteAsync(string databaseName, string tableName, string data)
        {
            return Task.FromResult(HandleWriteRequest(databaseName, tableName, JsonSerializer.Deserialize<JsonElement>(data)));
        }

        public Task<string> ReadAsync(string databaseName, string tableName, string[]? items, string? where, string? order)
        {
            var itemsElement = items != null ? JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(items)) : (JsonElement?)null;
            var whereElement = !string.IsNullOrEmpty(where) ? JsonSerializer.Deserialize<JsonElement>($"\"{where}\"") : (JsonElement?)null;
            var orderElement = !string.IsNullOrEmpty(order) ? JsonSerializer.Deserialize<JsonElement>($"\"{order}\"") : (JsonElement?)null;
            
            return Task.FromResult(HandleReadRequest(databaseName, tableName, itemsElement, whereElement, orderElement));
        }

        public Task<string> ReadRowsAsync(string databaseName, string tableName)
        {
            return Task.FromResult(HandleReadRowsRequest(databaseName, tableName));
        }

        public Task<string> DeleteAsync(string databaseName, string tableName, string whereClause)
        {
            var whereElement = JsonSerializer.Deserialize<JsonElement>($"\"{whereClause}\"");
            return Task.FromResult(HandleDeleteRequest(databaseName, tableName, whereElement));
        }

        public Task<string> CreateAsync(string databaseName, string tableName, string columns)
        {
            var columnsElement = JsonSerializer.Deserialize<JsonElement>(columns);
            return Task.FromResult(HandleCreateRequest(databaseName, tableName, columnsElement));
        }

        public Task<string> WriteRowAsync(string databaseName, string tableName, string data)
        {
            var dataElement = JsonSerializer.Deserialize<JsonElement>(data);
            return Task.FromResult(HandleWriteRowRequest(databaseName, tableName, dataElement));
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

                foreach (var property in writeData.EnumerateObject())
                {
                    dataDict[property.Name] = property.Value.GetString() ?? string.Empty;
                }

                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    Console.WriteLine("Database does not exist. Creating a new database...");
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

        private string HandleReadRequest(string databaseName, string tableName, JsonElement? readItems, JsonElement? whereClause, JsonElement? order)
        {
            try
            {
                var propertyNames = new List<string>();
                string where_clause = string.Empty;
                string order_clause = string.Empty;

                if (readItems.HasValue && readItems.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var filter in readItems.Value.EnumerateArray())
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

                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    throw new Exception($"Database '{databaseName}' does not exist.");
                }

                var results = SQLite_Manager.ExecuteReader(connection, tableName, propertyNames.Count > 0 ? propertyNames.ToArray() : null!, where_clause, order_clause);

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

        private string HandleReadRowsRequest(string databaseName, string tableName)
        {
            try
            {
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    throw new Exception($"Database '{databaseName}' does not exist.");
                }

                var rows = SQLite_Manager.GetAllRows(connection, tableName);
                if (rows == null || rows.Count == 0)
                {
                    return CreateErrorResponse($"No rows found in table '{tableName}' in database '{databaseName}'.");
                }

                return CreateSuccessResponse(new
                {
                    action = "READ_ROWS",
                    database = databaseName,
                    table = tableName,
                    rows_read = rows.Count,
                    row_data = rows,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Read row operation failed: {ex.Message}");
            }
        }

        private string HandleDeleteRequest(string databaseName, string tableName, JsonElement whereClause)
        {
            try
            {
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    throw new Exception($"Database '{databaseName}' does not exist.");
                }

                string whereCondition = whereClause.GetString() ?? string.Empty;
                SQLite_Manager.DeleteData(connection, tableName, whereCondition);

                return CreateSuccessResponse(new
                {
                    action = "DELETE",
                    database = databaseName,
                    table = tableName,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        private string HandleCreateRequest(string databaseName, string tableName, JsonElement columns)
        {
            try
            {
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    Console.WriteLine("Database does not exist. Creating a new database...");
                    SQLite_Manager.CreateDatabase($"/var/lib/WaveOS/{databaseName}.wvdb");
                    connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                }

                var columnDefinitions = new List<string>();
                columnDefinitions.Add("id INTEGER PRIMARY KEY AUTOINCREMENT");

                if (columns.ValueKind == JsonValueKind.Array)
                {
                    foreach (var column in columns.EnumerateArray())
                    {
                        var columnType = column.GetProperty("type").GetString() ?? "TEXT";
                        var columnName = column.GetProperty("name").GetString() ?? "column_name";
                        var columnConstraints = column.GetProperty("constraints").EnumerateArray()
                            .Select(c => c.GetString() ?? "")
                            .ToList();

                        string columnDef = $"{columnName} {columnType.ToUpper()}";
                        if (columnConstraints.Any())
                        {
                            columnDef += $" {string.Join(" ", columnConstraints).ToUpper()}";
                        }

                        columnDefinitions.Add(columnDef);
                    }
                }

                string tableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columnDefinitions)})";
                SQLite_Manager.ExecuteNonQuery(connection, tableQuery);

                return CreateSuccessResponse(new
                {
                    action = "CREATE",
                    database = databaseName,
                    table = tableName,
                    columns_created = columnDefinitions.Count - 1,
                    query = tableQuery,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Create operation failed: {ex.Message}");
            }
        }

        private string HandleWriteRowRequest(string databaseName, string tableName, JsonElement writeData)
        {
            try
            {
                var connection = SQLite_Manager.OpenConnection($"/var/lib/WaveOS/{databaseName}.wvdb");
                if (connection == null)
                {
                    throw new Exception($"Database '{databaseName}' does not exist. Create the table first using CREATE action.");
                }

                var columns = new List<string>();
                var values = new List<object>();

                foreach (var property in writeData.EnumerateObject())
                {
                    columns.Add(property.Name);
                    values.Add(property.Value.ToString() ?? string.Empty);
                }

                SQLite_Manager.InsertOrReplaceRowData(connection, tableName, columns.ToArray(), values.ToArray());

                return CreateSuccessResponse(new
                {
                    action = "WRITE_ROW",
                    database = databaseName,
                    table = tableName,
                    records_written = 1,
                    columns_inserted = columns.Count,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                });
            }
            catch (Exception ex)
            {
                return CreateErrorResponse($"Write row operation failed: {ex.Message}");
            }
        }
    }
}