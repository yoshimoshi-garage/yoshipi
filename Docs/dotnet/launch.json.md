# `launch.json`

Place the following contents in a file named `launch.json` inside the `.vscode` folder of your Workspace

```
{
    "version": "0.2.0",
    "configurations": [
       {
           "name": "YoshiPi Remote Debugger",
           "type": "coreclr",
           "request": "launch",
           "preLaunchTask": "build",
           "program": "/home/pi/.dotnet/dotnet",
           "args": ["/home/pi/yoshipi/App.dll"],
           "cwd": "/home/pi/yoshipi",
           "stopAtEntry": false,
           "console": "internalConsole",
           "pipeTransport": {
               "pipeCwd": "${workspaceFolder}",
               "pipeProgram": "plink.exe",
               "pipeArgs": [
                   "-i",
                   "\"${userHome}\\.ssh\\yoshikey.ppk\"",
                   "pi@yoshipi.local"
               ],
               "debuggerPath": "/home/pi/vsdbg/vsdbg"
           }
       }
    ]
}
```