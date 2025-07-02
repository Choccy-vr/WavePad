import 'package:flutter/material.dart';
import 'package:material_symbols_icons/material_symbols_icons.dart';
import 'package:wave_weather/Weather-Details.dart';
import 'details_tom.dart';
import 'details_week.dart';

class DetailsPage extends StatefulWidget {
  const DetailsPage({super.key});

  @override
  State<DetailsPage> createState() => _DetailsPageState();
}

class _DetailsPageState extends State<DetailsPage> {
  int selectedIndex = 0;
  String Tempature = '??°';
  String Humidity = '??%';
  String Precipitation = '? Inch';
  String Location = 'Unkown Location';
  String Condition = 'Condition';
  IconData ConditionIcon = Symbols.partly_cloudy_day_rounded;
  List<String> HorlyTime = ['??:?? AM', '??:?? PM', '??:?? AM', '??:?? PM'];
  List<IconData> HorlyConditionIcons = [
    Symbols.partly_cloudy_day_rounded,
    Symbols.wb_sunny_rounded,
    Symbols.cloud_rounded,
    Symbols.rainy_rounded,
  ];
  String UVIndex = '??';
  String WindSpeed = '?? Mph';
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
                            Icon(ConditionIcon, color: Colors.yellow, size: 42),
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
            Card(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    for (int i = 0; i < HorlyTime.length; i++)
                      Column(
                        children: [
                          Icon(
                            HorlyConditionIcons[i],
                            color: Theme.of(context).colorScheme.primary,
                            size: 48,
                          ),
                          Text(
                            HorlyTime[i],
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
