import 'package:animations/animations.dart';
import 'package:flutter/material.dart';

class SharedAxisPageRoute<T> extends PageRouteBuilder<T> {
  final Widget child;
  final SharedAxisTransitionType transitionType;

  SharedAxisPageRoute({
    required this.child,
    this.transitionType = SharedAxisTransitionType.horizontal,
  }) : super(
         pageBuilder: (context, animation, secondaryAnimation) => child,
         transitionDuration: Duration(milliseconds: 300),
         transitionsBuilder: (context, animation, secondaryAnimation, child) {
           return SharedAxisTransition(
             animation: animation,
             secondaryAnimation: secondaryAnimation,
             transitionType: transitionType,
             child: child,
           );
         },
       );
}
