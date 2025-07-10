import 'package:flutter/material.dart';
import 'package:video_player/video_player.dart';

class LoopingVideoWidget extends StatefulWidget {
  final String assetPath;
  const LoopingVideoWidget({super.key, required this.assetPath});

  @override
  State<LoopingVideoWidget> createState() => _LoopingVideoWidgetState();
}

class _LoopingVideoWidgetState extends State<LoopingVideoWidget> {
  late VideoPlayerController _controller;

  @override
  void initState() {
    super.initState();
    _controller = VideoPlayerController.asset(widget.assetPath)
      ..setLooping(true)
      ..initialize().then((_) {
        setState(() {});
        _controller.play();
      });
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (!_controller.value.isInitialized) {
      return const Center(child: CircularProgressIndicator());
    }
    // Use AspectRatio to maintain the video's aspect ratio, but fill parent constraints
    return AspectRatio(
      aspectRatio: _controller.value.aspectRatio,
      child: VideoPlayer(_controller),
    );
  }
}
