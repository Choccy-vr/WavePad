import 'package:dbus/dbus.dart';

class SysStatsClient {
  late DBusClient _client;
  late DBusRemoteObject _sysStats;
  bool _initialized = false;

  Future<void> init() async {
    if (_initialized) return;

    _client = DBusClient.session();
    _sysStats = DBusRemoteObject(
      _client,
      name: 'org.waveOS.SysStats',
      path: DBusObjectPath('/org/waveOS/SysStats'),
    );
    _initialized = true;
  }

  Future<bool> isConnectedWiFi() async {
    if (!_initialized) await init();

    try {
      final result = await _sysStats.callMethod(
        'org.waveOS.SysStats',
        'GetWiFiIsConnectedAsync',
        [],
      );

      return result.returnValues[0].asBoolean();
    } catch (e) {
      print('Error calling GetWiFiIsConnectedAsync: $e');
      rethrow;
    }
  }

  Future<bool> isConnectedUSB() async {
    if (!_initialized) await init();

    try {
      final result = await _sysStats.callMethod(
        'org.waveOS.SysStats',
        'GetUSBConnectedDeviceCountAsync',
        [],
      );

      return result.returnValues[0].asInt32() > 0;
    } catch (e) {
      print('Error calling GetUSBConnectedDeviceCountAsync: $e');
      rethrow;
    }
  }

  void dispose() {
    if (_initialized) {
      _client.close();
      _initialized = false;
    }
  }
}
