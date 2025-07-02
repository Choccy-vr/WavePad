using System.Text.Json;

namespace WeatherService
{
    public static class Weather
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _baseUrl = "https://api.open-meteo.com/v1";

        public static async Task<WeatherData> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                string url = $"{_baseUrl}/forecast?" +
                           $"latitude={latitude}&longitude={longitude}" +
                           "&current=temperature_2m,relative_humidity_2m,apparent_temperature,is_day,precipitation,weather_code,wind_speed_10m,wind_direction_10m,uv_index&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch";

                string response = await _httpClient.GetStringAsync(url);
                var json = JsonSerializer.Deserialize<JsonElement>(response);

                var current = json.GetProperty("current");
                
                return new WeatherData
                {
                    LastUpdated = DateTime.Parse(current.GetProperty("time").GetString() ?? DateTime.UtcNow.ToString()),
                    Temperature = current.GetProperty("temperature_2m").GetDouble(),
                    Humidity = current.GetProperty("relative_humidity_2m").GetInt32(),
                    ApparentTemperature = current.GetProperty("apparent_temperature").GetDouble(),
                    IsDay = current.GetProperty("is_day").GetInt32() == 1,
                    Precipitation = current.GetProperty("precipitation").GetDouble(),
                    WeatherCode = current.GetProperty("weather_code").GetInt32(),
                    WindSpeed = current.GetProperty("wind_speed_10m").GetDouble(),
                    WindDirection = current.GetProperty("wind_direction_10m").GetDouble(),
                    UVIndex = current.GetProperty("uv_index").GetDouble(),
                    Description = GetWeatherDescription(current.GetProperty("weather_code").GetInt32()),
                    Location = $"{latitude:F2}°, {longitude:F2}°"
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get current weather: {ex.Message}");
            }
        }

        public static async Task<List<DailyWeatherData>> GetDailyWeatherAsync(double latitude, double longitude)
        {
            try
            {
                string url = $"{_baseUrl}/forecast?" +
                           $"latitude={latitude}&longitude={longitude}" +
                           "&daily=weather_code,temperature_2m_max,temperature_2m_min,uv_index_max,wind_speed_10m_max" + 
                           "&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch";

                string response = await _httpClient.GetStringAsync(url);
                var json = JsonSerializer.Deserialize<JsonElement>(response);

                var daily = json.GetProperty("daily");
                var times = daily.GetProperty("time").EnumerateArray().ToArray();
                var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray().ToArray();
                var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray().ToArray();
                var uvIndexes = daily.GetProperty("uv_index_max").EnumerateArray().ToArray();
                var windSpeeds = daily.GetProperty("wind_speed_10m_max").EnumerateArray().ToArray();
                var weatherCodes = daily.GetProperty("weather_code").EnumerateArray().ToArray();
                var forecasts = new List<DailyWeatherData>();
                
                for (int i = 0; i < times.Length; i++)
                {
                    int weatherCode = weatherCodes[i].GetInt32();
                    forecasts.Add(new DailyWeatherData
                    {
                        Date = DateTime.Parse(times[i].GetString() ?? DateTime.UtcNow.ToString()),
                        MaxTemperature = maxTemps[i].GetDouble(),
                        MinTemperature = minTemps[i].GetDouble(),
                        WeatherCode = weatherCode,
                        Description = GetWeatherDescription(weatherCode),
                    });
                }

                return forecasts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get weather forecast: {ex.Message}");
            }
        }

        public static async Task<List<HourlyWeatherData>> GetHourlyWeatherAsync(double latitude, double longitude)
        {
            try
            {
                string url = $"{_baseUrl}/forecast?" +
                           $"latitude={latitude}&longitude={longitude}" +
                           "&hourly=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation_probability,weather_code,precipitation,uv_index,is_day" +
                           "&forecast_hours=24" +
                           "&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch";

                string response = await _httpClient.GetStringAsync(url);
                var json = JsonSerializer.Deserialize<JsonElement>(response);

                var hourly = json.GetProperty("hourly");
                var times = hourly.GetProperty("time").EnumerateArray().ToArray();
                var temperatures = hourly.GetProperty("temperature_2m").EnumerateArray().ToArray();
                var humidity = hourly.GetProperty("relative_humidity_2m").EnumerateArray().ToArray();
                var apparentTemperatures = hourly.GetProperty("apparent_temperature").EnumerateArray().ToArray();
                var precipitationProb = hourly.GetProperty("precipitation_probability").EnumerateArray().ToArray();
                var weatherCodes = hourly.GetProperty("weather_code").EnumerateArray().ToArray();
                var precipitation = hourly.GetProperty("precipitation").EnumerateArray().ToArray();
                var isDay = hourly.GetProperty("is_day").EnumerateArray().ToArray();

                
                

                var hourlyData = new List<HourlyWeatherData>();

                for (int i = 0; i < times.Length; i++)
                {
                    int weatherCode = weatherCodes[i].GetInt32();
                    hourlyData.Add(new HourlyWeatherData
                    {
                        Time = DateTime.Parse(times[i].GetString() ?? DateTime.UtcNow.ToString()),
                        Temperature = temperatures[i].GetDouble(),
                        Humidity = humidity[i].GetInt32(),
                        ApparentTemperature = apparentTemperatures[i].GetDouble(),
                        PrecipitationProbability = precipitationProb[i].GetInt32(),
                        WeatherCode = weatherCode,
                        precipitation = precipitation[i].GetDouble(),
                        IsDay = isDay[i].GetInt32() == 1,
                        Description = GetWeatherDescription(weatherCode),
                        
                    });
                }

                return hourlyData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get hourly weather: {ex.Message}");
            }
        }
        private static string GetWeatherDescription(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "Clear sky",
                1 => "Mainly clear",
                2 => "Partly cloudy",
                3 => "Overcast",
                45 => "Fog",
                48 => "Depositing rime fog",
                51 => "Light drizzle",
                53 => "Moderate drizzle",
                55 => "Dense drizzle",
                56 => "Light freezing drizzle",
                57 => "Dense freezing drizzle",
                61 => "Slight rain",
                63 => "Moderate rain",
                65 => "Heavy rain",
                66 => "Light freezing rain",
                67 => "Heavy freezing rain",
                71 => "Slight snow fall",
                73 => "Moderate snow fall",
                75 => "Heavy snow fall",
                77 => "Snow grains",
                80 => "Slight rain showers",
                81 => "Moderate rain showers",
                82 => "Violent rain showers",
                85 => "Slight snow showers",
                86 => "Heavy snow showers",
                95 => "Thunderstorm",
                96 => "Thunderstorm with slight hail",
                99 => "Thunderstorm with heavy hail",
                _ => "Unknown"
            };
        }    }

    // Data classes
    public class WeatherData
    {
        public DateTime LastUpdated { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double ApparentTemperature { get; set; }
        public bool IsDay { get; set; }
        public double Precipitation { get; set; }
        public int WeatherCode { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double UVIndex { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class DailyWeatherData
    {
        public DateTime Date { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double UVIndexMax { get; set; }
        public double WindSpeedMax { get; set; }
        public int WeatherCode { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class HourlyWeatherData
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double ApparentTemperature { get; set; }
        public int PrecipitationProbability { get; set; }
        public int WeatherCode { get; set; }
        public double precipitation { get; set; }
        public bool IsDay { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}