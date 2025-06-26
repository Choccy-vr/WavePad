using System.Text.Json;
namespace WeatherService
{
    public static class Location
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public static async Task<LocationData> GetLocationFromIPAsync(string IP)
        {
            try
            {
                // Using ip-api.com (free, no API key needed)
                string response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{IP}");
                var json = JsonSerializer.Deserialize<JsonElement>(response);

                return new LocationData
                {
                    Latitude = json.GetProperty("lat").GetDouble(),
                    Longitude = json.GetProperty("lon").GetDouble(),
                    City = json.GetProperty("city").GetString() ?? "",
                    Country = json.GetProperty("country").GetString() ?? "",
                    Region = json.GetProperty("region").GetString() ?? "",
                    RegionName = json.GetProperty("regionName").GetString() ?? "",
                    IP = json.GetProperty("query").GetString() ?? "",
                    Status = json.GetProperty("status").GetString() ?? "",
                    Continent = json.GetProperty("continent").GetString() ?? "",
                    ContinentCode = json.GetProperty("continentCode").GetString() ?? "",
                    CountryCode = json.GetProperty("countryCode").GetString() ?? "",
                    Zip = json.GetProperty("zip").GetString() ?? "",
                    Timezone = json.GetProperty("timezone").GetString() ?? "",
                    Offset = json.GetProperty("offset").GetInt32(),
                    currency = json.GetProperty("currency").GetString() ?? ""

                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get location from IP: {ex.Message}");
            }
        }
        public static async Task<string> GetMyIPAsync()
        {
            try
            {
                string ip = await _httpClient.GetStringAsync("https://api.ipify.org");
                return ip.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get IP: {ex.Message}");
            }
        }
    }
    public class LocationData
    {
        public required string IP { get; set; }
        public required string Status { get; set; }
        public required string Continent { get; set; }
        public required string ContinentCode { get; set; }
        public required string Country { get; set; }
        public required string CountryCode { get; set; }
        public required string Region { get; set; }
        public required string RegionName { get; set; }
        public required string City { get; set; }
        public required string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Timezone { get; set; }
        public int Offset { get; set; }
        public required string currency { get; set; }
    }
}