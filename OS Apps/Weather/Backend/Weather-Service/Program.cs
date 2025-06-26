using System.Threading.Tasks;
using SQLite_Manager;
using System.Data.SQLite;
namespace WeatherService
{
    class Program
    {
        private static string _dbPath = "/var/lib/WaveOS/weather.wvdb";
        private static string _weatherTable = "weather_data";
        private static string _locationTable = "location_data";
        private static SQLiteConnection _connection;

        static async Task Main(string[] args)
        {

            LocationData locationData = await Location.GetLocationFromIPAsync(await Location.GetMyIPAsync());
            WeatherData weatherData = await Weather.GetCurrentWeatherAsync(locationData.Latitude, locationData.Longitude);
            Console.WriteLine($"Current Weather in {weatherData.Location}:");
            Console.WriteLine($"Temperature: {Weather.ConvertCelsiusToFahrenheit(weatherData.Temperature)}°F");
            Console.WriteLine($"Humidity: {weatherData.Humidity}%");
            Console.WriteLine($"Wind Speed: {weatherData.WindSpeed} mph");
            Console.WriteLine($"Wind Direction: {weatherData.WindDirection}°");
            Console.WriteLine($"Weather Code: {weatherData.WeatherCode}");
            Console.WriteLine($"Description: {weatherData.Description}");
            Console.WriteLine($"Last Updated: {weatherData.LastUpdated}");
            Console.WriteLine("Weather data retrieval complete.");
        }
    }
}