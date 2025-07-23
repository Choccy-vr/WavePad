import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'widgets/system_toolbar.dart';
import 'OpenApp.dart';

class DirectoryScanner {
  // Stream-based async scanning
  static Stream<FileSystemEntity> scanDirectoryStream(
    String directoryPath, {
    bool recursive = false,
  }) async* {
    final directory = Directory(directoryPath);

    try {
      if (await directory.exists()) {
        await for (final entity in directory.list(recursive: recursive)) {
          yield entity; // Streams each file as it's found
        }
      }
    } catch (e) {
      print('Error scanning directory: $e');
    }
  }
}

class AppsPage extends StatefulWidget {
  const AppsPage({super.key});

  @override
  State<AppsPage> createState() => _AppsPageState();
}

class _AppsPageState extends State<AppsPage> {
  final List<FileSystemEntity> _entities = [];
  bool _isScanning = false;
  final FocusNode _focusNode = FocusNode();

  void _handleKey(KeyEvent event, var context) {
    if (event is KeyDownEvent &&
        event.logicalKey == LogicalKeyboardKey.arrowLeft) {
      Navigator.pop(context);
    }
  }

  Future<void> _scanDirectoryAsync() async {
    setState(() {
      _isScanning = true;
      _entities.clear();
    });

    // Stream
    await for (final entity in DirectoryScanner.scanDirectoryStream(
      "~/Applications",
      recursive: false, // Do not scan all subdirectories
    )) {
      setState(() {
        _entities.add(entity);
      });
    }

    setState(() => _isScanning = false);
  }

  @override
  void initState() {
    super.initState();
    _scanDirectoryAsync(); // Scan apps on startup
  }

  @override
  Widget build(BuildContext context) {
    return Focus(
      autofocus: true,
      focusNode: _focusNode,
      onKeyEvent: (FocusNode node, KeyEvent event) {
        _handleKey(event, context);
        return KeyEventResult.ignored;
      },
      child: Scaffold(
        body: Center(
          child: Stack(
            children: [
              Column(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  //Toolbar
                  Align(
                    alignment: Alignment.topRight,
                    child: Padding(
                      padding: const EdgeInsets.all(5.0),
                      child: SystemToolbar(),
                    ),
                  ),
                  //Apps
                  Center(
                    child: SizedBox(
                      width: 700,
                      height: 360,
                      child: GridView.count(
                        mainAxisSpacing: 50,
                        crossAxisSpacing: 80,
                        crossAxisCount: 4,
                        children: _entities
                            .whereType<Directory>() // Look for folders
                            .map((entity) {
                              // Get just the folder name (App1, App2, etc.)
                              final appName = entity.path
                                  .split(Platform.pathSeparator)
                                  .last;

                              return SizedBox(
                                width: 136,
                                height: 136,
                                child: FloatingActionButton(
                                  heroTag: 'app_$appName',
                                  child: Image.file(
                                    File('${entity.path}/icon.png'),
                                  ),
                                  onPressed: () {
                                    print('App folder clicked: $appName');
                                    startApp('${entity.path}/$appName');
                                  },
                                ),
                              );
                            })
                            .toList(),
                      ),
                    ),
                  ),
                  //Loading indicator
                  if (_isScanning)
                    Align(
                      alignment: Alignment.bottomRight,
                      child: CircularProgressIndicator(),
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
