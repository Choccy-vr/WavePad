import 'dart:io';
import 'dart:convert';

Future<void> startApp(String appPath) async {
  try {
    final process = await Process.start(appPath, []);

    // Listen to output
    process.stdout.transform(utf8.decoder).listen((data) {
      print('App output: $data');
    });

    process.stderr.transform(utf8.decoder).listen((data) {
      print('App error: $data');
    });

    // Wait for process to finish
    final exitCode = await process.exitCode;
    print('App exited with code: $exitCode');
  } catch (e) {
    print('Failed to start app: $e');
  }
}
