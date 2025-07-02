import 'package:flutter/material.dart';
import 'package:material_symbols_icons/material_symbols_icons.dart';
import 'package:wave_weather/Weather-Details.dart';
import 'details_today.dart';
import 'details_week.dart';

class DetailsTomPage extends StatefulWidget {
  const DetailsTomPage({super.key});

  @override
  State<DetailsTomPage> createState() => _DetailsTomPageState();
}

class _DetailsTomPageState extends State<DetailsTomPage> {
  int selectedIndex = 1;
  String Tempature = '??°';
  String Location = 'Unkown Location';
  String Current_Condition = 'Condition';
  IconData Current_ConditionIcon = Symbols.partly_cloudy_day_rounded;
  String Forecasted_Condtion = 'Unkown Condition';
  IconData Forecasted_ConditionIcon = Symbols.partly_cloudy_day_rounded;
  String Forecasted_Precipitation_Prob = '??%';
  String Forecasted_MaxTemp = '??°';
  String Forecasted_LowTemp = '??°';
  String Forecasted_Humidity = '??%';
  String Forecasted_UVIndex = '??';
  String Forecasted_WindSpeed = '?? Mph';
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
              return DetailsTomPage();
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
                            Icon(
                              Current_ConditionIcon,
                              color: Colors.yellow,
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
            Card(
              child: Padding(
                padding: const EdgeInsets.symmetric(
                  vertical: 10,
                  horizontal: 25,
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Icon(
                      Forecasted_ConditionIcon,
                      color: Colors.yellow,
                      size: 100,
                    ),
                    Text(
                      Forecasted_Condtion,
                      style: Theme.of(context).textTheme.headlineMedium
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
                          'Max: ',
                          style: Theme.of(context).textTheme.headlineMedium
                              ?.copyWith(
                                color: Theme.of(context).colorScheme.primary,
                              ),
                        ),
                        Text(
                          Forecasted_MaxTemp,
                          style: Theme.of(context).textTheme.headlineMedium
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
                          'Low: ',
                          style: Theme.of(context).textTheme.headlineMedium
                              ?.copyWith(
                                color: Theme.of(context).colorScheme.primary,
                              ),
                        ),
                        Text(
                          Forecasted_LowTemp,
                          style: Theme.of(context).textTheme.headlineMedium
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
                        'Chance of Rain',
                        'Humidity',
                        'Max UV Index',
                        'Max Wind Speed',
                      ];
                      return WeatherCard(
                        title: titles[index],
                        icon: [
                          Symbols.umbrella_rounded,
                          Symbols.water_drop_rounded,
                          Symbols.wb_sunny_rounded,
                          Symbols.air_rounded,
                        ][index],
                        value: [
                          Forecasted_Precipitation_Prob,
                          Forecasted_Humidity,
                          Forecasted_UVIndex,
                          Forecasted_WindSpeed,
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
