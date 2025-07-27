## If you don't want to read, you can just scroll down to the pics
(This project was originally started as a highway project, but I have not submitted it to highway; it is now only for SoM.)
### Reviewer? look at these videos too: 
https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Wave_Env_OnDevice.mp4
https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Wave_Setup_OnDevice.mp4
# Demo WavePad v1 (0.1a)

Hello and welcome to WavePad

Thank you for voting on WavePad; it means the world to me.

Please take your time and read this page. WavePad is a huge project and I would appreciate it if you saw what ll it can do and what went into it.

### Why is this the Demo Page?

WavePad is both a hardware and Software Project. You have the actual hardware, and you have WaveOS. WaveOS is very specialized and will only run on WavePad it might be able to run on a VM if you try hard enough but would would want to install an OS to demo a product. Then you have the hardware aspect. I do not have the actual hardware built yet, but this way I can still share my designs.

# What is WavePad?

WavePad is a project I have been working on long and hard on. The time shown on Hackatime is nowhere near close to what I spent on this because I might've been brainstorming, getting a rough design, or doing other stuff that was not done on a computer. WavePad is a gesture-controlled desktop companion. My best explanation is to pretend to put a Stream Deck, Apple Watch, and Hand Tracking all in one product, you get this. It is meant to resemble a Stream Deck Mini.

# Use Case For WavePad

WavePad can be used almost anywhere due to it only needing power and WiFi for most of its functions. Its intended use case is like a Stream Deck.  WavePad sits on your desk and helps you while you are working or playing. It can act like a macro pad for your pc, helping your productivity. It can play, pause music, and turn the volume up and down. But that is not it, you can also check the weather and more. This project is very complex. There will be more apps in the future. My idea is to control your smart home, get notifications from your phone and computer, open apps on your computer, control music on Spotify, and more. As you can tell, this is a huge undertaking and can't possibly be done in one summer. 

# What is WaveOS

WaveOS is a custom OS built by me based on Raspberry Pi OS Lite. WaveOS is made with rpi-image-gen. WaveEnv (Wave Environment) is the desktop environment for WaveOS, a sleek, minimalist environment meant not to distract you. It has a bunch of under-the-hood changes to make it work specifically for WavePad, but I won't bore you with those. 

# Apps?

Yes, WaveOS has apps like every computer. This helps WavePad achieve certain things like acting like a macropad, weather, etc. All of those apps they are WaveOS Sys Apps these are simply just apps that are part of the base system and most likely crucial to the device's function. But the best part you can build your own app with an app store to come!

# I can make my own app!

Yep, not fully supported yet, but I have made a tool called wvpkg that packages and installs wvpkg because of how WaveOS and WavePad are made. You can pretty much just make an app like any other Linux app, use wvpkg to package it, and install it to your WavePad. My plan is to get this more fleshed out and to add an app store.

# Hardware

## PCB

The PCB/Mainboard is a CM4 carrier board with a DSI port and a CSI port for the display and camera. It also has an SD card reader for storage and a USB-C port to power it and transmit data.

![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Schematic.png)
![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Schematic-USB.png)
![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Schematic-HighSpeed.png)
![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Schematic-SD.png)
![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/PCB.png)

# WaveOS

I can only put images and videos of all the user apps, try to just merge them in your mind

## Setup

An OOBE that sets up your WavePad and teaches you how to use it



https://github.com/user-attachments/assets/51294527-9858-40f4-9589-a049399a5ba1



![Setup-Wifi.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Setup-Wifi.png)

## Wave Env

The main app/environment of WaveOS 

![MainPage.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/MainPage.png)

## Weather

The main Weather frontend app. Weather is split into two backends, and the frontend backend never closes and shares the weather data to all apps, so any app can use the weather. The frontend is a normal app that takes the weather data and displays it to the user

![Main-Page-Weather.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Main-Page-Weather.png)

![Weather-Details.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Weather-Details.png)



https://github.com/user-attachments/assets/21fdbf74-21af-47ca-adb4-2b918eda360f



## Macro Pad

This app uses the HID feature of WavePad by emulating a Macropad when connected to a pc. In the future, you will be able to change the macros

![macro_pad.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/macro_pad.png)

![Macro-NoUSB.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Macro-NoUSB.png)

# Conclusion

There is some more detailed stuff I left out of here if you want go check the rest of the repo and check out my Journal.md and Devlogs on SoM. 

I hope you enjoyed this project; it has been one of my dream projects to make. To see more, go look at my Journal.MD File.
