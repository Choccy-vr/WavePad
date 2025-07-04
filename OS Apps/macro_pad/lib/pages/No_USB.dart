import 'package:flutter/material.dart';
import 'package:material_symbols_icons/symbols.dart';

class No_USB extends StatelessWidget {
  const No_USB({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              Symbols.usb_off_rounded,
              size: 150,
              color: Theme.of(context).colorScheme.error,
            ),
            SizedBox(height: 20),
            Text(
              'Please connect your device to a computer via USB to continue.',
              style: Theme.of(context).textTheme.headlineLarge?.copyWith(
                color: Theme.of(context).colorScheme.onErrorContainer,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }
}
