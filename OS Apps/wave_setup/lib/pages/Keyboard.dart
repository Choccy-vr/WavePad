import 'package:flutter/material.dart';
import 'package:virtual_keyboard_multi_language/virtual_keyboard_multi_language.dart';

class Keyboard extends StatefulWidget {
  final String title;
  final bool initialShiftEnabled;
  final bool isNumeric;
  final ValueChanged<String> onDone;

  const Keyboard({
    super.key,
    required this.title,
    this.initialShiftEnabled = false,
    this.isNumeric = false,
    required this.onDone,
  });

  @override
  _KeyboardState createState() => _KeyboardState();
}

class _KeyboardState extends State<Keyboard> {
  // Title of the keyboard.
  String title = '';
  // Holds the text that user typed.
  String text = '';
  // True if shift enabled.
  bool shiftEnabled = false;

  // is true will show the numeric keyboard.
  bool isNumericMode = false;

  late TextEditingController _controllerText;

  @override
  void initState() {
    super.initState();
    title = widget.title;
    shiftEnabled = widget.initialShiftEnabled;
    isNumericMode = widget.isNumeric;
    _controllerText = TextEditingController();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          children: <Widget>[
            Padding(
              padding: const EdgeInsets.all(25.0),
              child: Row(
                mainAxisSize: MainAxisSize.max,
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Align(
                    alignment: Alignment.centerLeft,
                    child: Text(
                      title,
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        color: Theme.of(context).colorScheme.onSurface,
                      ),
                    ),
                  ),
                  Row(
                    mainAxisSize: MainAxisSize.max,
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      OutlinedButton(
                        onPressed: () => widget.onDone('Cancelled'),
                        child: Text(
                          'Cancel',
                          style: TextStyle(
                            color: Theme.of(context).colorScheme.error,
                          ),
                        ),
                      ),
                      const SizedBox(width: 10),
                      ElevatedButton(
                        onPressed: () => widget.onDone(text),
                        child: const Text('Done'),
                      ),
                    ],
                  ),
                ],
              ),
            ),
            Expanded(
              child: Align(
                alignment: Alignment.topLeft,
                child: Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 25.0),
                  child: Text(
                    text,
                    style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                      color: Theme.of(context).colorScheme.onSurface,
                    ),
                    textAlign: TextAlign.left,
                  ),
                ),
              ),
            ),
            Container(
              color: Theme.of(context).colorScheme.surfaceContainer,
              child: VirtualKeyboard(
                height: 300,
                textColor: Theme.of(context).colorScheme.primary,
                textController: _controllerText,
                defaultLayouts: [VirtualKeyboardDefaultLayouts.English],
                type: isNumericMode
                    ? VirtualKeyboardType.Numeric
                    : VirtualKeyboardType.Alphanumeric,
                postKeyPress: _onKeyPress,
              ),
            ),
          ],
        ),
      ),
    );
  }

  /// Fired when the virtual keyboard key is pressed.
  void _onKeyPress(VirtualKeyboardKey key) {
    if (key.keyType == VirtualKeyboardKeyType.String) {
      text = text + ((shiftEnabled ? key.capsText : key.text) ?? '');
    } else if (key.keyType == VirtualKeyboardKeyType.Action) {
      switch (key.action) {
        case VirtualKeyboardKeyAction.Backspace:
          if (text.isEmpty) return;
          text = text.substring(0, text.length - 1);
          break;
        case VirtualKeyboardKeyAction.Return:
          text = '$text\n';
          break;
        case VirtualKeyboardKeyAction.Space:
          text = text + (key.text ?? '');
          break;
        case VirtualKeyboardKeyAction.Shift:
          setState(() {
            shiftEnabled = !shiftEnabled;
          });
          break;
        default:
      }
    }
    // Update the screen
    setState(() {});
  }
}
