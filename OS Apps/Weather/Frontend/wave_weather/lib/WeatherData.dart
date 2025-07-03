import 'package:wave_weather/Weather.dart';
import 'dart:async';
import 'package:flutter/material.dart';
import 'package:material_symbols_icons/symbols.dart';

class WeatherIcon {
  final IconData icon;
  final Color color;

  WeatherIcon({required this.icon, required this.color});
}

class WeatherData {
  static CurrentWeatherData? currentWeather;
  static List<HourlyWeatherData> hourlyWeather = [];
  static List<DailyWeatherData> dailyWeather = [];
  static LocationData? locationData;

  static Timer? _currentTimer;
  static Timer? _weatherTimer;

  static Future<void> updateCurrent() async {
    currentWeather = await GetCurrentWeather();
  }

  static Future<void> updateHourly() async {
    hourlyWeather = await GetHourlyWeather();
  }

  static Future<void> updateDaily() async {
    dailyWeather = await GetDailyWeather();
  }

  static Future<void> updateLocation() async {
    locationData = await GetLocationData();
  }

  static Future<void> updateWeather() async {
    await updateCurrent();
    await updateHourly();
    await updateDaily();
    await updateLocation();
  }

  static void startAutoUpdate() {
    // Update current weather immediately
    updateCurrent();
    // Update current weather every minute
    _currentTimer?.cancel();
    _currentTimer = Timer.periodic(
      const Duration(minutes: 1),
      (_) => updateCurrent(),
    );

    // Update all weather every 15 minutes
    _weatherTimer?.cancel();
    _weatherTimer = Timer.periodic(
      const Duration(minutes: 15),
      (_) => updateWeather(),
    );
  }

  static void stopAutoUpdate() {
    _currentTimer?.cancel();
    _weatherTimer?.cancel();
  }

  static WeatherIcon GetWeatherIcon(int code, {bool isDay = true}) {
    switch (code) {
      case 0: // Clear
        return WeatherIcon(
          icon: isDay ? Symbols.sunny_rounded : Symbols.moon_stars_rounded,
          color: Colors.yellow,
        );
      case 1: // Mainly Clear
        return WeatherIcon(
          icon: isDay ? Symbols.sunny_rounded : Symbols.moon_stars_rounded,
          color: Colors.yellow,
        );
      case 2: // Partly Cloudy
        return WeatherIcon(
          icon: isDay
              ? Symbols.partly_cloudy_day_rounded
              : Symbols.partly_cloudy_night_rounded,
          color: Colors.yellow,
        );
      case 3: // Overcast
        return WeatherIcon(icon: Symbols.cloud_rounded, color: Colors.grey);
      case 45: // Fog
        return WeatherIcon(icon: Symbols.foggy_rounded, color: Colors.grey);
      case 48: // Depositing rime fog
        return WeatherIcon(icon: Symbols.foggy_rounded, color: Colors.grey);
      case 51: // Light Drizzle
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.blue,
        );
      case 53: // Moderate Drizzle
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.blue,
        );
      case 55: // Dense Drizzle
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.blue,
        );
      case 56: // Light Freezing Drizzle
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.cyan,
        );
      case 57: // Dense Freezing Drizzle
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.cyan,
        );
      case 61: // Slight Rain
        return WeatherIcon(
          icon: Symbols.rainy_light_rounded,
          color: Colors.blue,
        );
      case 63: // Moderate Rain
        return WeatherIcon(icon: Symbols.rainy_rounded, color: Colors.blue);
      case 65: // Heavy Rain
        return WeatherIcon(
          icon: Symbols.rainy_heavy_rounded,
          color: Colors.blue,
        );
      case 66: // Light Freezing Rain
        return WeatherIcon(icon: Symbols.rainy_rounded, color: Colors.cyan);
      case 67: // Heavy Freezing Rain
        return WeatherIcon(
          icon: Symbols.rainy_heavy_rounded,
          color: Colors.cyan,
        );
      case 71: // Slight Snow Fall
        return WeatherIcon(icon: Symbols.snowing_rounded, color: Colors.white);
      case 73: // Moderate Snow Fall
        return WeatherIcon(
          icon: Symbols.weather_snowy_rounded,
          color: Colors.white,
        );
      case 75: // Heavy Snow Fall
        return WeatherIcon(
          icon: Symbols.snowing_heavy_rounded,
          color: Colors.white,
        );
      case 77: // Snow Grains
        return WeatherIcon(icon: Symbols.snowing_rounded, color: Colors.white);
      case 80: // Slight Rain Showers
        return WeatherIcon(icon: Symbols.rainy_rounded, color: Colors.grey);
      case 81: // Moderate Rain Showers
        return WeatherIcon(icon: Symbols.rainy_rounded, color: Colors.grey);
      case 82: // Violent Rain Showers
        return WeatherIcon(icon: Symbols.rainy_rounded, color: Colors.grey);
      case 85: // Slight Snow Showers
        return WeatherIcon(
          icon: Symbols.weather_snowy_rounded,
          color: Colors.grey,
        );
      case 86: // Heavy Snow Showers
        return WeatherIcon(
          icon: Symbols.weather_snowy_rounded,
          color: Colors.grey,
        );
      case 95: // Thunderstorm
        return WeatherIcon(
          icon: Symbols.thunderstorm_rounded,
          color: Colors.grey,
        );
      case 96: // Thunderstorm with slight hail
        return WeatherIcon(
          icon: Symbols.weather_hail_rounded,
          color: Colors.grey,
        );
      case 99: // Thunderstorm with heavy hail
        return WeatherIcon(
          icon: Symbols.weather_hail_rounded,
          color: Colors.grey,
        );
      default:
        // Fallback icon for unknown weather codes
        return WeatherIcon(
          icon: Symbols.question_mark_rounded,
          color: Colors.grey,
        );
    }
  }
}
