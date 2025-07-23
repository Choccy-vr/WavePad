import 'package:flutter/material.dart';
import '/animations/Shared_Axis.dart';
import 'package:animations/animations.dart';
import 'package:material_symbols_icons/symbols.dart';
import 'Language_Page.dart';

class StartPage extends StatelessWidget {
  const StartPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            Text(
              'Welcome to WavePad',
              style: Theme.of(context).textTheme.displayLarge?.copyWith(
                color: Theme.of(context).colorScheme.onSurface,
              ),
            ),
            Text(
              'Wave Right \n or \n Press Next',
              style: Theme.of(context).textTheme.displaySmall?.copyWith(
                color: Theme.of(context).colorScheme.onSurface,
              ),
              textAlign: TextAlign.center,
            ),
            Hero(
              tag: 'next_button',
              child: ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    SharedAxisPageRoute(
                      child: LanguagePage(),
                      transitionType: SharedAxisTransitionType.horizontal,
                    ),
                  );
                },
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(Symbols.keyboard_arrow_right_rounded, size: 24),
                    SizedBox(width: 8),
                    Text('Next'),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
