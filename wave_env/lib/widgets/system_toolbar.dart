import 'package:flutter/material.dart';

class SystemToolbar extends StatelessWidget {
  const SystemToolbar({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 125,
      height: 55,
      decoration: BoxDecoration(
        color: Theme.of(context).colorScheme.surfaceContainer,
        borderRadius: BorderRadius.circular(27.5),
      ),
      child: Row(
        children: [
          SizedBox(
            width: 60,
            height: 60,
            child: Center(
              child: Icon(
                Icons.usb_off_rounded,
                color: Theme.of(context).colorScheme.error,
                size: 30,
              ),
            ),
          ),
          SizedBox(
            width: 60,
            height: 60,
            child: Center(
              child: Icon(
                Icons.wifi_rounded,
                color: Theme.of(context).colorScheme.onSurfaceVariant,
                size: 30,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
