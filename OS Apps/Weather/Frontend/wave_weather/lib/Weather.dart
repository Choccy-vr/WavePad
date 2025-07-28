import 'package:dbus/dbus.dart';

class CurrentWeatherData {
  final String lastUpdated;
  final double temperature;
  final int humidity;
  final double apparentTemperature;
  final bool isDay;
  final double precipitation;
  final int weatherCode;
  final String weatherDescription;
  final double windSpeed;
  final double windDirection;
  final double uvIndex;

  CurrentWeatherData({
    required this.lastUpdated,
    required this.temperature,
    required this.humidity,
    required this.apparentTemperature,
    required this.isDay,
    required this.precipitation,
    required this.weatherCode,
    required this.weatherDescription,
    required this.windSpeed,
    required this.windDirection,
    required this.uvIndex,
  });
}

class HourlyWeatherData {
  final String time;
  final double temperature;
  final int humidity;
  final double apparentTemperature;
  final int precipitationProbability;
  final double precipitation;
  final int weatherCode;
  final String weatherDescription;
  final bool isDay;

  HourlyWeatherData({
    required this.time,
    required this.temperature,
    required this.humidity,
    required this.apparentTemperature,
    required this.precipitationProbability,
    required this.precipitation,
    required this.weatherCode,
    required this.weatherDescription,
    required this.isDay,
  });
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
}

class LocationData {
  final double latitude;
  final double longitude;
  final String continent;
  final String continentCode;
  final String country;
  final String countryCode;
  final String region;
  final String regionName;
  final String city;
  final String zip;
  final String timezone;

  LocationData({
    required this.latitude,
    required this.longitude,
    required this.continent,
    required this.continentCode,
    required this.country,
    required this.countryCode,
    required this.region,
    required this.regionName,
    required this.city,
    required this.zip,
    required this.timezone,
  });
}

class WeatherDBusClient {
  late DBusClient _client;
  late DBusRemoteObject _weatherObject;
  bool _initialized = false;

  Future<void> init() async {
    if (_initialized) return;

    _client = DBusClient.session();
    _weatherObject = DBusRemoteObject(
      _client,
      name: 'org.waveOS.Weather',
      path: DBusObjectPath('/org/waveOS/Weather'),
    );
    _initialized = true;
  }

  Future<CurrentWeatherData> getCurrentWeather() async {
    await init();

    try {
      final result = await _weatherObject.callMethod(
        'org.waveOS.Weather',
        'GetCurrentWeatherAsync',
        [],
      );

      // The D-Bus result should contain your WeatherData object
      final weatherStruct = result.returnValues[0] as DBusStruct;

      return CurrentWeatherData(
        lastUpdated: weatherStruct.children[11]
            .asString(), // DateTime as string
        temperature: weatherStruct.children[0].asDouble(),
        humidity: weatherStruct.children[1].asInt32(),
        apparentTemperature: weatherStruct.children[2].asDouble(),
        isDay: weatherStruct.children[3].asBoolean(),
        precipitation: weatherStruct.children[4].asDouble(),
        weatherCode: weatherStruct.children[5].asInt32(),
        weatherDescription: weatherStruct.children[6].asString(),
        windSpeed: weatherStruct.children[7].asDouble(),
        windDirection: weatherStruct.children[8].asDouble(),
        uvIndex: weatherStruct.children[9].asDouble(),
      );
    } catch (e) {
      throw Exception('Failed to fetch current weather data from D-Bus: $e');
    }
  }

  Future<List<HourlyWeatherData>> getHourlyWeather() async {
    await init();

    try {
      final result = await _weatherObject.callMethod(
        'org.waveOS.Weather',
        'GetHourlyWeatherAsync',
        [],
      );

      final weatherArray = result.returnValues[0] as DBusArray;

      return weatherArray.children.map((item) {
        final weatherStruct = item as DBusStruct;
        return HourlyWeatherData(
          time: weatherStruct.children[0].asString(), // DateTime as string
          temperature: weatherStruct.children[1].asDouble(),
          humidity: weatherStruct.children[2].asInt32(),
          apparentTemperature: weatherStruct.children[3].asDouble(),
          precipitationProbability: weatherStruct.children[4].asInt32(),
          weatherCode: weatherStruct.children[5].asInt32(),
          precipitation: weatherStruct.children[6].asDouble(),
          isDay: weatherStruct.children[7].asBoolean(),
          weatherDescription: weatherStruct.children[8].asString(),
        );
      }).toList();
    } catch (e) {
      throw Exception('Failed to fetch hourly weather data from D-Bus: $e');
    }
  }

