# Getting Started

These will be a doc here on how to start local development.

## Troubleshooting

#### Win32Exception Can't find the specifiled file "python2.6"

You can get the error below when you are trying to build the app in Unity3D. To fix, change `FileName` on line 44 in `VunglePostBuilder.cs` to just `python`.

```
Win32Exception: ApplicationName='python2.6', 
CommandLine='"**/Ruzik-Odyssey-App/Ruzik Odyssey/Assets/Editor/Vungle/VunglePostProcessor.py" "**/sadf/sadf" "**/Ruzik-Odyssey-App/Ruzik Odyssey/Assets/Editor/Vungle/VungleSDK"', CurrentDirectory='', Native error= Cannot find the specified file
```
