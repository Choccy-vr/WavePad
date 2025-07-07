---
title: "WavePad"
author: "Ginobeano"
description: "Gesture Controlled Desktop Companion"
created_at: "2025-05-22"
---

# May 22nd: Started Repo

- Added README.md and JOURNAL.md

- Submited my pitch to the Slack server

**Total time spent: 1.5h**

# May 23rd: Started PCB

- Added PCB files

- Add CSI Port

- Add USB C with Power and Data

- Added BOM of what I have so far

![Screenshot 2025-05-22 221017.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Screenshot%202025-05-23%20160045.png)

**Total time spent: 4h**

# May 24th: Worked on PCB

- Finished Schematic

- Finished Routing

- Encountered a problem with connecting GND to dense areas

![](C:\Users\wante\AppData\Roaming\marktext\images\2025-05-24-19-48-04-image.png)

**Total time spent: 3h**

# May 25th: Started work on base OS

- Created base OS using Rpi-Image-Gen.

- Based off slim example

**Total time spent: 2h**

# May 26th: Started work on desktop enviroment

- Designed first few screens in Figma
- Researched USB Device Classes
- Started learning Flutter & Dart

**Total time spent: 1.5h**

# May 28th: Learning Flutter & Fixed OS

- Continued learning Flutter & Dart
- Added Composite USB Device in WaveOS

**Total time spent: 2h**

# June 3rd: Finished v1.0 PCB

- Redid entire PCB

- Finished V1.0 PCB

![](C:\Users\wante\AppData\Roaming\marktext\images\2025-06-03-21-37-12-image.png)

**Total time spent: 3h**

# June 17th: Back To Work

- I finally finished my other project and ready to work on this one! I have a ton of motivation right now.

- I made the layout of the Home page most of the information are just placeholders right now until I implement all of the features but stuff like time works great.

- I think I am finally getting a hang of Flutter somethings I don't really like but I can't deny it is probably the best way to make good looking UI
  
  ![Screenshot 2025-06-17 102922.png](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Screenshot%202025-06-17%20102922.png)

**Total Time Spent: 3h**

- I made the Apps Screen too I think it looks really good

- I also researched and decided how I am going to handle data from services like weather to allow for easy cross app communication

- I am running into this weird bug on my home screen where I have to double press the right arrow to make it transtion to the new page but the apps page doesn't have that issue

![](C:\Users\wante\AppData\Roaming\marktext\images\2025-06-17-16-01-51-image.png)

**Total Time Spent: 2h**

# June 18th: Fixed Navigation Bug

- I finally the bug where you have to double press the right arrow to navigate to the next page

- I still have to decide where I put the apps so the Enviroment can recognize it.

**Total Time Spent: 1h**

# June 19 & 20th: app installing

- I made a package installer for packaging linux executables as .wvpkg

- I made a new file format for apps called wvpkg (Wave Package)

- I think it sounds pretty good.

- I also made it install all apps to ~/Applications a new folder in WaveOS

- Wave_Env (Desktop Enviroment) will scan this directory for apps and get the icon from the image in the app folder

# June 20th: OS

- Now comes the hard part the OS

- I already have the base OS based oof of Raspberry Pi OS Lite

- But I am now creating and database manager for cross app communication

- I also have to create the system services for a bunch of the apps to get them to work in the background

**Total Time Spent: 6h**

# June 21st: SQLite Manager

- Good progress was made today, and really, the past two days. 

- I basically finished my SQLite Manager. 

- SQLite Manager is a class I made that pretty much simplifies the SQLite experience for me. 

- I organized the project, but it was nothing big. 

- I also made it make a .wvdb file instead of a .db file simply because it looks more professional in my opinion. 

- The thing I am most proud of I did today was simplifying the creation of a table, it went from what I believe is a mess of a string with no hints on what you want to something nice and simple, shown in the picture.

