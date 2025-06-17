import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'widgets/system_toolbar.dart';
import 'package:material_symbols_icons/symbols.dart';

class AppsPage extends StatelessWidget {
  const AppsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return KeyboardListener(
      focusNode: FocusNode(),
      autofocus: true,
      onKeyEvent: (KeyEvent event) {
        if (event is KeyDownEvent) {
          if (event.logicalKey == LogicalKeyboardKey.arrowLeft) {
            Navigator.pop(context); // Go back with right arrow
          }
        }
      },
      child: Scaffold(
        body: Center(
          child: Stack(
            children: [
              Column(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Align(
                    alignment: Alignment.topRight,
                    child: Padding(
                      padding: const EdgeInsets.all(5.0),
                      child: SystemToolbar(),
                    ),
                  ),
                  Center(
                    child: SizedBox(
                      width: 700,
                      height: 360,
                      child: GridView.count(
                        mainAxisSpacing: 50,
                        crossAxisSpacing: 80,
                        crossAxisCount: 4,
                        children: [
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app1',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app2',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app3',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app4',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app5',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app6',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app7',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
                            ),
                          ),
                          SizedBox(
                            width: 136,
                            height: 136,
                            child: FloatingActionButton(
                              heroTag: 'app8',
                              child: Icon(Symbols.add_2_rounded, size: 60),
                              onPressed: () {
                                print('App clicked');
                              },
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
