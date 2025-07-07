import 'dart:io';
import 'dart:typed_data';
import 'Keys.dart';

class HIDController {
  static const String keyboardDevice = '/dev/hidg0';
  static const String mediaDevice = '/dev/hidg1';

  // Send keyboard report
  static Future<void> sendKeyboardReport(
    int modifier,
    List<int> keycodes,
  ) async {
    final report = Uint8List(8);
    report[0] = modifier;
    report[1] = 0; // Reserved

    for (int i = 0; i < 6 && i < keycodes.length; i++) {
      report[i + 2] = keycodes[i];
    }

    final file = File(keyboardDevice);
    await file.writeAsBytes(report);
  }

  // Send media report
  static Future<void> sendMediaReport(int usageCode) async {
    final report = Uint8List(2);
    report[0] = usageCode & 0xFF; // Low byte
    report[1] = (usageCode >> 8) & 0xFF; // High byte

    final file = File(mediaDevice);
    await file.writeAsBytes(report);
  }

  // Release all keys
  static Future<void> releaseAllKeys() async {
    await sendKeyboardReport(0, []);
  }

  // Release media keys
  static Future<void> releaseMediaKeys() async {
    await sendMediaReport(0);
  }

  // High-level key press function
  static Future<void> sendKeyPress(int modifier, int keycode) async {
    await sendKeyboardReport(modifier, [keycode]);
    await Future.delayed(Duration(milliseconds: 10));
    await releaseAllKeys();
  }

  // High-level media key function
  static Future<void> sendMediaKey(int usageCode) async {
    await sendMediaReport(usageCode);
    await Future.delayed(Duration(milliseconds: 10));
    await releaseMediaKeys();
  }

  // Send text string
  static Future<void> sendText(String text) async {
    for (int i = 0; i < text.length; i++) {
      final char = text[i].toLowerCase();
      int keycode = 0;
      int modifier = 0;

      if (Key_Definition.values.any((k) => k.name == char)) {
        keycode = Key_Definition.values.firstWhere((k) => k.name == char).value;
      } else if (char == ' ') {
        keycode = Key_Definition.space.value;
      }

      if (keycode != 0) {
        await sendKeyPress(modifier, keycode);
        await Future.delayed(Duration(milliseconds: 50));
      }
    }
  }
}
