import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';

class Direction_Page extends StatelessWidget {
  const Direction_Page({Key? key}) : super(key: key);

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
                          Symbols.help_rounded,
                          size: 68,
                          color: Theme.of(context).colorScheme.primary,
                        ),
                      ),
                      Hero(
                        tag: 'text',
                        child: Text(
                          'How to navigate WaveOS',
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
                    Text(
                      'WaveOS can be navigated by simply waving your hand.',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                      textAlign: TextAlign.center,
                    ),
                    SizedBox(height: 16),
                    Text(
                      'Add Video Here',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                      textAlign: TextAlign.center,
                    ),
                    SizedBox(height: 56),
                    //TODO: Wheel Effect
                    Text(
                      'Wave Up',
                      style: Theme.of(context).textTheme.headlineSmall
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
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
