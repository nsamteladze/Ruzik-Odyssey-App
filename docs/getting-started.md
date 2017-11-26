# Getting Started

## Local Development

First of all, dowload [Unity3D](https://unity3d.com/get-unity/download) and [Visual Studio 2017](https://www.visualstudio.com/downloads/). 

Next, checkout this repo and open the project in __Unity3D__ (not Visual Studio 2017). When you first open the project, Unity will import all the asset, so it will take a little while. Once done, you can select the start scene from `Assets` -> `Scenes` -> `Start Scene` and click `Play` to start the game in Unity3D.

Unity3D `Library` folder (includes settings, user preferences, caches and more) is not checked in to reduce the repository size. You can get the default settings from `LibraryDefault` folder. This will include the correct Build Settings and Player Settings. Just create a new `Library` folder (same root folder as `LibraryDefault`) and copy all the files from `LibraryDefault` into it.

To build the game for a particular platform, go to `Build` -> `Build Settings`, switch to the platform you want, and click `Build`. Once the platform is saved, you can just use `Ctrl + B` command.

#### How to Play

Use [Unity Remote](https://docs.unity3d.com/Manual/UnityRemote5.html) app on your phone to control the game. Install the app on your phone, connect your phone using a USB cable and open the app. Next, go to `Edit` -> `Project Settings` -> `Editor` and select your phone under `Unity Remote` -> `Device`. Now start the app on the Start Scene and use your phone to play.

## Troubleshooting

#### Win32Exception Can't find the specifiled file "python2.6"

You can get the error below when you are trying to build the app in Unity3D. To fix, change `FileName` on line 44 in `VunglePostBuilder.cs` to just `python`.

```
Win32Exception: ApplicationName='python2.6', 
CommandLine='
"**/Ruzik-Odyssey-App/Ruzik Odyssey/Assets/Editor/Vungle/VunglePostProcessor.py" 
"**/sadf/sadf" 
"**/Ruzik-Odyssey-App/Ruzik Odyssey/Assets/Editor/Vungle/VungleSDK"', 
CurrentDirectory='', Native error= Cannot find the specified file
```
