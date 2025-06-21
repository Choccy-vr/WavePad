using System.Data.SQLite;

namespace WaveDB
{
    class Program
    {
        static string SystemDBPath = "C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\system.wvdb";
        // Production code 
        // static string SystemDBPath = "/var/lib/WaveOS/system.db";

        // Table Names
        static string SystemInfoTable = "system_info";
        static void Main(string[] args)
        {

            HandleSystemInfo();
            
        }
        static void HandleSystemInfo()
        {
            //Create a new system database file if it does not exist
            if (!File.Exists(SystemDBPath))
            {
                Console.WriteLine("Database file does not exist. Creating a new system database...");
                SQLite_Manager.CreateDatabase(SystemDBPath);
            }

            //Open the system database
            SQLiteConnection connection = SQLite_Manager.OpenConnection(SystemDBPath);
            if (connection != null)
            {
                Console.WriteLine("Database connection established successfully.");
            }
            else
            {
                throw new Exception("Database connection failed.");
            }
            
            // Check if the system info exists
            if (!SQLite_Manager.isTableExist(connection, SystemInfoTable))
            {
                //Create the system info table if it does not exist
                SQLite_Manager.CreateTable(connection, SystemInfoTable,
                    TableBuilder.Id(),
                    TableBuilder.Text("property_name").NotNull().Unique(),
                    TableBuilder.Text("property_value").NotNull(),
                    TableBuilder.Integer("number").WithDefault("0"),
                    TableBuilder.Timestamp("last_updated")
                );

            }
            else
            {
                Console.WriteLine("System info table already exists.");
            }

            // Check if the system info table has the required columns
            if (SQLite_Manager.ColumnsExist(connection, SystemInfoTable, "os_name", "os_version").Contains("os_name"))
            {
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "os_name", "WaveOS");
            }
            else if (SQLite_Manager.ColumnsExist(connection, SystemInfoTable, "os_version").Contains("os_version"))
            {
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "os_version", "1.0.0");
            }
            else
            {
                Console.WriteLine("Fields already exist in the system info table.");
            }

            // Update the OS version
            SQLite_Manager.UpdateData(connection, SystemInfoTable, "os_version", "1.0.1");
            // Retrieve the OS name
            var osName = SQLite_Manager.GetData(connection, SystemInfoTable, "os_name");
            Console.WriteLine($"OS Name: {osName}");
            // Retrieve all system info
            var systemInfo = SQLite_Manager.ExecuteReader(connection, SystemInfoTable);
            Console.WriteLine($"Found {systemInfo.Count} rows in system info table.");
            foreach (var row in systemInfo)
            {
                Console.WriteLine("--- Row ---");
                foreach (var column in row)
                {
                    string columnName = column.Key;
                    object columnValue = column.Value;
                    Console.WriteLine($"{columnName}: {columnValue}");
                }
                Console.WriteLine();
            }

        }

    }
}
