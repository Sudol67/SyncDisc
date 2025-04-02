# Synchronization application

Created purely for my private use. It was created after I bought external SSD and needed something that would sync files from my computer to SSD as a backup. Didin't want to use third-party software for privacy and saw opportunity to create something on my own.

Don't expect something groundbreaking but it has everything I need:
- It's fast
- It's simple
- It displays all the files that are missing from the target location
- It checks not only for missing files but also missing folder or even checks date of last modification. If the file in the source path has newer date of last modification, it will be added to syncing list.

It uses multithreading. It comes in handy when synchronization takes too much time or there is a problem.

How to use:
1. Run .exe file
2. Enter source path in the white box located on the upper left of the window. You can do it manually or by selecting folder with "choose path" button
3. Entering target path is analogical to entering source path. Target path box is located on the right side of the source path box
4. After enetring source and target paths, click 'Check' button on the bottom of the window. Program will perform comparing files between paths. After completing the task, all the elements added for syncing will be displayed
5. After checking for mising elements in target path, click 'Synchronize' button to start synchronization. Files already synced will be marked green. After syncing, message box will appear infroming about finishing the task
6. If something becomes stuck or syncing is taking too long, press 'STOP' button to abort the proces. It will not delete already copied files.

Future plans for improving the app:
- Better looking GUI
- When replacing file with newer version, older is moved to seperate archive folder. It will be an option in settings
- Adding size to file list (not necessary, just additional functionality)

Open for suggestions how to improve the app or reporting some bugs
