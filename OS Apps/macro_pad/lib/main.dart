import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:window_manager/window_manager.dart';
import 'package:quick_usb/quick_usb.dart';
//pages
import 'package:macro_pad/pages/Main_Page.dart';
import 'package:macro_pad/pages/No_USB.dart';

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
  runApp(const MainApp());
}

class MainApp extends StatefulWidget {
  const MainApp({super.key});

  @override
  State<MainApp> createState() => _MainAppState();
}

class _MainAppState extends State<MainApp> {
  var _devices = [];

  @override
  void initState() {
    super.initState();
    WidgetsFlutterBinding.ensureInitialized();
    QuickUsb.init();
    _refreshUsbList();
  }

  Future<void> _refreshUsbList() async {
    final list = await QuickUsb.getDeviceList();
    setState(() => _devices = list);
    await QuickUsb.exit();
  }

  @override
  Widget build(BuildContext context) {
    bool hasUsb = _devices.isNotEmpty;
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'WaveOS Macro Pad',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color.fromARGB(255, 80, 200, 120),
          brightness: Brightness.dark,
        ),
        textTheme: GoogleFonts.interTextTheme(Theme.of(context).textTheme),
      ),
      home: hasUsb ? const Main_Page() : const No_USB(),
    );
  }
}
