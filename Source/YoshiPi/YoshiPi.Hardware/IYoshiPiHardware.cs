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
}
