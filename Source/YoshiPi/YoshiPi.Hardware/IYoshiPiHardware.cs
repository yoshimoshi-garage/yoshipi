using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;

namespace YoshiPi;

public interface IYoshiPiHardware
{
    IRelay Relay1 { get; }
    IRelay Relay2 { get; }
    IButton Button1 { get; }
    IButton Button2 { get; }

    GpioConnector Gpio { get; }
    AdcConnector Adc { get; }
    MikroBusConnector MikroBus { get; }
    II2cBus GroveI2c { get; }

    IRealTimeClock Rtc { get; }
    IPixelDisplay Display { get; }

    // remove after test
    Mcp23008 MCP { get; }
}
