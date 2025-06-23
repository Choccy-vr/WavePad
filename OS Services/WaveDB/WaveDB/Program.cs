using System.Data.SQLite;

namespace WaveDB
{
    class Program
    {
        static string SystemDBPath = "C:\\Users\\wante\\WavePad\\OS Services\\WaveDB\\WaveDB\\system.wvdb";
        // Production code 
        // static string SystemDBPath = "/var/lib/WaveOS/system.wvdb";

        // Table Names
        static string SystemInfoTable = "system_info";

        // System Info
        static string OSName = "WaveOS";
        static string OSVersion = "1.0.0";
        static string DeviceName = "WavePad";
        static bool IsSetupComplete = false;
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
                    TableBuilder.Timestamp("last_updated")
                );

            }
            else
            {
                Console.WriteLine("System info table already exists.");
            }

            // Check if system info data already exists, if not populate initial values
            var existingData = SQLite_Manager.ExecuteReader(connection, SystemInfoTable);
            if (existingData.Count == 0)
            {
                // Insert initial system info data
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "os_name", OSName);
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "os_version", OSVersion);
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "device_name", DeviceName);
                SQLite_Manager.InsertOrReplaceData(connection, SystemInfoTable, "setup_complete", IsSetupComplete.ToString());
                Console.WriteLine("Initial system info data populated.");
            }
            else
            {
                Console.WriteLine("System info data already exists.");
            }
            /*
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
            }*/

        }
        
        

    }
}
