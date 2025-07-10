import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:wave_env/OpenApp.dart';
import 'package:window_manager/window_manager.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:material_symbols_icons/symbols.dart';
// Import diffrent scripts
import 'widgets/system_toolbar.dart';
import 'apps.dart'; // Import apps page
import 'widgets/clock_widget.dart';
import 'Weather.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  // Must add this line.
  await windowManager.ensureInitialized();

  WindowOptions windowOptions = WindowOptions(
    size: Size(800, 480),
    center: true,
    //fullScreen: true, //enable this on build
    skipTaskbar: false,
    titleBarStyle: TitleBarStyle.hidden,
  );
  windowManager.waitUntilReadyToShow(windowOptions, () async {
    await windowManager.show();
    await windowManager.focus();
  });

  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'WaveOS Demo',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color.fromARGB(255, 80, 200, 120),
          brightness: Brightness.dark,
        ),
        textTheme: GoogleFonts.interTextTheme(Theme.of(context).textTheme),
      ),
      home: const Home_Page(),
    );
  }
}

class Home_Page extends StatefulWidget {
  const Home_Page({super.key});

  @override
  State<Home_Page> createState() => _HomePageState();
}

class _HomePageState extends State<Home_Page> {
  final FocusNode _focusNode = FocusNode();
  late CurrentWeatherData WeatherData;
  late DailyWeatherData DailyWeather;
  late LocationData Location;

  void _handleKey(KeyEvent event, var context) {
    if (event is KeyDownEvent &&
        event.logicalKey == LogicalKeyboardKey.arrowLeft) {
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) {
            return AppsPage();
          },
        ),
      );
    }
  }

  void _navigateToApps() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => AppsPage()),
    );
  }

  @override
  void initState() {
    super.initState();
    GetCurrentWeather().then((weather) {
      setState(() {
        WeatherData = weather;
      });
    });
    GetDayWeather().then((dailyWeather) {
      setState(() {
        DailyWeather = dailyWeather;
      });
    });
    GetLocationData().then((location) {
      setState(() {
        Location = location;
      });
    });
  }

  @override
  Widget build(BuildContext context) {
    return Focus(
      autofocus: true,
      focusNode: _focusNode,
      onKeyEvent: (FocusNode node, KeyEvent event) {
        _handleKey(event, context);
        return KeyEventResult.ignored;
      },
      child: Scaffold(
        body: Stack(
          children: [
            // Toolbar
            Align(
              alignment: Alignment.topRight,
              child: Padding(
                padding: const EdgeInsets.all(5.0),
                child: SystemToolbar(),
              ),
            ),
            //Weather & Date
            Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                // Weather and Date Section
                Padding(
                  padding: const EdgeInsets.only(
                    top: 70.0,
                    left: 70.0,
                    right: 70.0,
                    bottom: 50.0,
                  ),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Container(
                        height: 85,
                        decoration: BoxDecoration(
                          color: Theme.of(context).colorScheme.surfaceContainer,
                          borderRadius: BorderRadius.circular(16),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black26,
                              blurRadius: 4,
                              offset: Offset(0, 2),
                            ),
                          ],
                        ),
                        child: Material(
                          color: Colors.transparent,
                          child: InkWell(
                            borderRadius: BorderRadius.circular(16),
                            onTap: () {
                              startApp('~/Applications/wave_weather');
                            },
                            child: Padding(
                              padding: const EdgeInsets.all(10.0),
                              child: Row(
                                children: [
                                  Padding(
                                    padding: const EdgeInsets.all(10.0),
                                    child: Row(
                                      children: [
                                        //Replace Text and Icon with real weather data
                                        Icon(
                                          Symbols.partly_cloudy_day_rounded,
                                          size: 48,
                                          color: Theme.of(
                                            context,
                                          ).colorScheme.primary,
                                        ),
                                        const SizedBox(width: 10),
                                        Text(
                                          '${WeatherData.tempature}°',
                                          style: Theme.of(context)
                                              .textTheme
                                              .displaySmall
                                              ?.copyWith(
                                                color: Theme.of(
                                                  context,
                                                ).colorScheme.primary,
                                              ),
                                        ),
                                        const SizedBox(width: 10),
                                        Column(
                                          children: [
                                            Text(
                                              Location.City,
                                              style: Theme.of(context)
                                                  .textTheme
                                                  .titleMedium
                                                  ?.copyWith(
                                                    color: Theme.of(context)
                                                        .colorScheme
                                                        .onSurfaceVariant,
                                                  ),
                                            ),
                                            Text(
                                              'H:${DailyWeather.maxTemperature}° L:${DailyWeather.minTemperature}°',
                                              style: Theme.of(context)
                                                  .textTheme
                                                  .bodyMedium
                                                  ?.copyWith(
                                                    color: Theme.of(context)
                                                        .colorScheme
                                                        .onSurfaceVariant,
                                                  ),
                                            ),
                                          ],
                                        ),
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                      ),
                      const ClockWidget(),
                    ],
                  ),
                ),
                //Smart Stack
                //Not yet implemented
                /*Expanded(
                  child: Center(
                    child: Container(
                      width: 300, // adjust as needed
                      decoration: BoxDecoration(
                        color: Theme.of(context).colorScheme.surfaceContainer,
                        borderRadius: BorderRadius.circular(55),
                        border: Border.all(
                          color: Theme.of(context).colorScheme.outline,
                          width: 1.5,
                        ),
                      ),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          Row(
                            children: [
                              Icon(
                                Symbols.alarm_on_rounded,
                                size: 45,
                                color: Theme.of(
                                  context,
                                ).colorScheme.onSurfaceVariant,
                              ),
                              SizedBox(width: 24),
                              Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: [
                                  Text(
                                    'Next Alarm', // Replace with actual smart stack suggestion
                                    style: Theme.of(context)
                                        .textTheme
                                        .headlineSmall!
                                        .copyWith(
                                          color: Theme.of(
                                            context,
                                          ).colorScheme.onSurfaceVariant,
                                        ),
                                  ),
                                  Text(
                                    '7:00 AM', // Replace with actual time
                                    style: Theme.of(context)
                                        .textTheme
                                        .titleLarge!
                                        .copyWith(
                                          color: Theme.of(
                                            context,
                                          ).colorScheme.primary,
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
                ),*/
                // Shortcuts
                Padding(
                  padding: const EdgeInsets.all(25.0),
                  child: Align(
                    alignment: Alignment.bottomLeft,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      crossAxisAlignment: CrossAxisAlignment.end,
                      children: [
                        SizedBox(
                          width: 80,
                          height: 80,
                          child: FloatingActionButton(
                            heroTag: 'Shortcut1',
                            onPressed: _navigateToApps,
                            tooltip: 'Placeholder',
                            child: const Icon(Icons.add, size: 28),
                          ),
                        ),
                        SizedBox(
                          width: 80,
                          height: 80,
                          child: FloatingActionButton(
                            heroTag: 'Shortcut2',
                            onPressed: _navigateToApps,
                            tooltip: 'Placeholder',
                            child: const Icon(Icons.add, size: 28),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
