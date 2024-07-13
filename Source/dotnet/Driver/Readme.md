# YoshiMaker YoshiPi

**At24Cxx I2C EEPROMs (AT24C32 / AT24C64 / AT24C128 / AT24C256)**

The **YoshiPi** library is designed for the [YoshiMaker YoshiPi carrier board](https://www.yoshimaker.com) for the Raspberry Pi Zero 2 W.

Full source code and samples are available in the [YoshiPi source repository on Github](https://github.com/yoshimoshi-garage/yoshipi).

YoshiPi depends on the [Meadow software stack](http://developer.wildernesslabs.co/) and allows easy integration of hundreds of peripherals from the [Meadow.Foundation](https://github.com/WildernessLabs/Meadow.Foundation) library with your Raspberry Pi Zero 2.

## Installation

You can install the library from within Visual studio using the the NuGet Package Manager or from the command line using the .NET CLI:

`dotnet add package YoshiPi`

## Usage

```csharp

public class MeadowApp : YoshiPiApp
{
    private TestService _testService;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Hardware.Button1.Clicked += (s, e) => Console.WriteLine("Button1 clicked!");

        return Task.CompletedTask;
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}

```

## Contributing

- **Found a bug?** [Report an issue](https://github.com/yoshimoshi-garage/yoshipi/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/yoshimoshi-garage/yoshipi/issues)
- Want to **contribute code?** Fork the [YoshiPi Repository](https://github.com/yoshimoshi-garage/yoshipi) and submit a pull request against the `develop` branch



