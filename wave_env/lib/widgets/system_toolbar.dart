import 'package:flutter/material.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'dart:async';
import 'package:quick_usb/quick_usb.dart';
import 'dart:io';

class SystemToolbar extends StatefulWidget {
  const SystemToolbar({Key? key}) : super(key: key);

  @override
  State<SystemToolbar> createState() => _SystemToolbarState();
}

class _SystemToolbarState extends State<SystemToolbar> {
  bool WiFi = false;
  bool USB = false;
  Timer? _timer;

  var _devices = [];

  Future<void> _refreshUsbList() async {
    final list = await QuickUsb.getDeviceList();
    setState(() => _devices = list);
    await QuickUsb.exit();
  }

  @override
  void initState() {
    super.initState();
    WidgetsFlutterBinding.ensureInitialized();
    QuickUsb.init();
    _refreshUsbList();
    _startStatusUpdates();
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  void _startStatusUpdates() {
    _timer = Timer.periodic(const Duration(seconds: 2), (timer) async {
      // Replace these with your actual status check functions
      bool wifiStatus = await checkWiFiStatus();
      bool usbStatus = await checkUSBStatus();

      setState(() {
        WiFi = wifiStatus;
        USB = usbStatus;
      });
    });
  }

  Future<bool> checkWiFiStatus() async {
    try {
      if (Platform.isLinux) {
        final result = await Process.run('nmcli', ['-t', '-f', 'WIFI', 'g']);
        if (result.exitCode == 0 &&
            result.stdout.toString().trim() == 'enabled') {
          return true;
        }
      }
      // Fallback: try to ping Google DNS
      final result = await InternetAddress.lookup('8.8.8.8');
      return result.isNotEmpty && result[0].rawAddress.isNotEmpty;
    } catch (_) {
      return false;
    }
  }

  Future<bool> checkUSBStatus() async {
    _refreshUsbList();
    return _devices.isNotEmpty;
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 125,
      height: 55,
      decoration: BoxDecoration(
        color: Theme.of(context).colorScheme.surfaceContainer,
        borderRadius: BorderRadius.circular(27.5),
      ),
      child: Row(
        children: [
          SizedBox(
            width: 60,
            height: 60,
            child: Center(
              child: Icon(
                USB ? Symbols.usb_rounded : Symbols.usb_off_rounded,
                color: Theme.of(context).colorScheme.error,
                size: 30,
              ),
            ),
          ),
          SizedBox(
            width: 60,
            height: 60,
            child: Center(
              child: Icon(
                WiFi ? Symbols.wifi_rounded : Symbols.wifi_off_rounded,
                color: Theme.of(context).colorScheme.onSurfaceVariant,
                size: 30,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
