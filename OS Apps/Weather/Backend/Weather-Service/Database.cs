using System.Data.SQLite;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
namespace WeatherService
{
    public static class Database
    {
        private static string _weatherDBPath = "/var/lib/WaveOS/weather.wvdb";
        private static string _locationDBPath = "/var/lib/WaveOS/location.wvdb";
        private static string _weatherTable = "weather_data";
        private static string _locationTable = "location_data";
        private static string _pipeName = "WaveDB_Pipe";

        public static async Task<string> SendToWaveDBAsync(string requestJson)
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut))
                {
                    // Connect to the WaveDB pipe server
                    await pipeClient.ConnectAsync(5000); // 5 seconds timeout
                    Console.WriteLine("Connected to WaveDB pipe server.");

                    // Send the request JSON to the WaveDB server
                    byte[] requestBytes = Encoding.UTF8.GetBytes(requestJson);

                    // send the request
                    await pipeClient.WriteAsync(requestBytes, 0, requestBytes.Length);
                    await pipeClient.FlushAsync();
                    Console.WriteLine($"Request sent to WaveDB pipe server. {requestJson}");

                    // Read the response from the WaveDB server
                    byte[] responseBytes = new byte[4096];
                    int bytesRead = await pipeClient.ReadAsync(responseBytes, 0, responseBytes.Length);
                    string responseJson = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                    Console.WriteLine($"Response received from WaveDB pipe server: {responseJson}");
                    return responseJson;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to communicate with WaveDB pipe server: {ex.Message}");
            }
        }
        public static async Task<bool> SendWeatherToWaveDBAsync(WeatherData weatherData)
        {
            try
            {
                var request = new
                {
                    action = "WRITE",
                    database = _weatherDBPath,
                    table = _weatherTable,
                    data = new
                    {
                        temperature = weatherData.Temperature,
                        humidity = weatherData.Humidity,
                        wind_speed = weatherData.WindSpeed,
                        wind_direction = weatherData.WindDirection,
                        weather_code = weatherData.WeatherCode,
                        description = weatherData.Description,
                        last_updated = weatherData.LastUpdated.ToString("o"),
                        location = weatherData.Location
                    }
                };

                string requestJson = JsonSerializer.Serialize(request);
                string responseJson = await SendToWaveDBAsync(requestJson);

                var response = JsonSerializer.Deserialize<JsonElement>(responseJson);
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send weather data to WaveDB: {ex.Message}");
            }
        }
        public static async Task<bool> SendLocationToWaveDBAsync(LocationData locationData)
        {
            try
            {
                var request = new
                {
                    action = "WRITE",
                    database = _locationDBPath,
                    table = _locationTable,
                    data = new
                    {
                        latitude = locationData.Latitude,
                        longitude = locationData.Longitude,
                        city = locationData.City,
                        country = locationData.Country,
                        ip = locationData.IP,
                        region = locationData.Region,
                        region_name = locationData.RegionName,
                        status = locationData.Status,
                        continent = locationData.Continent,
                        continent_code = locationData.ContinentCode,
                        country_code = locationData.CountryCode,
                        zip = locationData.Zip,
                        timezone = locationData.Timezone,
                        offset = locationData.Offset
                    }
                };

                string requestJson = JsonSerializer.Serialize(request);
                string responseJson = await SendToWaveDBAsync(requestJson);

                var response = JsonSerializer.Deserialize<JsonElement>(responseJson);
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send location data to WaveDB: {ex.Message}");
            }
        }
    }
}