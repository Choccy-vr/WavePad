import 'dart:async';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class ClockWidget extends StatefulWidget {
  const ClockWidget({super.key});

  @override
  State<ClockWidget> createState() => _ClockWidgetState();
}

class _ClockWidgetState extends State<ClockWidget> {
  final ValueNotifier<DateTime> dateTimeNotifier = ValueNotifier<DateTime>(
    DateTime.now(),
  );
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    _startPreciseTimer();
  }

  void _startPreciseTimer() {
    final now = DateTime.now();
    final msUntilNextSecond = 1000 - now.millisecond;
    _timer = Timer(Duration(milliseconds: msUntilNextSecond), () {
      if (!mounted) return;
      dateTimeNotifier.value = DateTime.now();

      // Periodic updates every second
      _timer = Timer.periodic(const Duration(seconds: 1), (_) {
        if (!mounted) return;
        dateTimeNotifier.value = DateTime.now();
      });
    });
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return ValueListenableBuilder<DateTime>(
      valueListenable: dateTimeNotifier,
      builder: (context, currentDate, _) {
        return Column(
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            Text(
              DateFormat('hh:mm').format(currentDate),
              style: Theme.of(context).textTheme.displayLarge!.copyWith(
                color: Theme.of(context).colorScheme.onSurface,
                fontSize: 85,
              ),
            ),
            Text(
              DateFormat('MMMM d').format(currentDate),
              style: Theme.of(context).textTheme.displaySmall!.copyWith(
                color: Theme.of(context).colorScheme.onSurface,
              ),
            ),
          ],
        );
      },
    );
  }
}
