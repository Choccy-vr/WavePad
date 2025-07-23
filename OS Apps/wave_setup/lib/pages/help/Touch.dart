import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'Direction_Up.dart';

class Touch_Page extends StatelessWidget {
  const Touch_Page({super.key});

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
                      'WaveOS can be navigated using touch.',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                      textAlign: TextAlign.center,
                    ),
                    SizedBox(height: 56),
                    Text(
                      'Press Next',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                    ),
                    SizedBox(height: 56),
                    Hero(
                      tag: 'next_button',
                      child: ElevatedButton(
                        onPressed: () {
                          Navigator.push(
                            context,
                            SharedAxisPageRoute(
                              child: Direction_Page(),
                              transitionType:
                                  SharedAxisTransitionType.horizontal,
                            ),
                          );
                        },
                        child: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Icon(
                              Symbols.keyboard_arrow_right_rounded,
                              size: 24,
                            ),
                            SizedBox(width: 8),
                            Text('Next'),
                          ],
                        ),
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
