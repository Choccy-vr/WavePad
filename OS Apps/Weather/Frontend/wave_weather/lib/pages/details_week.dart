import 'package:flutter/material.dart';
import 'package:material_symbols_icons/material_symbols_icons.dart';
import 'package:wave_weather/Weather.dart';
import 'package:wave_weather/widgets/Weather-Details.dart';
import 'package:wave_weather/pages/details_tom.dart';
import 'package:wave_weather/WeatherData.dart';
import 'details_today.dart';
import 'details_tom.dart';

class DetailsWeekPage extends StatefulWidget {
  const DetailsWeekPage({super.key});

  @override
  State<DetailsWeekPage> createState() => _DetailsWeekPageState();
}

class _DetailsWeekPageState extends State<DetailsWeekPage> {
  int selectedIndex = 2;
  String Tempature = (WeatherData.currentWeather != null)
      ? '${WeatherData.currentWeather!.tempature}°'
      : '??°';
  String Location = (WeatherData.locationData != null)
      ? '${WeatherData.locationData!.City}, ${WeatherData.locationData!.Region}'
      : 'Unknown Location';
  String Current_Condition = (WeatherData.currentWeather != null)
      ? WeatherData.currentWeather!.weatherDescription
      : 'Unknown Condition';
  WeatherIcon Current_ConditionIcon = WeatherData.GetWeatherIcon(
    WeatherData.currentWeather?.weatherCode ?? 100,
    isDay: WeatherData.currentWeather?.isDay ?? true,
  );
  List<DailyWeatherData> Days = WeatherData.dailyWeather;
  List<String> Date = [];
  List<String> Forecasted_Condtion = [];
  List<WeatherIcon> Forecasted_ConditionIcon = [];
  List<String> Forecasted_Max_Temperature = [];
  List<String> Forecasted_Min_Temperature = [];
  List<String> Forecasted_Max_UVIndex = [];
  List<String> Forecasted_Max_WindSpeed = [];

  void NavigateToPage(int index) {
    switch (index) {
      case 0:
        Navigator.pop(
          context,
          MaterialPageRoute(
            builder: (context) {
              return DetailsPage();
            },
          ),
        );
        break;
      case 1:
        Navigator.pop(
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
              return DetailsWeekPage();
            },
          ),
        );
    }
  }

  @override
  void initState() {
    super.initState();
    // Initialize the weather data
    if (Days.isNotEmpty) {
      for (var day in Days) {
        Date.add(day.date);
        Forecasted_Condtion.add(day.weatherDescription);
        Forecasted_ConditionIcon.add(
          WeatherData.GetWeatherIcon(day.weatherCode),
        );
        Forecasted_Max_Temperature.add('${day.maxTemperature}°');
        Forecasted_Min_Temperature.add('${day.minTemperature}°');
        Forecasted_Max_UVIndex.add('${day.uvIndexMax}');
        Forecasted_Max_WindSpeed.add('${day.windSpeedMax} mph');
      }
    }
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
                          Tempature,
                          style: Theme.of(context).textTheme.headlineMedium
                              ?.copyWith(
                                color: Theme.of(context).colorScheme.primary,
                              ),
                        ),
                        SizedBox(width: 30),
                        Row(
                          children: [
                            Icon(
                              Current_ConditionIcon.icon,
                              color: Current_ConditionIcon.color,
                              size: 42,
                            ),
                            SizedBox(width: 10),
                            Text(
                              Current_Condition,
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
            Padding(
              padding: const EdgeInsets.only(
                top: 0,
                left: 25,
                right: 25,
                bottom: 25,
              ),
              child: ListView(
                shrinkWrap: true,
                physics: NeverScrollableScrollPhysics(),
                children: [
                  for (int i = 0; i < Days.length; i++)
                    Card(
                      child: Padding(
                        padding: const EdgeInsets.all(10.0),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceAround,
                          children: [
                            Text(
                              Date[i],
                              style: Theme.of(context).textTheme.titleMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.onSurfaceVariant,
                                  ),
                            ),
                            Icon(
                              Forecasted_ConditionIcon[i].icon,
                              color: Forecasted_ConditionIcon[i].color,
                              size: 30,
                            ),
                            Text(
                              Forecasted_Condtion[i],
                              style: Theme.of(context).textTheme.titleMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.onSurfaceVariant,
                                  ),
                            ),
                            Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Text(
                                  'Max:',
                                  style: Theme.of(context).textTheme.titleMedium
                                      ?.copyWith(
                                        color: Theme.of(
                                          context,
                                        ).colorScheme.onSurfaceVariant,
                                      ),
                                ),
                                Text(
                                  Forecasted_Max_Temperature[i],
                                  style: Theme.of(context).textTheme.titleMedium
                                      ?.copyWith(
                                        color: Theme.of(
                                          context,
                                        ).colorScheme.primaryFixed,
                                      ),
                                ),
                              ],
                            ),
                            Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Text(
                                  'Low:',
                                  style: Theme.of(context).textTheme.titleMedium
                                      ?.copyWith(
                                        color: Theme.of(
                                          context,
                                        ).colorScheme.onSurfaceVariant,
                                      ),
                                ),
                                Text(
                                  Forecasted_Min_Temperature[i],
                                  style: Theme.of(context).textTheme.titleMedium
                                      ?.copyWith(
                                        color: Theme.of(
                                          context,
                                        ).colorScheme.primaryFixed,
                                      ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
