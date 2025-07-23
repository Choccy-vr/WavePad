import 'package:flutter/material.dart';
import 'package:material_symbols_icons/material_symbols_icons.dart';
import 'package:wave_weather/Weather.dart';
import 'package:wave_weather/widgets/Weather-Details.dart';
import 'details_tom.dart';
import 'details_week.dart';
import 'package:wave_weather/WeatherData.dart';

class DetailsPage extends StatefulWidget {
  const DetailsPage({super.key});

  @override
  State<DetailsPage> createState() => _DetailsPageState();
}

class _DetailsPageState extends State<DetailsPage> {
  int selectedIndex = 0;
  String Temperature = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.temperature}°'
      : '??°';
  String Humidity = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.humidity}%'
      : '??%';
  String Precipitation = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.precipitation} Inch'
      : '? Inch';
  String Location = (WeatherData.locationData != null)
      ? '${WeatherData.locationData!.city}, ${WeatherData.locationData!.region}'
      : 'Unknown Location';
  String Condition = (WeatherData.currentWeather != null)
      ? WeatherData.currentWeather!.weatherDescription
      : 'Unknown Condition';
  WeatherIcon ConditionIcon = WeatherData.GetWeatherIcon(
    WeatherData.currentWeather?.weatherCode ?? 100,
    isDay: WeatherData.currentWeather?.isDay ?? true,
  );
  List<HourlyWeatherData> HourlyTime = WeatherData.hourlyWeather;
  late List<WeatherIcon> HourlyConditionIcons;
  String UVIndex = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.uvIndex}'
      : '??';
  String WindSpeed = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.windSpeed} Mph'
      : '?? Mph';
  void NavigateToPage(int index) {
    switch (index) {
      case 0:
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) {
              return DetailsPage();
            },
          ),
        );
        break;
      case 1:
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) {
              return DetailsTomPage();
            },
          ),
        );
        break;
      case 2:
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) {
              return DetailsWeekPage();
            },
          ),
        );
        break;
      default:
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) {
              return DetailsPage();
            },
          ),
        );
    }
  }

  @override
  void initState() {
    super.initState();
    HourlyConditionIcons = [
      for (var data in HourlyTime)
        WeatherData.GetWeatherIcon(data.weatherCode, isDay: data.isDay),
    ];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            //top bar
            Container(
              decoration: BoxDecoration(
                color: Theme.of(context).colorScheme.surfaceContainer,
                borderRadius: BorderRadius.only(
                  bottomLeft: Radius.circular(16),
                  bottomRight: Radius.circular(16),
                ),
              ),
              child: Padding(
                padding: EdgeInsets.only(
                  top: 10,
                  left: 25,
                  right: 25,
                  bottom: 10,
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(
                      Location,
                      style: Theme.of(context).textTheme.headlineMedium
                          ?.copyWith(
                            color: Theme.of(
                              context,
                            ).colorScheme.onSurfaceVariant,
                          ),
                    ),
                    Center(
                      child: Icon(
                        Symbols.keyboard_arrow_up_rounded,
                        color: Theme.of(context).colorScheme.primary,
                        size: 48,
                      ),
                    ),
                    Row(
                      children: [
                        Text(
                          Temperature,
                          style: Theme.of(context).textTheme.headlineMedium
                              ?.copyWith(
                                color: Theme.of(context).colorScheme.primary,
                              ),
                        ),
                        SizedBox(width: 30),
                        Row(
                          children: [
                            Icon(
                              ConditionIcon.icon,
                              color: ConditionIcon.color,
                              size: 42,
                            ),
                            SizedBox(width: 10),
                            Text(
                              Condition,
                              style: Theme.of(context).textTheme.headlineMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.onSurfaceVariant,
                                  ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
            Center(
              child: Padding(
                padding: const EdgeInsets.all(25.0),
                child: Wrap(
                  spacing: 12,
                  children: [
                    ChoiceChip(
                      label: Text('Today'),
                      selected: selectedIndex == 0,
                      onSelected: (bool selected) {
                        setState(() {
                          selectedIndex = selected ? 0 : selectedIndex;
                        });
                        if (selected) {
                          NavigateToPage(0);
                        }
                      },
                    ),
                    ChoiceChip(
                      label: Text('Tomorrow'),
                      selected: selectedIndex == 1,
                      onSelected: (bool selected) {
                        setState(() {
                          selectedIndex = selected ? 1 : selectedIndex;
                        });
                        if (selected) {
                          NavigateToPage(1);
                        }
                      },
                    ),
                    ChoiceChip(
                      label: Text('7 Day'),
                      selected: selectedIndex == 2,
                      onSelected: (bool selected) {
                        setState(() {
                          selectedIndex = selected ? 2 : selectedIndex;
                        });
                        if (selected) {
                          NavigateToPage(2);
                        }
                      },
                    ),
                  ],
                ),
              ),
            ),
            //Hourly
            Divider(
              color: Theme.of(context).colorScheme.outline,
              thickness: .5,
            ),
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 0, horizontal: 25),
              child: Card(
                child: Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      for (int i = 0; i < HourlyTime.length; i++)
                        Column(
                          children: [
                            Icon(
                              HourlyConditionIcons[i].icon,
                              color: HourlyConditionIcons[i].color,
                              size: 48,
                            ),
                            Text(
                              HourlyTime[i].time,
                              style: Theme.of(context).textTheme.titleMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.onSurfaceVariant,
                                  ),
                            ),
                          ],
                        ),
                    ],
                  ),
                ),
              ),
            ),
            Divider(
              color: Theme.of(context).colorScheme.outline,
              thickness: .5,
            ),
            //Details
            Expanded(
              child: LayoutBuilder(
                builder: (context, constraints) {
                  double spacing = 10;
                  double padding = 16 * 2;

                  // Calculate item height
                  double itemHeight =
                      (constraints.maxHeight - spacing - padding) / 2;

                  return GridView.builder(
                    padding: EdgeInsets.all(16),
                    itemCount: 4,
                    gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                      crossAxisCount: 2,
                      crossAxisSpacing: 25,
                      mainAxisSpacing: 25,
                      childAspectRatio: constraints.maxWidth / 2 / itemHeight,
                    ),
                    itemBuilder: (context, index) {
                      final titles = [
                        'Humidity',
                        'Precipitation',
                        'UV Index',
                        'Wind Speed',
                      ];
                      return WeatherCard(
                        title: titles[index],
                        icon: [
                          Symbols.water_drop_rounded,
                          Symbols.umbrella_rounded,
                          Symbols.wb_sunny_rounded,
                          Symbols.air_rounded,
                        ][index],
                        value: [
                          Humidity,
                          Precipitation,
                          UVIndex,
                          WindSpeed,
                        ][index],
                      );
                    },
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
