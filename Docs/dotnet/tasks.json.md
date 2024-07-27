# `tasks.json`

Place the following contents in a file named `tasks` inside the `.vscode` folder of your Workspace

```
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}\\Samples\\CANMonitor\\CANMonitor.csproj",
                "-c",
                "Debug"
            ],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "publish",
            "type": "shell",
            "dependsOn": "build",
            "presentation": {
                "reveal": "always",
                "panel": "new"
            },
            "options": {
                "cwd": "${workspaceFolder}"
            },
            "windows": {
                "command": "${cwd}\\.vscode\\yoshipub.bat"
            },
            "problemMatcher": []
        }
    ]
}
```