using Tmds.DBus;
using System.Net.NetworkInformation;
namespace WeatherService
{
    class Program
    {
        private static LocationData oldLocation = new LocationData()
        {
            Latitude = 0,
            Longitude = 0,
            City = "",
            Country = "",
            Region = "",
            RegionName = "",
            IP = "",
            Status = "",
            Continent = "",
            ContinentCode = "",
            CountryCode = "",
            Zip = "",
            Timezone = "",
            Offset = 0,
            currency = ""
        };
        private static DBusService? _dbusService;
        static async Task Main(string[] args)
        {
            // Initialize the DBus service
            await InitializeDBusService();
            // Update the weather data immediately
            await UpdateWeather();

            // Start both timers concurrently
            var weatherTask = Task.Run(async () =>
            {
                using var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));
                while (await timer.WaitForNextTickAsync())
                {
                    await UpdateWeather();
                }
            });

            var currentWeatherTask = Task.Run(async () =>
            {
                using var currentTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
                while (await currentTimer.WaitForNextTickAsync())
                {
                    await UpdateCurrentWeather();
                }
            });

            await Task.WhenAll(weatherTask, currentWeatherTask);
            //Nothing runs below this point

        }
        private static async Task InitializeDBusService()
        {
            try
            {
                var connection = new Connection(Address.Session);
                await connection.ConnectAsync();

                _dbusService = new DBusService();
                await connection.RegisterObjectAsync(_dbusService);
                await connection.RegisterServiceAsync("org.waveOS.Weather");

                Console.WriteLine("D-Bus weather service registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize D-Bus service: {ex.Message}");
                throw;
            }
        }
        public static async Task<LocationData> GetLocation()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                LocationData location = await Location.GetLocationFromIPAsync(await Location.GetMyIPAsync());
                if (location != null && location != oldLocation)
                {
                    if (location.Latitude != 0 && location.Longitude != 0)
                    {
                        if (location.Latitude != oldLocation.Latitude || location.Longitude != oldLocation.Longitude)
                        {
                            oldLocation = location;
                            _dbusService?.SetLocation(location);
                        }
                        return location;
                    }
                    else
                    {
                        Console.WriteLine("Invalid location data received");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve location data");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("No internet connection");
                return null;
            }
        }
        public static async Task UpdateWeather()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                LocationData location = await GetLocation();
                if (location != null)
                {
                    WeatherData current = await Weather.GetCurrentWeatherAsync(location.Latitude, location.Longitude);
                    List<DailyWeatherData> daily = await Weather.GetDailyWeatherAsync(location.Latitude, location.Longitude);
                    List<HourlyWeatherData> hourly = await Weather.GetHourlyWeatherAsync(location.Latitude, location.Longitude);
                    _dbusService?.SetCurrentWeather(current);
                    _dbusService?.ClearHourlyWeather();
                    _dbusService?.ClearDailyWeather();
                    _dbusService?.SetHourlyWeatherList(hourly);
                    _dbusService?.SetDailyWeatherList(daily);
                    
                }
            }
            else
            {
                Console.WriteLine("No internet connection");
            }
        }
        public static async Task UpdateCurrentWeather()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (oldLocation != null)
                {
                    WeatherData current = await Weather.GetCurrentWeatherAsync(oldLocation.Latitude, oldLocation.Longitude);
                    _dbusService?.SetCurrentWeather(current);
                }
            }
            else
            {
                Console.WriteLine("No internet connection");
            }
        }
    }

}