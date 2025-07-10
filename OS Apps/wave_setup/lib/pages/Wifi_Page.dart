import 'package:dbus_wifi/models/wifi_network.dart';
import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'USB_Page.dart';
import 'package:dbus_wifi/dbus_wifi.dart';
import 'Keyboard.dart';

//Uncomment to actual use Wi-Fi functionality
class Wifi_Page extends StatefulWidget {
  const Wifi_Page({Key? key}) : super(key: key);

  @override
  State<Wifi_Page> createState() => _Wifi_PageState();
}

class _Wifi_PageState extends State<Wifi_Page> {
  final wifi = DbusWifi();
  List<WifiNetwork> networks = [];
  WifiNetwork? connectedNetwork;
  Future<List<WifiNetwork>> NetworksNearby() async {
    // Check if Wi-Fi device is available
    if (await wifi.hasWifiDevice) {
      // Search for Wi-Fi networks
      final results = await wifi.search(timeout: Duration(seconds: 7));
      print('Found ${results.length} networks');
      return results;
    }
    return [];
  }

  Future<Object> ConnectToNetwork(WifiNetwork network, String password) async {
    // Connect to a network
    try {
      await wifi.connect(network, password);
      await wifi.close();
      connectedNetwork = network;
      return ('Connected to ${network.ssid}');
    } catch (e) {
      await wifi.close();
      return ('Failed to connect: $e');
    }
  }

  @override
  void initState() {
    super.initState();
    // Initialize Wi-Fi device
    _loadNetworks();
  }

  void _loadNetworks() async {
    var discoveredNetworks = await NetworksNearby();
    setState(() {
      networks = discoveredNetworks;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(25.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            children: [
              Column(
                children: [
                  Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Hero(
                        tag: 'icon',
                        child: Icon(
                          Symbols.wifi_rounded,
                          size: 68,
                          color: Theme.of(context).colorScheme.primary,
                        ),
                      ),
                      Hero(
                        tag: 'text',
                        child: Text(
                          'WiFi',
                          style: Theme.of(context).textTheme.displaySmall
                              ?.copyWith(
                                color: Theme.of(context).colorScheme.onSurface,
                              ),
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 25),
                  Hero(
                    tag: 'divider',
                    child: Divider(
                      color: Theme.of(context).colorScheme.outlineVariant,
                      thickness: 2,
                    ),
                  ),
                ],
              ),
              Padding(
                padding: const EdgeInsets.all(16.0),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    if (networks.isEmpty) ...[
                      Center(
                        child: Column(
                          children: [
                            Icon(
                              Symbols.wifi_off_rounded,
                              size: 128,
                              color: Theme.of(context).colorScheme.error,
                            ),
                            Text(
                              'No Wi-Fi networks found',
                              style: Theme.of(context).textTheme.displayMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.onSurface,
                                  ),
                            ),
                          ],
                        ),
                      ),
                    ] else
                      ...networks.map(
                        (network) => Card(
                          color: Theme.of(
                            context,
                          ).colorScheme.secondaryContainer,
                          child: ListTile(
                            leading: Icon(
                              Symbols.network_wifi_rounded,
                              size: 32,
                              color: Theme.of(context).colorScheme.primary,
                            ),
                            title: Text(network.ssid),
                            trailing: Icon(
                              Symbols.check_rounded,
                              size: 32,
                              color: Theme.of(context).colorScheme.primary,
                            ),
                            onTap: () async {
                              final password = await showDialog<String>(
                                context: context,
                                builder: (context) => Dialog(
                                  child: Keyboard(
                                    title: network.ssid,
                                    onDone: (value) {
                                      Navigator.of(context).pop(
                                        value,
                                      ); // Return value and close dialog
                                    },
                                  ),
                                ),
                              );
                              if (password != null) {
                                var result = await ConnectToNetwork(
                                  network,
                                  password,
                                );
                                if (result == 'Connected to ${network.ssid}') {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(
                                      content: Text(result.toString()),
                                      duration: Duration(seconds: 2),
                                    ),
                                  );
                                  Navigator.push(
                                    context,
                                    SharedAxisPageRoute(
                                      child: USB_Page(),
                                      transitionType:
                                          SharedAxisTransitionType.horizontal,
                                    ),
                                  );
                                } else {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(
                                      content: Text(result.toString()),
                                      duration: Duration(seconds: 2),
                                    ),
                                  );
                                }
                              }
                            },
                          ),
                        ),
                      ),
                  ],
                ),
              ),
              Align(
                alignment: Alignment.bottomRight,
                child: OutlinedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      SharedAxisPageRoute(
                        child: USB_Page(),
                        transitionType: SharedAxisTransitionType.horizontal,
                      ),
                    );
                  },
                  child: Text(
                    'Skip',
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: Theme.of(context).colorScheme.error,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
