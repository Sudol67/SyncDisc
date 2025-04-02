# Synchronization application

![Logo of an app. It is a dark blue rectangle with white text 'SyncDisc' on the bottom. The text is in Century Schoolbook font with bold formatting](/Synchronizacja/Resources/Logo.png)

Created purely for my personal use. This project was developed after I bought an external SSD and needed a tool to sync files from my computer to the SSD as a backup. I didn’t want to use a third-party software due to privacy concerns and saw an opportunity to create something on my own.

This tool isn't meant to be revolutionary — just practical and efficient for my needs:
- Fast and lightweight
- Easy to use
- Detects files and folders missing from the backup destination
- Compares last modified dates to identify updated files that need syncing
- It uses multithreading — helpful when syncing takes a while or when something slows things down.

How to use:
1. Run the .exe file.
2. Enter the source path using the white box in the upper left corner of the window. You can type it manually or use the "Choose Path" button to select a folder.
3. Enter the target path the same way — the target path box is located to the right of the source path box.
4. Once both paths are entered, click the "Check" button at the bottom of the window. The program will compare files between the two locations. When the check is complete, all files and folders that need to be synced will be listed.
5. Click the "Synchronize" button to start the sync. Files that have been successfully synced will be marked in green. After the process finishes, a message box will appear to confirm completion.
6. If something gets stuck or the sync takes too long, click the "STOP" button to abort the process. Already copied files will not be deleted.

Future plans for improving the app:
- A better-looking GUI
- Polish language support
- Option to move replaced files (when syncing newer versions) to a separate archive folder
- Displaying file sizes in the list (optional, just an extra feature)

I’m open to suggestions for improving the app or fixing any bugs — feel free to reach out!
