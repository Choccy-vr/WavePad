import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'package:window_manager/window_manager.dart';
import 'dart:async';
import 'dart:ui';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await windowManager.ensureInitialized();

  WindowOptions windowOptions = WindowOptions(
    size: const Size(226, 55),
    center: false,
    backgroundColor: Colors.transparent,
    skipTaskbar: true, // Hide from taskbar
    titleBarStyle: TitleBarStyle.hidden, // Remove title bar
    alwaysOnTop: true,
  );

  windowManager.waitUntilReadyToShow(windowOptions, () async {
    await windowManager.setAlignment(Alignment.topRight);
    await windowManager.setHasShadow(false);
    await windowManager.show();
  });
  runApp(const MainApp());
}

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'System Tray',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color.fromARGB(255, 80, 200, 120),
          brightness: Brightness.dark,
        ),
        textTheme: GoogleFonts.interTextTheme(Theme.of(context).textTheme),
      ),
      home: SystemToolbar(),
      debugShowCheckedModeBanner: false,
    );
  }
}

class SystemToolbar extends StatefulWidget {
  const SystemToolbar({super.key});

  @override
  State<SystemToolbar> createState() => _SystemToolbarState();
}

class _SystemToolbarState extends State<SystemToolbar> {
  bool WiFi = false;
  bool USB = true;
  bool Notifications = true;
  // Code for getting all this stuff will be here once I move everything to DBUS
  // TODO: Implement USB, WiFi, and Notification status checks

  @override
  Widget build(BuildContext context) {
    int visibleItems = 2; // USB and WiFi always visible
    if (Notifications) visibleItems++;
    double totalWidth =
        (visibleItems * 60) + ((visibleItems - 1) * 4) + 16; // padding
    return Expanded(
      child: ClipRRect(
        borderRadius: BorderRadius.circular(27.5),
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: 30, sigmaY: 30),
          child: Container(
            width: totalWidth,
            height: 55,
            decoration: BoxDecoration(
              // Gradient using your theme colors with glass effect
              gradient: LinearGradient(
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
                colors: [
                  Color.lerp(
                    Theme.of(context).colorScheme.surfaceContainer,
                    Colors.white,
                    0.15, // Less white tint
                  )!.withOpacity(0.87),
                  Color.lerp(
                    Theme.of(context).colorScheme.surfaceContainer,
                    Colors.white,
                    0.05, // Even less white at bottom
                  )!.withOpacity(0.77),
                ],
              ),
              borderRadius: BorderRadius.circular(27.5),
              border: Border.all(
                color: Color.lerp(
                  Theme.of(context).colorScheme.outline,
                  Colors.white,
                  0.1, // Blend your outline color with white
                )!.withOpacity(0.2),
                width: 1.5,
              ),
              boxShadow: [
                BoxShadow(
                  color: Theme.of(context).colorScheme.primary.withOpacity(
                    0.1,
                  ), // Use your primary color
                  offset: Offset(0, 1),
                  blurRadius: 0,
                  spreadRadius: 0,
                ),
                BoxShadow(
                  color: Colors.black.withOpacity(0.1),
                  offset: Offset(0, 4),
                  blurRadius: 12,
                  spreadRadius: 0,
                ),
              ],
            ),
            child: Container(
              // Inner container for inset effect
              margin: EdgeInsets.all(1),
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(26.5),
                gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [
                    Theme.of(
                      context,
                    ).colorScheme.surfaceContainer.withOpacity(0.1),
                    Theme.of(
                      context,
                    ).colorScheme.surfaceContainer.withOpacity(0.05),
                  ],
                ),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  SizedBox(width: 8), // Padding on the left
                  if (Notifications) ...[
                    SizedBox(
                      width: 60,
                      height: 60,
                      child: Center(
                        child: Icon(
                          Symbols.notifications_unread_rounded,
                          color: Theme.of(context).colorScheme.primaryFixed,
                          size: 40,
                        ),
                      ),
                    ),
                    SizedBox(width: 4),
                  ],
                  SizedBox(
                    width: 60,
                    height: 60,
                    child: Center(
                      child: Icon(
                        USB ? Symbols.usb_rounded : Symbols.usb_off_rounded,
                        color: USB
                            ? Theme.of(context).colorScheme.primaryFixed
                            : Theme.of(context).colorScheme.error,
                        size: 40,
                      ),
                    ),
                  ),
                  SizedBox(width: 4),
                  SizedBox(
                    width: 60,
                    height: 60,
                    child: Center(
                      child: Icon(
                        WiFi ? Symbols.wifi_rounded : Symbols.wifi_off_rounded,
                        color: WiFi
                            ? Theme.of(context).colorScheme.primaryFixed
                            : Theme.of(context).colorScheme.error,
                        size: 40,
                      ),
                    ),
                  ),
                  SizedBox(width: 8), // Padding on the right
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
