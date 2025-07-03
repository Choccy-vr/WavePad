import 'dart:io';
import 'dart:convert';

class CurrentWeatherData {
  final String lastUpdated;
  final double tempature;
  final int humidity;
  final double ApparentTemperature;
  final bool isDay;
  final double precipitation;
  final int weatherCode;
  final String weatherDescription;
  final double windSpeed;
  final double windDirection;
  final double uvIndex;

  CurrentWeatherData({
    required this.lastUpdated,
    required this.tempature,
    required this.humidity,
    required this.ApparentTemperature,
    required this.isDay,
    required this.precipitation,
    required this.weatherCode,
    required this.weatherDescription,
    required this.windSpeed,
    required this.windDirection,
    required this.uvIndex,
  });

  factory CurrentWeatherData.fromJson(Map<String, dynamic> json) {
    if (json.isEmpty) {
      throw ArgumentError('JSON data is empty');
    }
    if (json['success'] == false) {
      throw Exception(
        'Failed to fetch weather data: ${json['error'] ?? 'Unknown error'}',
      );
    }
    return CurrentWeatherData(
      lastUpdated: json['last_updated'] as String,
      tempature: (json['tempature'] as num).toDouble(),
      humidity: (json['humidity'] as num).toInt(),
      ApparentTemperature: (json['apparent_temperature'] as num).toDouble(),
      isDay: json['is_day'] == 1 || json['is_day'] == true,
      precipitation: (json['precipitation'] as num).toDouble(),
      weatherCode: json['weather_code'] as int,
      weatherDescription: json['weather_description'] as String,
      windSpeed: (json['wind_speed'] as num).toDouble(),
      windDirection: (json['wind_direction'] as num).toDouble(),
      uvIndex: (json['uv_index'] as num).toDouble(),
    );
  }
}

class HourlyWeatherData {
  final String time;
  final double tempature;
  final int humidity;
  final double ApparentTemperature;
  final int precipitation_probability;
  final double precipitation;
  final int weatherCode;
  final String weatherDescription;
  final bool isDay;

  HourlyWeatherData({
    required this.time,
    required this.tempature,
    required this.humidity,
    required this.ApparentTemperature,
    required this.precipitation_probability,
    required this.precipitation,
    required this.weatherCode,
    required this.weatherDescription,
    required this.isDay,
  });

  factory HourlyWeatherData.fromJson(Map<String, dynamic> json) {
    if (json.isEmpty) {
      throw ArgumentError('JSON data is empty');
    }
    if (json['success'] == false) {
      throw Exception(
        'Failed to fetch weather data: ${json['error'] ?? 'Unknown error'}',
      );
    }
    return HourlyWeatherData(
      time: json['time'] as String,
      tempature: (json['tempature'] as num).toDouble(),
      humidity: (json['humidity'] as num).toInt(),
      ApparentTemperature: (json['apparent_temperature'] as num).toDouble(),
      precipitation_probability: (json['precipitation_probability'] as num)
          .toInt(),
      precipitation: (json['precipitation'] as num).toDouble(),
      weatherCode: json['weather_code'] as int,
      weatherDescription: json['weather_description'] as String,
      isDay: json['is_day'] == 1 || json['is_day'] == true,
    );
  }
}

class DailyWeatherData {
  final String date;
  final double maxTemperature;
  final double minTemperature;
  final double uvIndexMax;
  final double windSpeedMax;
  final int weatherCode;
  final String weatherDescription;

  DailyWeatherData({
    required this.date,
    required this.maxTemperature,
    required this.minTemperature,
    required this.uvIndexMax,
    required this.windSpeedMax,
    required this.weatherCode,
    required this.weatherDescription,
  });

  factory DailyWeatherData.fromJson(Map<String, dynamic> json) {
    if (json.isEmpty) {
      throw ArgumentError('JSON data is empty');
    }
    if (json['success'] == false) {
      throw Exception(
        'Failed to fetch weather data: ${json['error'] ?? 'Unknown error'}',
      );
    }
    return DailyWeatherData(
      date: json['date'] as String,
      maxTemperature: (json['max_temperature'] as num).toDouble(),
      minTemperature: (json['min_temperature'] as num).toDouble(),
      uvIndexMax: (json['uv_index_max'] as num).toDouble(),
      windSpeedMax: (json['wind_speed_max'] as num).toDouble(),
      weatherCode: json['weather_code'] as int,
      weatherDescription: json['weather_description'] as String,
    );
  }
}

class LocationData {
  final double latitude;
  final double longitude;
  final String Continent;
  final String ContinentCode;
  final String Country;
  final String CountryCode;
  final String Region;
  final String RegionName;
  final String City;
  final String Zip;
  final String Timezone;