  Future<List<DailyWeatherData>> getDailyWeather() async {
    await init();

    try {
      final result = await _weatherObject.callMethod(
        'org.waveOS.Weather',
        'GetDailyWeatherAsync',
        [],
      );

      final weatherArray = result.returnValues[0] as DBusArray;

      return weatherArray.children.map((item) {
        final weatherStruct = item as DBusStruct;
        return DailyWeatherData(
          date: weatherStruct.children[0].asString(), // DateTime as string
          maxTemperature: weatherStruct.children[1].asDouble(),
          minTemperature: weatherStruct.children[2].asDouble(),
          uvIndexMax: weatherStruct.children[3].asDouble(),
          windSpeedMax: weatherStruct.children[4].asDouble(),
          weatherCode: weatherStruct.children[5].asInt32(),
          weatherDescription: weatherStruct.children[6].asString(),
        );
      }).toList();
    } catch (e) {
      throw Exception('Failed to fetch daily weather data from D-Bus: $e');
    }
  }

  Future<DailyWeatherData> getDailyWeatherByDate(DateTime date) async {
    await init();

    try {
      final result = await _weatherObject.callMethod(
        'org.waveOS.Weather',
        'GetDailyWeatherByDateAsync',
        [DBusString(date.toIso8601String())],
      );

      final weatherStruct = result.returnValues[0] as DBusStruct;

      return DailyWeatherData(
        date: weatherStruct.children[0].asString(),
        maxTemperature: weatherStruct.children[1].asDouble(),
        minTemperature: weatherStruct.children[2].asDouble(),
        uvIndexMax: weatherStruct.children[3].asDouble(),
        windSpeedMax: weatherStruct.children[4].asDouble(),
        weatherCode: weatherStruct.children[5].asInt32(),
        weatherDescription: weatherStruct.children[6].asString(),
      );
    } catch (e) {
      throw Exception(
        'Failed to fetch daily weather data by date from D-Bus: $e',
      );
    }
  }

  Future<LocationData> getLocation() async {
    await init();

    try {
      final result = await _weatherObject.callMethod(
        'org.waveOS.Weather',
        'GetLocationAsync',
        [],
      );

      final locationStruct = result.returnValues[0] as DBusStruct;

      return LocationData(
        latitude: locationStruct.children[0].asDouble(),
        longitude: locationStruct.children[1].asDouble(),
        continent: locationStruct.children[2].asString(),
        continentCode: locationStruct.children[3].asString(),
        country: locationStruct.children[4].asString(),
        countryCode: locationStruct.children[5].asString(),
        region: locationStruct.children[6].asString(),
        regionName: locationStruct.children[7].asString(),
        city: locationStruct.children[8].asString(),
        zip: locationStruct.children[9].asString(),
        timezone: locationStruct.children[10].asString(),
      );
    } catch (e) {
      throw Exception('Failed to fetch location data from D-Bus: $e');
    }
  }

  void dispose() {
    if (_initialized) {
      _client.close();
      _initialized = false;
    }
  }
}

// Global instance for easy access
final WeatherDBusClient weatherClient = WeatherDBusClient();

Future<CurrentWeatherData> getCurrentWeather() async {
  return await weatherClient.getCurrentWeather();
}

Future<List<HourlyWeatherData>> getHourlyWeather() async {
  return await weatherClient.getHourlyWeather();
}

Future<List<DailyWeatherData>> getDailyWeather() async {
  return await weatherClient.getDailyWeather();
}

Future<DailyWeatherData> getDailyWeatherByDate(DateTime date) async {
  return await weatherClient.getDailyWeatherByDate(date);
}

Future<LocationData> getLocation() async {
  return await weatherClient.getLocation();
}
