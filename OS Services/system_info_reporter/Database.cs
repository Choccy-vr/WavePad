namespace SystemInfo
{
    class Database
    {
        private static readonly WaveDBClient _dbClient = new WaveDBClient();
        private static bool _isInitialized = false;

        public static async Task InitializeAsync()
        {
            if (_isInitialized) return;

            await _dbClient.ConnectAsync();

            // Define columns for the system_info table
            var systemInfoColumns = new[]
            {
                new { name = "OSName", type = "TEXT" },
                new { name = "OSVersion", type = "REAL" },
                new { name = "buildNumber", type = "INTEGER" },
                new { name = "endingLetter", type = "TEXT" }
            };
            // Define columns for the device_info table
            var deviceInfoColumns = new[]
            {
                new { name = "Model", type = "TEXT" },
                new { name = "Version", type = "REAL" },
                new { name = "Serial", type = "TEXT" }
            };
            // Create the table if it doesn't exist
            await _dbClient.CreateAsync("SystemInfo", "system_info", systemInfoColumns);
            await _dbClient.CreateAsync("SystemInfo", "device_info", deviceInfoColumns);

            _isInitialized = true;
        }

        public static async Task SaveSystemInfoAsync(system_info info)
        {
            if (!_isInitialized) await InitializeAsync();

            // Write the system_info object as a new row
            await _dbClient.WriteRowAsync("SystemInfo", "system_info", info);
        }
        public static async Task SaveDeviceInfoAsync(device_info info)
        {
            if (!_isInitialized) await InitializeAsync();

            // Write the device_info object as a new row
            await _dbClient.WriteRowAsync("SystemInfo", "device_info", info);
        }
    }
}