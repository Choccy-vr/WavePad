import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import '../Widgets/Video_Loop.dart';
import 'Direction_Down.dart';

class Direction_Page extends StatefulWidget {
  const Direction_Page({super.key});

  @override
  State<Direction_Page> createState() => _Direction_PageState();
}

class _Direction_PageState extends State<Direction_Page> {
  final FocusNode _focusNode = FocusNode();

  @override
  void dispose() {
    _focusNode.dispose();
    super.dispose();
  }

  void _handleKey(KeyEvent event, var context) {
    if (event is KeyDownEvent &&
        event.logicalKey == LogicalKeyboardKey.arrowUp) {
      Navigator.push(
        context,
        SharedAxisPageRoute(
          child: Direction__Down_Page(),
          transitionType: SharedAxisTransitionType.vertical,
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Focus(
        autofocus: true,
        focusNode: _focusNode,
        onKeyEvent: (FocusNode node, KeyEvent event) {
          _handleKey(event, context);
          return KeyEventResult.ignored;
        },
        child: Center(
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
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onSurface,
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
                        'WaveOS can be navigated by simply waving your hand.',
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
                                  assetPath: 'assets/Up.MOV',
                                ),
                              ),
                            ),
                          ),
                          Hero(
                            tag: 'instruction',
                            child: Text(
                              'Wave Up',
                              style: Theme.of(context).textTheme.displaySmall
                                  ?.copyWith(
                                    color: Theme.of(
                                      context,
                                    ).colorScheme.primary,
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
      ),
    );
  }
}
