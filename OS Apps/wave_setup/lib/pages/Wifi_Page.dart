import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'USB_Page.dart';

class Wifi_Page extends StatelessWidget {
  const Wifi_Page({Key? key}) : super(key: key);

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
                    Card(
                      color: Theme.of(context).colorScheme.secondaryContainer,
                      child: ListTile(
                        leading: Icon(
                          Symbols.network_wifi_rounded,
                          size: 32,
                          color: Theme.of(context).colorScheme.primary,
                        ),
                        title: Text('Connection'),
                        subtitle: Text('Connecting Details'),
                        trailing: Icon(
                          Symbols.check_rounded,
                          size: 32,
                          color: Theme.of(context).colorScheme.primary,
                        ),
                        onTap: () {
                          Navigator.push(
                            context,
                            SharedAxisPageRoute(
                              child: USB_Page(),
                              transitionType:
                                  SharedAxisTransitionType.horizontal,
                            ),
                          );
                        },
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
