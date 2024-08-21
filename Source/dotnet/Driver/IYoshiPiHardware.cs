using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;

namespace YoshiPi;

/// <summary>
/// Represents the hardware interface for the YoshiPi device, providing access to various peripherals and connectors.
/// </summary>
public interface IYoshiPiHardware : IMeadowAppEmbeddedHardware
{
    /// <summary>
    /// Gets the first relay.
    /// </summary>
    IRelay Relay1 { get; }

    /// <summary>
    /// Gets the second relay.
    /// </summary>
    IRelay Relay2 { get; }

    /// <summary>
    /// Gets the first button.
    /// </summary>
    IButton Button1 { get; }

    /// <summary>
    /// Gets the second button.
    /// </summary>
    IButton Button2 { get; }

    /// <summary>
    /// Gets the GPIO connector.
    /// </summary>
    GpioConnector Gpio { get; }

    /// <summary>
    /// Gets the ADC connector.
    /// </summary>
    AdcConnector Adc { get; }

    /// <summary>
    /// Gets the MikroBus connector.
    /// </summary>
    MikroBusConnector MikroBus { get; }

    /// <summary>
    /// Gets the Grove I2C bus.
    /// </summary>
    II2cBus GroveI2c { get; }

    /// <summary>
    /// Gets the Qwiic I2C bus.
    /// </summary>
    II2cBus Qwiic { get; }

    /// <summary>
    /// Gets the real-time clock.
    /// </summary>
    IRealTimeClock Rtc { get; }

    /// <summary>
    /// Gets the pixel display.
    /// </summary>
    IColorInvertableDisplay Display { get; }

    /// <summary>
    /// Gets the calibratable touchscreen.
    /// </summary>
    ICalibratableTouchscreen Touchscreen { get; }
}
