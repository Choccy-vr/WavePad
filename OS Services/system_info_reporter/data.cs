using System.Text.Json;

namespace SystemInfo
{
    public class system_info
    {
        public required string OSName { get; set; }
        public required double OSVersion { get; set; }
        public required int buildNumber { get; set; }
        public required char endingLetter { get; set; }

        // Add any other properties as needed
    }
    public class device_info
    {
        public required string Model { get; set; }
        public required double Version { get; set; }
        public required string Serial { get; set; }

        // Add any other properties as needed
    }
    public static class system_info_manager
    {
        static string jsonPath = "System_Info.json";

        public static system_info? GetSystemInfo()
        {
            if (!System.IO.File.Exists(jsonPath))
                return null;

            string json = System.IO.File.ReadAllText(jsonPath);
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                if (document.RootElement.TryGetProperty("system", out JsonElement systemElement))
                {
                    return JsonSerializer.Deserialize<system_info>(systemElement.GetRawText())!;
                }
            }
            return null;
        }

        public static device_info? GetDeviceInfo()
        {
            if (!System.IO.File.Exists(jsonPath))
                return null;

            string json = System.IO.File.ReadAllText(jsonPath);
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                if (document.RootElement.TryGetProperty("device", out JsonElement deviceElement))
                {
                    return JsonSerializer.Deserialize<device_info>(deviceElement.GetRawText())!;
                }
            }
            return null;
        }
    }
}