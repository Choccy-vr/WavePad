import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import '../Widgets/Video_Loop.dart';
import 'Home.dart';

class Select_Page extends StatefulWidget {
  const Select_Page({Key? key}) : super(key: key);

  @override
  State<Select_Page> createState() => _Select_PageState();
}

class _Select_PageState extends State<Select_Page> {
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
                padding: const EdgeInsets.only(
                  left: 16.0,
                  right: 16.0,
                  top: 16.0,
                ),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      'WaveOS can be controlled by making hand gestures.',
                      style: Theme.of(context).textTheme.headlineLarge
                          ?.copyWith(
                            color: Theme.of(context).colorScheme.onSurface,
                          ),
                      textAlign: TextAlign.center,
                    ),
                    SizedBox(height: 16),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        Hero(
                          tag: 'video',
                          child: SizedBox(
                            height: 140,
                            child: AspectRatio(
                              aspectRatio:
                                  16 /
                                  9, // or any ratio you want, or use the controller's aspectRatio
                              child: LoopingVideoWidget(
                                assetPath: 'assets/Select.MOV',
                              ),
                            ),
                          ),
                        ),
                        Hero(
                          tag: 'instruction',
                          child: ElevatedButton(
                            onPressed: () {
                              Navigator.push(
                                context,
                                SharedAxisPageRoute(
                                  child: Home_Page(),
                                  transitionType:
                                      SharedAxisTransitionType.horizontal,
                                ),
                              );
                            },
                            child: Text(
                              'Make a OK gesture',
                              style: Theme.of(context).textTheme.headlineMedium
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.primary,
                                  ),
                            ),
                          ),
                        ),
                      ],
                    ),
                    SizedBox(height: 16),
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
