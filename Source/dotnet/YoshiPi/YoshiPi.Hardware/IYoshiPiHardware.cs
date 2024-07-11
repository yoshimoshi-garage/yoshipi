using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;

namespace YoshiPi;

public abstract class YoshiPiApp : App<RaspberryPi, YoshiPiHardwareProvider, IYoshiPiHardware>
{
}

public interface IYoshiPiHardware : IMeadowAppEmbeddedHardware
{
    IRelay Relay1 { get; }
    IRelay Relay2 { get; }
    IButton Button1 { get; }
    IButton Button2 { get; }

    GpioConnector Gpio { get; }
    AdcConnector Adc { get; }
    MikroBusConnector MikroBus { get; }
    II2cBus GroveI2c { get; }
    II2cBus Qwiic { get; }

    IRealTimeClock Rtc { get; }
    IPixelDisplay Display { get; }
    ICalibratableTouchscreen Touchscreen { get; }
}
