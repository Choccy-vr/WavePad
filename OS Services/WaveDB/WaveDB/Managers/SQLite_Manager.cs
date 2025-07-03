using System.Data.SQLite;

namespace WaveDB
{
    public class SQLite_Manager
    {
        private static bool IsValidName(string Name)
        {
            // Only allow letters, numbers, and underscores
            return System.Text.RegularExpressions.Regex.IsMatch(Name, @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }
        /// <summary>
        /// Executes a non-query SQL command (like CREATE, INSERT, UPDATE, DELETE) on the given SQLite connection.
        /// </summary>
        public static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Creates a new SQLite database file at the specified path.
        /// </summary>
        public static void CreateDatabase(string fullPath)
        {
            if (fullPath == null)
                throw new ArgumentNullException(nameof(fullPath));

            // Create directory if it doesn't exist
            string? directory = Path.GetDirectoryName(fullPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create database file
            SQLiteConnection.CreateFile(fullPath);
        }
        /// <summary>
        /// Opens a connection to the SQLite database at the specified path.
        /// </summary>
        public static SQLiteConnection OpenConnection(string fullPath)
        {
            if (fullPath == null)
                throw new ArgumentNullException(nameof(fullPath));

            string connectionString = $"Data Source={fullPath};Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("SQLite Manager: Connected to SQLite!");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                connection.Close();
                throw;
            }
        }
        /// <summary>
        /// Closes the given SQLite connection if it is open.
        /// </summary>
        public static void CloseConnection(SQLiteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Creates a new table in the SQLite database with the specified name and columns.
        /// The table will only be created if it does not already exist.
        /// </summary>
        public static void CreateTable(SQLiteConnection connection, string tableName, params ColumnDefinition[] columns)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (columns == null || columns.Length == 0)
                throw new ArgumentNullException(nameof(columns));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }

            // Convert columns to SQL string
            string columnDefinitions = string.Join(", ", columns.Select(col => col.ToString()));

            string query = $"CREATE TABLE IF NOT EXISTS {tableName} ({columnDefinitions})";
            ExecuteNonQuery(connection, query);

            Console.WriteLine($"SQLite Manager: Table '{tableName}' created successfully!");
        }
        /// <summary>
        /// Creates a new reader to execute a SELECT query on the specified table.
        /// This method allows you to specify which properties to select, a WHERE clause, and an ORDER BY clause.
        /// /// It returns a list of dictionaries, where each dictionary represents a row with column names as keys.
        /// </summary>
        public static List<Dictionary<string, object?>> ExecuteReader(SQLiteConnection connection,
            string tableName, string[]? property_names = null, string? whereClause = null,
            string? orderBy = null)
        {
            if (!IsValidName(tableName))
                throw new ArgumentException("Invalid table name");

            // Build query
            string propertyList = property_names != null ? string.Join(", ", property_names) : "*";
            string query = $"SELECT {propertyList} FROM {tableName}";

            if (!string.IsNullOrEmpty(whereClause))
                query += $" WHERE {whereClause}";

            if (!string.IsNullOrEmpty(orderBy))
                query += $" ORDER BY {orderBy}";

            var results = new List<Dictionary<string, object?>>();

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            row[columnName] = value;
                        }
                        results.Add(row);
                    }
                }
            }
            return results;
        }
        public static List<Dictionary<string, object?>> GetAllRows(
    SQLiteConnection connection,
    string tableName)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (!IsValidName(tableName))
                throw new ArgumentException("Invalid table name.");

            string query = $"SELECT * FROM {tableName}";
            var results = new List<Dictionary<string, object?>>();

            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string col = reader.GetName(i);
                        object? val = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[col] = val;
                    }
                    results.Add(row);
                }
            }
            return results;
        }
        /// <summary>
        /// Retrieves a single property value from the specified table based on the property name.
        /// If the property is not found, it returns "Not Found".
        /// </summary>
        public static string GetData(SQLiteConnection connection, string tableName, string property_name)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (property_name == null)
                throw new ArgumentNullException(nameof(property_name));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }
            string selectQuery = $"SELECT property_value FROM {tableName} WHERE property_name = @name";
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@name", property_name);

                object result = command.ExecuteScalar();
                return result?.ToString() ?? "Not Found";
            }
        }
        /// <summary>
        /// Updates the value of a property in the specified table.
        /// If the property does not exist, it will not create a new entry.
        /// </summary>
        public static void UpdateData(SQLiteConnection connection, string tableName, string property_name, string newValue)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (property_name == null)
                throw new ArgumentNullException(nameof(property_name));
            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }
            string updateQuery = $@"
                UPDATE {tableName} 
                SET property_value = @newValue, last_updated = CURRENT_TIMESTAMP 
                WHERE property_name = @name";
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@newValue", newValue);
                command.Parameters.AddWithValue("@name", property_name);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"SQLite Manager: Updated: {property_name} = {newValue}");
                }
                else
                {
                    Console.WriteLine($"SQLite Manager: No rows updated for property: {property_name}");
                }
            }
        }
        /// <summary>
        /// Inserts or replaces a property in the specified table.
        /// If the property already exists, it will be updated with the new value.
        /// If the property does not exist, it will be inserted as a new entry.
        /// </summary>
        public static void InsertOrReplaceData(SQLiteConnection connection, string tableName, string property_name, string property_value)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (property_name == null)
                throw new ArgumentNullException(nameof(property_name));
            if (property_value == null)
                throw new ArgumentNullException(nameof(property_value));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }
            string insertQuery = $@"
                INSERT OR REPLACE INTO {tableName} (property_name, property_value, last_updated) 
                VALUES (@name, @value, CURRENT_TIMESTAMP)";
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@name", property_name);
                command.Parameters.AddWithValue("@value", property_value);
                command.ExecuteNonQuery();
                Console.WriteLine($"SQLite Manager: Inserted: {property_name} = {property_value}");
            }
        }
        public static void InsertOrReplaceRowData(SQLiteConnection connection, string tableName, string[] columns, object[] values)
        {
            if (columns.Length != values.Length)
            {
                throw new ArgumentException("Columns and values arrays must have the same length");
            }

            try
            {
                string columnsList = string.Join(", ", columns);
                string valuesList = string.Join(", ", values.Select((_, i) => $"@param{i}"));

                string query = $"INSERT OR REPLACE INTO {tableName} ({columnsList}) VALUES ({valuesList})";

                using (var command = new SQLiteCommand(query, connection))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@param{i}", values[i] ?? DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                    Console.WriteLine($"SQLite Manager: Inserted/Replaced row in {tableName}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to insert or replace row data: {ex.Message}");
            }
        }
        /// <summary>
        /// Deletes a property from the specified table based on the property name.
        /// If the property does not exist, it will not throw an error but will notify that the property was not found.
        /// </summary>
        public static void DeleteData(SQLiteConnection connection, string tableName, string property_name)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (property_name == null)
                throw new ArgumentNullException(nameof(property_name));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }
            string deleteQuery = $"DELETE FROM {tableName} WHERE property_name = @name";
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@name", property_name);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine($"SQLite Manager: Deleted property: {property_name}");
                else
                    Console.WriteLine($"SQLite Manager: Property {property_name} not found");
            }
        }
        /// <summary>
        /// Checks if a table exists in a database.
        /// </summary>
        public static bool isTableExist(SQLiteConnection connection, string tableName)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }

            string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    Console.WriteLine($"SQLite Manager: Table {tableName} exists.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"SQLite Manager: Table {tableName} does not exist.");
                    return false;
                }
            }
        }
        /// <summary>
        /// Checks if a column exists in a specified table.
        /// </summary>
        public static bool isColumnExist(SQLiteConnection connection, string tableName, string columnName)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            if (!IsValidName(tableName) || !IsValidName(columnName))
            {
                throw new ArgumentException("Invalid table or column name. Only letters, numbers, and underscores are allowed.");
            }

            string query = $"PRAGMA table_info({tableName})";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == columnName)
                        {
                            Console.WriteLine($"SQLite Manager: Column {columnName} exists in table {tableName}.");
                            return true;
                        }
                    }
                }
            }
            Console.WriteLine($"SQLite Manager: Column {columnName} does not exist in table {tableName}.");
            return false;
        }
        /// <summary>
        /// Checks if multiple columns exist in a specified table.
        /// Returns a list of existing column names.
        /// </summary>
        public static List<string> ColumnsExist(SQLiteConnection connection, string tableName, params string[] columnNames)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (columnNames == null || columnNames.Length == 0)
                throw new ArgumentNullException(nameof(columnNames));
            if (!IsValidName(tableName))
            {
                throw new ArgumentException("Invalid table name. Only letters, numbers, and underscores are allowed.");
            }

            foreach (var columnName in columnNames)
            {
                if (!IsValidName(columnName))
                {
                    throw new ArgumentException($"Invalid column name: {columnName}. Only letters, numbers, and underscores are allowed.");
                }
            }
            List<string> existingColumns = new List<string>();
            string query = $"PRAGMA table_info({tableName})";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foreach (var columnName in columnNames)
                        {
                            // Check if the column name matches
                            if (reader["name"].ToString() == columnName)
                            {
                                Console.WriteLine($"SQLite Manager: Column {columnName} exists in table {tableName}.");
                                existingColumns.Add(columnName);
                            }
                        }
                    }
                }
            }
            return existingColumns;
        }
    }
}