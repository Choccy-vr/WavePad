import 'dart:io';
import 'dart:convert';
import 'package:intl/intl.dart';

class CurrentWeatherData {
  final String lastUpdated;
  final double tempature;

  CurrentWeatherData({required this.lastUpdated, required this.tempature});

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
    );
  }
}

class DailyWeatherData {
  final String date;
  final double maxTemperature;
  final double minTemperature;

  DailyWeatherData({
    required this.date,
    required this.maxTemperature,
    required this.minTemperature,
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
    );
  }
}

class LocationData {
  final String City;

  LocationData({required this.City});

  factory LocationData.fromJson(Map<String, dynamic> json) {
    if (json.isEmpty) {
      throw ArgumentError('JSON data is empty');
    }
    return LocationData(City: json['city'] as String);
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
      data: ['last_updated', 'tempature'],
    );
    print('Response from WaveDB: $result');
    final weather = CurrentWeatherData.fromJson(result);
    return weather;
  } catch (e) {
    throw Exception('Failed to fetch current weather data: $e');
  }
}

Future<DailyWeatherData> GetDayWeather() async {
  final databasePath = '/var/lib/WaveOS/weather.wvdb';
  final today = DateFormat('yyyy-MM-dd').format(DateTime.now());
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
    final todayWeather = weather.firstWhere(
      (w) => w.date.startsWith(today),
      orElse: () => throw Exception('No weather data for today'),
    );
    return todayWeather;
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
      data: ['city'],
    );
    print('Response from WaveDB: $result');
    final location = LocationData.fromJson(result);
    return location;
  } catch (e) {
    throw Exception('Failed to fetch location data: $e');
  }
}
