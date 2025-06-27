using System.IO.Pipes;
using System.Text;
using System.Text.Json;
namespace WeatherService
{
    public static class Database
    {
        private static string _weatherDBPath = "/var/lib/WaveOS/weather.wvdb";
        private static string _locationDBPath = "/var/lib/WaveOS/location.wvdb";
        private static string _currentWeatherTable = "current_weather_data";
        private static string _hourlyWeatherTable = "hour_weather_data";
        private static string _dailyWeatherTable = "daily_weather_data";
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
        public static async Task<bool> SendCurrentWeatherToWaveDBAsync(WeatherData weatherData)
        {
            try
            {
                var request = new
                {
                    action = "WRITE",
                    database = _weatherDBPath,
                    table = _currentWeatherTable,
                    data = new
                    {
                        last_updated = weatherData.LastUpdated.ToString("o"),
                        tempature = weatherData.Temperature,
                        humidity = weatherData.Humidity,
                        apparent_tempature = weatherData.ApparentTemperature,
                        is_day = weatherData.IsDay,
                        precipitation = weatherData.Precipitation,
                        weather_code = weatherData.WeatherCode,
                        weather_description = weatherData.Description,
                        wind_speed = weatherData.WindSpeed,
                        wind_direction = weatherData.WindDirection,
                        uv_index = weatherData.UVIndex,
                        location = weatherData.Location,
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
        public static async Task<bool> SendHourlyWeatherToWaveDBAsync(HourlyWeatherData weatherData)
        {
            try
            {
                var request = new
                {
                    action = "WRITE_ROW",
                    database = _weatherDBPath,
                    table = _hourlyWeatherTable,
                    data = new
                    {
                        time = weatherData.Time.ToString("o"),
                        tempature = weatherData.Temperature,
                        humidity = weatherData.Humidity,
                        apparent_tempature = weatherData.ApparentTemperature,
                        precipitation_probability = weatherData.PrecipitationProbability,
                        weather_code = weatherData.WeatherCode,
                        precipitation = weatherData.precipitation,
                        is_day = weatherData.IsDay,
                        weather_description = weatherData.Description
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
        public static async Task<bool> SendDailyWeatherToWaveDBAsync(DailyWeatherData weatherData)
        {
            try
            {
                var request = new
                {
                    action = "WRITE_ROW",
                    database = _weatherDBPath,
                    table = _dailyWeatherTable,
                    data = new
                    {
                        date = weatherData.Date.ToString("o"),
                        tempature_min = weatherData.MinTemperature,
                        tempature_max = weatherData.MaxTemperature,
                        weather_code = weatherData.WeatherCode,
                        weather_description = weatherData.Description,
                    }
                };

                string requestJson = JsonSerializer.Serialize(request);
                string responseJson = await SendToWaveDBAsync(requestJson);

                var response = JsonSerializer.Deserialize<JsonElement>(responseJson);
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send daily weather data to WaveDB: {ex.Message}");
            }
        }
        public static async Task<bool> CreateWeatherDatabaseAsync()
        {
            try
            {
                var request = new
                {
                    action = "CREATE",
                    database = _weatherDBPath,
                    tables = new[]
                    {
                        new
                        {
                            name = _currentWeatherTable,
                            columns = new[]
                            {
                                new { name = "last_updated", type = "TEXT" },
                                new { name = "tempature", type = "REAL" },
                                new { name = "humidity", type = "REAL" },
                                new { name = "apparent_tempature", type = "REAL" },
                                new { name = "is_day", type = "INTEGER" },
                                new { name = "precipitation", type = "REAL" },
                                new { name = "weather_code", type = "INTEGER" },
                                new { name = "weather_description", type = "TEXT" },
                                new { name = "wind_speed", type = "REAL" },
                                new { name = "wind_direction", type = "TEXT" },
                                new { name = "uv_index", type = "REAL" },
                                new { name = "location", type = "TEXT" }
                            }
                        },
                        new
                        {
                            name = _hourlyWeatherTable,
                            columns = new[]
                            {
                                new { name = "time", type = "TEXT" },
                                new { name = "tempature", type = "REAL" },
                                new { name = "humidity", type = "REAL" },
                                new { name = "apparent_tempature", type = "REAL" },
                                new { name = "precipitation_probability", type = "REAL" },
                                new { name = "weather_code", type = "INTEGER" },
                                new { name = "precipitation", type = "REAL" },
                                new { name = "is_day", type = "INTEGER" },
                                new { name = "weather_description", type = "TEXT" }
                            }
                        },
                        new
                        {
                            name = _dailyWeatherTable,
                            columns = new[]
                            {
                                new { name = "date", type = "TEXT" },
                                new { name = "tempature_min", type = "REAL" },
                                new { name = "tempature_max", type = "REAL" },
                                new { name = "weather_code", type = "INTEGER" },
                                new { name = "weather_description", type = "TEXT" }
                            }
                        }
                    }
                        
                    };

                string requestJson = JsonSerializer.Serialize(request);
                string responseJson = await SendToWaveDBAsync(requestJson);

                var response = JsonSerializer.Deserialize<JsonElement>(responseJson);
                return response.GetProperty("success").GetBoolean();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create weather database: {ex.Message}");
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