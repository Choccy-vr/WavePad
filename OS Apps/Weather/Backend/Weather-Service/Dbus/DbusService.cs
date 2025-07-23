using Tmds.DBus;
namespace WeatherService
{
    public class DBusService : IWeatherService
    {
        private WeatherData? _currentWeather;
        private List<HourlyWeatherData> _hourlyWeather = new();
        private List<DailyWeatherData> _dailyWeather = new();
        private LocationData? _locationData;

        public ObjectPath ObjectPath => "/org/waveOS/Weather";

        internal void SetCurrentWeather(WeatherData weather)
        {
            _currentWeather = weather;
        }

        internal void AddHourlyWeather(HourlyWeatherData weather)
        {
            // Remove existing entry for the same time to avoid duplicates
            _hourlyWeather.RemoveAll(h => h.Time == weather.Time);
            _hourlyWeather.Add(weather);
        }

        internal void AddDailyWeather(DailyWeatherData weather)
        {
            // Remove existing entry for the same date to avoid duplicates
            _dailyWeather.RemoveAll(d => d.Date.Date == weather.Date.Date);
            _dailyWeather.Add(weather);
        }

        internal void SetHourlyWeatherList(List<HourlyWeatherData> hourlyData)
        {
            _hourlyWeather = hourlyData;
        }

        internal void SetDailyWeatherList(List<DailyWeatherData> dailyData)
        {
            _dailyWeather = dailyData;
        }
        internal void SetLocation(LocationData location)
        {
            _locationData = location;
        }

        internal void ClearHourlyWeather()
        {
            _hourlyWeather.Clear();
        }

        internal void ClearDailyWeather()
        {
            _dailyWeather.Clear();
        }

        Task<WeatherData> IWeatherService.GetCurrentWeatherAsync()
        {
            if (_currentWeather == null)
                throw new InvalidOperationException("No current weather data available");

            return Task.FromResult(_currentWeather);
        }

        Task<HourlyWeatherData[]> IWeatherService.GetHourlyWeatherAsync()
        {
            return Task.FromResult(_hourlyWeather.ToArray());
        }

        public Task<HourlyWeatherData> GetHourlyWeatherByTimeAsync(DateTime time)
        {
            var exactMatch = _hourlyWeather.FirstOrDefault(h => h.Time == time);
            if (exactMatch != null)
                return Task.FromResult(exactMatch);
            throw new InvalidOperationException("No hourly weather data available for the specified time.");
        }

        Task<DailyWeatherData[]> IWeatherService.GetDailyWeatherAsync()
        {
            return Task.FromResult(_dailyWeather.ToArray());
        }

        public Task<DailyWeatherData> GetDailyWeatherByDateAsync(DateTime date)
        {
            var targetDate = date.Date;

            var weatherData = _dailyWeather.FirstOrDefault(d => d.Date.Date == targetDate);

            if (weatherData == null)
                throw new InvalidOperationException($"No daily weather data found for date: {targetDate:yyyy-MM-dd}");

            return Task.FromResult(weatherData);
        }

        public Task<LocationData> GetLocationAsync()
        {
            if (_locationData == null)
                throw new InvalidOperationException("No location data available");

            return Task.FromResult(_locationData);
        }
    }
}