import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'help/Touch.dart';

class Update_Page extends StatelessWidget {
  const Update_Page({Key? key}) : super(key: key);

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
                          Symbols.browser_updated_rounded,
                          size: 68,
                          color: Theme.of(context).colorScheme.primary,
                        ),
                      ),
                      Hero(
                        tag: 'text',
                        child: Text(
                          'Update',
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
                  mainAxisAlignment: MainAxisAlignment.spaceAround,
                  children: [
                    Text(
                      'Software updates are not yet available',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                      textAlign: TextAlign.center,
                    ),
                    SizedBox(height: 32),
                    Hero(
                      tag: 'next_button',
                      child: ElevatedButton(
                        onPressed: () {
                          Navigator.push(
                            context,
                            SharedAxisPageRoute(
                              child: Touch_Page(),
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
