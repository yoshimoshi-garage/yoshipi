# YoshiMaker.YoshiPi.Template

Contains a collection of Project Template(s) for the .NET IoT YoshiPi that runs Meadow.

Meadow is a complete, IoT platform with defense-grade security that runs full .NET applications on embeddable microcontrollers and Linux single-board computers including Raspberry Pi and NVIDIA Jetson.

For more information, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Installation

You can install YoshiPi project templates with the command:

```console
dotnet new install YoshiMaker.YoshiPi.Template
```

Once installed, it will list all the project templates included:

```console
Template Name            Short Name          Language        Tags
-----------------------  ------------------  --------------  -------------------
Meadow Core-Compute App  meadow-ccm          [C#],F#,VB.NET  Meadow/Console
```

## Usage

Open Visual Studio 2022, click on File -> New -> Project, in the _Create a new Project_ window, search for YoshiPi, and you will see the list of YoshiPi template(s) available you've just installed.

Alternatively, you can create a YoshiPi project via console using the _Short Name_ values on the list. For example:

```console
C:\Users\john>dotnet new meadow-feather --name Blinky
The template "Meadow F7 Feather App" was created successfully. 
```

Creates a basic YoshiPi application that shows an HMI with a button you can press on its touch-capable display.

## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/yoshimoshi-garage/yoshipi/issues)
- Have a **feature idea or enhancement request?** [Open a new feature request](https://github.com/yoshimoshi-garage/yoshipi/issues)

## Need Help?

If you have questions or need assistance, please join the .NET IoT Hardware [community on Discord](https://discord.gg/6fKG7UXGQK).