- ![](https://github.com/Choccy-vr/WavePad/blob/main/Jounal%20Img/Screenshot%202025-06-21%20143021.png)

**Total TIme Spent: 5h**

# June 22nd: Pipes!

- I basically finsished all I need to do for the SQLite Manager script so now I am handling comunication from WaveDB to other apps

- I am going to use named Pipes for the apps comunicating with WaveDB

- I probably should've used this instead of a database but I have already gotten this far can't stop now.

- And also it makes it feel more professional and me smarter.

- Writing data should work now but I still have to work on reading data.

**Total Time Spent: 2h**

# June 23rd: Pipes & JSON

- I finished at least v1 of the Pipe Manager

- I switched the data type to JSON 

- I have no idea what I was thinking trying to do it by strings

- Writing data works

- and Reading data works now with where & order by clauses

- I removed isSetupComplete because that should be handled within Setup's wvdb

- I built all the apps and added them to WaveOS

- If I had the hardware technically I could build WaveOS and have a running OS altough with a lot of problems.

**Total Time Spent: 4h**

# June 26th: Weather

- I didn't journal for a few days but it was all working on weather so I will just add it to today.

- I started work on the Weather Backend Service.

- The structure for the app is going to be a Backend service that runs in the background to get all the data and a Frontend which will get the data and display for the user.

- The weather data will be stored in a accessable .wvdb so other apps can go in and get the same weather data the rest of the system uses.

- I am using Open Meteo because I don't need a API key and I have had experience with it before.

- Right now I am updating my SQLite_Manager to support more complex tables right now it is just key-value pairs.

**Total Time Spent: 6h**

# June 27th: Weather Backend Finished

- I finished the Weather Backend 

- I finished improving the SQLite_Manager

- Weather Backend also includes the Location Service

- In the future this might be sepprated but as for now this works.

- On to designing the Weather ui.

**Total Time Spent: 4h**

# June 28th: Weather UI design

- I worked on designing the UI for the weather app

- I only made it a basic mock up and going to finalize it in the flutter app

- I just remembered I have to use pipes in this app hopefully dart supports named pipes.
  
  ![Main Page (1).png](C:\Users\wante\WavePad\Jounal%20Img\Main%20Page%20(1).png)
  
  ![Details (1).png](C:\Users\wante\WavePad\Jounal%20Img\Details%20(1).png)

**Total Time Spent: 3.5h**

# June 30th: Home Page in Flutter

- I turned the home page into a flutter app

- right now there is no functionality but I am going to add geting the weather once I am done the ui.

- I added a Last Updated Time

![Main-Page-Flutter.png](C:\Users\wante\WavePad\Jounal%20Img\Main-Page-Flutter.png)

**Total Time Spent: 1.5h**

# July 1st: Start of Month

- I worked alot on the detail pages 

- I worked for around 8.5hrs I am soooooo done

- I got navigation to work

- FInished the 3 pages: Today, Tommorow, Week

**Total Time Spent: 9h**

# July 3rd: Finished Weather

- I am finally DONE with the weather app

- I was so over that stupid app 

- but it is done and I am pretty happy with the result

- What the final product looks like is the picture from July 30th because I didn't really do alot of UI changes just made it actually worked

- I also don't want to get doxxed because location works too!

- I also made some changes to the SQLite_Manager for reading Row data

- I am now moving on to a macro app (like a Stream Deck)

- Also this is today and yesterday. Forgot to journal

**Total Time Spent: 8h**

# July 4th: Happy 4th of July

- I started work on the macro pad app 

- I got the UI done 

- but that is where the good news ends

- I thought it would be pretty simple to send HID commands to a pc

- best case just a dart package worse case maybe a platform channel with a pretty easy HID library.

- NO

- I have been researching turns out I need to basically interact with the raw USB interface

- I am just going to keep searching for something that will help me 



![Screenshot 2025-07-04 151632.png](C:\Users\wante\WavePad\Jounal%20Img\Screenshot%202025-07-04%20151632.png)

![Screenshot 2025-07-04 151949.png](C:\Users\wante\WavePad\Jounal%20Img\Screenshot%202025-07-04%20151949.png)

**Total Time Spent: 4h**

# July 6th: OS Modifications & Macro-Pad App

- I made some modifications to the os 

- I added logic for first boot and stuff like that

- I also added the usb functionailty

- I finished the Macro-Pad app and it now functions exactly like a macropad

- I made it able to detect if you are connected to a pc not just a wall outlet and throws an error if not connceted to a pc

- I am now going to modify a few OS things and make a OOBE and v1 (v0.0.1) of Wave OS is complete!

**Total Time Spent: 5h**
