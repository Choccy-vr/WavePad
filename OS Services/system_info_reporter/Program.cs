namespace SystemInfo
{
    class Program
    {
        static system_info? systemInfo;
        static device_info? deviceInfo;
        static async void Main(string[] args)
        {
            systemInfo = system_info_manager.GetSystemInfo()!;
            deviceInfo = system_info_manager.GetDeviceInfo()!;
            if (systemInfo != null)
            {
                Console.WriteLine($"OS Name: {systemInfo.OSName}");
                Console.WriteLine($"OS Version: {systemInfo.OSVersion}");
                Console.WriteLine($"Build Number: {systemInfo.buildNumber}");
                Console.WriteLine($"Ending Letter: {systemInfo.endingLetter}");
            }
            else
            {
                throw new Exception("System information file not found or invalid.");
            }
            if (deviceInfo != null)
            {
                Console.WriteLine($"Device Model: {deviceInfo.Model}");
                Console.WriteLine($"Device Version: {deviceInfo.Version}");
                Console.WriteLine($"Device Serial: {deviceInfo.Serial}");
            }
            else
            {
                throw new Exception("Device information file not found or invalid.");
            }
            await Database.SaveSystemInfoAsync(systemInfo);
            await Database.SaveDeviceInfoAsync(deviceInfo);
            Console.WriteLine("System and device information saved to database.");
        }
    }
}
