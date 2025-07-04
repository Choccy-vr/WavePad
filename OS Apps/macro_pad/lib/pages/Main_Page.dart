import 'package:flutter/material.dart';
import 'package:material_symbols_icons/symbols.dart';

class Macro {
  final String name;
  final IconData icon;
  final String key;

  Macro({required this.name, required this.icon, required this.key});
}

class Main_Page extends StatelessWidget {
  const Main_Page({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(20.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.content_copy_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Copy',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.content_cut_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Cut',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.content_paste_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Paste',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.volume_down_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Volume Down',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.play_pause_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Play/Pause',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 135,
                    height: 135,
                    child: FloatingActionButton(
                      onPressed: () {
                        // Add your action here
                      },
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Symbols.volume_up_rounded, size: 50),
                          SizedBox(height: 8), // Add some space
                          Text(
                            'Volume Up',
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(
                                  color: Theme.of(
                                    context,
                                  ).colorScheme.onPrimaryContainer,
                                ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