  LocationData({
    required this.latitude,
    required this.longitude,
    required this.Continent,
    required this.ContinentCode,
    required this.Country,
    required this.CountryCode,
    required this.Region,
    required this.RegionName,
    required this.City,
    required this.Zip,
    required this.Timezone,
  });

  factory LocationData.fromJson(Map<String, dynamic> json) {
    if (json.isEmpty) {
      throw ArgumentError('JSON data is empty');
    }
    return LocationData(
      latitude: json['latitude'] as double,
      longitude: json['longitude'] as double,
      Continent: json['continent'] as String,
      ContinentCode: json['continent_code'] as String,
      Country: json['country'] as String,
      CountryCode: json['country_code'] as String,
      Region: json['region'] as String,
      RegionName: json['region_name'] as String,
      City: json['city'] as String,
      Zip: json['zip'] as String,
      Timezone: json['timezone'] as String,
    );
  }
}

Future<Map<String, dynamic>> sendToWaveDB({
  required String action,
  required String database,
  required String table,
  required List<String> data,
  String pipeName = 'WaveDB_Pipe',
  Duration timeout = const Duration(seconds: 5),
}) async {
  final request = {
    'action': action,
    'database': database,
    'table': table,
    'data': data,
  };

  final requestJson = jsonEncode(request);

  // Write request to the named pipe
  final pipe = await File(
    '/tmp/$pipeName',
  ).open(mode: FileMode.write).timeout(timeout);
  await pipe.writeString(requestJson).timeout(timeout);
  await pipe.flush().timeout(timeout);
  await pipe.close().timeout(timeout);

  // Read response from the named pipe
  final responsePipe = await File(
    '/tmp/$pipeName',
  ).open(mode: FileMode.read).timeout(timeout);
  final responseBytes = await responsePipe.read(4096).timeout(timeout);
  await responsePipe.close().timeout(timeout);

  final responseJson = utf8.decode(responseBytes);
  return jsonDecode(responseJson) as Map<String, dynamic>;
}

Future<CurrentWeatherData> GetCurrentWeather() async {
  final databasePath = '/var/lib/WaveOS/weather.wvdb';
  try {
    final result = await sendToWaveDB(
      action: 'READ',
      database: databasePath,
      table: 'current_weather_data',
      data: [
        'last_updated',
        'tempature',
        'humidity',
        'apparent_tempature',
        'is_day',
        'precipitation',
        'weather_code',
        'weather_description',
        'wind_speed',
        'wind_direction',
        'uv_index',
      ],
    );
    print('Response from WaveDB: $result');
    final weather = CurrentWeatherData.fromJson(result);
    return weather;
  } catch (e) {
    throw Exception('Failed to fetch current weather data: $e');
  }
}

Future<List<HourlyWeatherData>> GetHourlyWeather() async {
  final databasePath = '/var/lib/WaveOS/weather.wvdb';
  try {
    final result = await sendToWaveDB(
      action: 'READ_ROWS',
      database: databasePath,
      table: 'hour_weather_data',
      data: [],
    );
    print('Response from WaveDB: $result');
    final weather = (result['data'] as List)
        .map((item) => HourlyWeatherData.fromJson(item))
        .toList();
    return weather;
  } catch (e) {
    throw Exception('Failed to fetch hourly weather data: $e');
  }
}

Future<List<DailyWeatherData>> GetDailyWeather() async {
  final databasePath = '/var/lib/WaveOS/weather.wvdb';
  try {
    final result = await sendToWaveDB(
      action: 'READ_ROWS',
      database: databasePath,
      table: 'daily_weather_data',
      data: [],
    );
    print('Response from WaveDB: $result');
    final weather = (result['data'] as List)
        .map((item) => DailyWeatherData.fromJson(item))
        .toList();
    return weather;
  } catch (e) {
    throw Exception('Failed to fetch daily weather data: $e');
  }
}

Future<LocationData> GetLocationData() async {
  final databasePath = '/var/lib/WaveOS/location.wvdb';
  try {
    final result = await sendToWaveDB(
      action: 'READ',
      database: databasePath,
      table: 'location_data',
      data: [
        'latitude',
        'longitude',
        'continent',
        'continent_code',
        'country',
        'country_code',
        'region',
        'region_name',
        'city',
        'zip',
        'timezone',
      ],
    );
    print('Response from WaveDB: $result');
    final location = LocationData.fromJson(result);
    return location;
  } catch (e) {
    throw Exception('Failed to fetch location data: $e');
  }
}
