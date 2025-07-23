using Tmds.DBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherService;

[DBusInterface("org.waveOS.Weather")]
public interface IWeatherService : IDBusObject
{
    Task<WeatherData> GetCurrentWeatherAsync();

    Task<HourlyWeatherData[]> GetHourlyWeatherAsync();

    Task<HourlyWeatherData> GetHourlyWeatherByTimeAsync(DateTime time);

    Task<DailyWeatherData[]> GetDailyWeatherAsync();

    Task<DailyWeatherData> GetDailyWeatherByDateAsync(DateTime date);
    
    Task<LocationData> GetLocationAsync();
}