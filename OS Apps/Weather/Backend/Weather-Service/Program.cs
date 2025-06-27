using System.Diagnostics.CodeAnalysis;
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
        static async Task Main(string[] args)
        {
            if (!File.Exists("/var/lib/WaveOS/weather.wvdb"))
            {
                await Database.CreateWeatherDatabaseAsync();
            }
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
                            await Database.SendLocationToWaveDBAsync(location);
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
                    await Database.SendCurrentWeatherToWaveDBAsync(current);
                    foreach (var day in daily)
                    {
                        await Database.SendDailyWeatherToWaveDBAsync(day);
                    }
                    foreach (var hour in hourly)
                    {
                        await Database.SendHourlyWeatherToWaveDBAsync(hour);
                    }
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
                    await Database.SendCurrentWeatherToWaveDBAsync(current);
                }
            }
            else
            {
                Console.WriteLine("No internet connection");
            }
        }
    }

}