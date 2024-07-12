# Debugging .NET YoshiPi Applications with VS Code

This document covers getting a VS Code remove debugger working with your YoshiPi.  It allows you to build on your development machine, push the compiled binaries to the target Raspberry Pi Zero 2 W, and then live debug the application.


## Visual Studio and Remote Debugging

First we'll address the elephant in the room: why doesn't this document cover Visual Studio, since that's what most .NET develoeprs use?

Well, honestly, because Microsoft has not prioritized device development for decades, and getting the debugger working from Visual Studio is difficult, fragile, and unsupported (i.e. will probably break).  We've had very little luck getting it to work reliably and have given up trying.

Even VSCode is not straightforward and requires some not-so-intuitive steps

## Install PuTTY

Much as we tried to avoid having to use another library, debugging will need PuTTY (actually PLINK) to do the non-interactive commands for the debugger.  PuTTY must be installed an PLINK must be in your path.

`plink` doesn't have a command line argument for version, so checking to see if you have it installed is done by running `plink` from a console/PowerShell terminal.  If you **don't** get an error about it being a recognized command, then you're all good.

i.e. if you see this, your install is **not** complete:

```
> plink
plink : The term 'plink' is not recognized as the name of a cmdlet, function, script file, or operable program.
Check the spelling of the name, or if a path was included, verify that the path is correct and try again.
```

## On Raspberry Pi: Create target folders

On the Raspberry Pi, we need to make sure a couple folders that will be used exist
```
mkdir ~/.ssh
mkdir ~/yoshipi
```

## On Windows: Create a Key Pair

We're going top create a key pair exclusively for the YoshiPi.  This isn't strictly required, but it allows the instructions to work for everyone regardless of any other keys they may have created, and keeps the batch and script files that automate things easier to maintain.

```
PS> ssh-keygen -f "$HOME\.ssh\yoshikey" -N '""'
```

## Copy the key to the Raspberry Pi

```
type $HOME\.ssh\yoshikey.pub | ssh pi@yoshipi.local "cat >> .ssh/authorized_keys"
```

## On Raspberry Pi: Installing the Debug Server

First, you need to install a debug server on the Raspebrry Pi, so SSH in to it and run the following.  This assumes you've already done the YoshiPi prerequisties.

```
cd ~
curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

```

## Open your Solution Folder and create a `.vscode` Folder

Open your Solution folder in VSCode.  Add a new folder to the root, if it doesn't exist, called `.vscode`


## Add `yoshipub.bat`, `launch.json` and `tasks.json` to your project environment

Add three files to the `.vscode` folder (click to see the contents of each): 
 - [`yoshipub.bat`](yoshipub.bad.md)
 - [`tasks.json`](tasks.json.md)
 - [`launch.json`](launch.json.md)
 
## Modify `tasks.json` to point to your desired startup project

We've not yet figured out a way to automate or script setting the startup project, so this requires some manual work on your part.

Open `tasks.json` and navigate to the `args` of the `build` task.  You will see a line that currently has this:

> `"${workspaceFolder}\\Samples\\CANMonitor\\CANMonitor.csproj",`

Modify this to be the path of your startup (YoshiPi) application.  You can see that the existing path is relative to the workspace folder, and is pointing at one of the YoshiPi samples as an example.