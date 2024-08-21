using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Relays;
using Meadow.Foundation.RTCs;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using System;

namespace YoshiPi;

/// <summary>
/// Represents YoshiPi hardware revision v1b
/// </summary>
public class YoshiPi_v1b : IYoshiPiHardware, IDisposable
{
    private readonly RaspberryPi _device;
    private readonly Mcp23008 _mcp23008;
    private readonly Mcp3004 _mcp3004;
    private readonly GpioConnector _gpio;
    private readonly AdcConnector _adc;
    private readonly MikroBusConnector _mikrobus;
    private IRelay? _relay1;
    private IRelay? _relay2;
    private IButton? _button1;
    private IButton? _button2;
    private readonly IDigitalInterruptPort? _mcpInt;
    private IRealTimeClock? _rtc;
    private IColorInvertableDisplay? _display;
    private ICalibratableTouchscreen? _touchscreen;
    private IDigitalOutputPort _displayCsOutputPort;
    private IDigitalOutputPort _displayDcOutputPort;
    private IDigitalOutputPort _displayResetOutputPort;
    private bool _isDisposed;
    private IDigitalOutputPort _touchChipSelectPort;

    /// <inheritdoc/>
    public IMeadowDevice ComputeModule => _device;
    /// <inheritdoc/>
    public GpioConnector Gpio => _gpio;
    /// <inheritdoc/>
    public AdcConnector Adc => _adc;
    /// <inheritdoc/>
    public II2cBus GroveI2c => _device.CreateI2cBus();
    /// <inheritdoc/>
    public II2cBus Qwiic => _device.CreateI2cBus();
    /// <inheritdoc/>
    public MikroBusConnector MikroBus => _mikrobus;

    /// <inheritdoc/>
    public IRelay Relay1 => _relay1 ??= new Relay(_device.Pins.GPIO16.CreateDigitalOutputPort(false));
    /// <inheritdoc/>
    public IRelay Relay2 => _relay2 ??= new Relay(_mcp23008.Pins.GP3.CreateDigitalOutputPort(false));
    /// <inheritdoc/>
    public IRealTimeClock Rtc => _rtc ??= new Ds3231(_device.CreateI2cBus());
    /// <inheritdoc/>
    public Mcp23008 MCP => _mcp23008;

    internal YoshiPi_v1b(RaspberryPi device)
    {
        _device = device;

        try
        {
            _mcpInt = _device.Pins.GPIO18.CreateDigitalInterruptPort(InterruptMode.EdgeRising, ResistorMode.Disabled);
        }
        catch
        {
            Resolver.Log.Error("Unable to create MCP23008 Interrupt - it may be in use");
        }

        _displayCsOutputPort = _device.Pins.GPIO4.CreateDigitalOutputPort(true);
        _displayDcOutputPort = _device.Pins.GPIO23.CreateDigitalOutputPort();
        _displayResetOutputPort = _device.Pins.GPIO24.CreateDigitalOutputPort();

        _mcp23008 = new Mcp23008(
            _device.CreateI2cBus(1),
            address: (byte)Mcp23008.Addresses.Default,
            interruptPort: _mcpInt,
            resetPort: _device.Pins.GPIO17.CreateDigitalOutputPort(false)
            );

        // pull the touch CS high right away in the event an app isn't using the touchscreen
        _touchChipSelectPort = _mcp23008.Pins.GP7.CreateDigitalOutputPort(true);

        _mcp3004 = new Mcp3004(
            _device.CreateSpiBus(
                _device.Pins.GPIO21,
                _device.Pins.GPIO20,
                _device.Pins.GPIO19,
                new Frequency(1, Frequency.UnitType.Megahertz)),
            _device.Pins.Pin24.CreateDigitalOutputPort(true));

        _gpio = new GpioConnector(
            "GPIO",
            new PinMapping
            {
                new PinMapping.PinAlias(GpioConnector.PinNames.D00, device.Pins.GPIO7),
                new PinMapping.PinAlias(GpioConnector.PinNames.D01, device.Pins.GPIO5),
                new PinMapping.PinAlias(GpioConnector.PinNames.D02, device.Pins.GPIO6),
                new PinMapping.PinAlias(GpioConnector.PinNames.D03, device.Pins.GPIO13),

                new PinMapping.PinAlias(GpioConnector.PinNames.D04, device.Pins.GPIO27), // also MB_INT
                new PinMapping.PinAlias(GpioConnector.PinNames.D05, device.Pins.GPIO22), // also MB_RST
                new PinMapping.PinAlias(GpioConnector.PinNames.D06, device.Pins.GPIO25), // also MB_CS
                new PinMapping.PinAlias(GpioConnector.PinNames.D07, device.Pins.GPIO12), // also MB_PWM
                new PinMapping.PinAlias(GpioConnector.PinNames.D08, _mcp23008.Pins.GP7), // also TS_CS
                new PinMapping.PinAlias(GpioConnector.PinNames.D09, device.Pins.GPIO26), // also TS_INT

            });

        _adc = new AdcConnector(
            "ADC",
            new PinMapping
            {
                new PinMapping.PinAlias(AdcConnector.PinNames.A00, _mcp3004.Pins.CH0),
                new PinMapping.PinAlias(AdcConnector.PinNames.A01, _mcp3004.Pins.CH1),
                new PinMapping.PinAlias(AdcConnector.PinNames.A02, _mcp3004.Pins.CH2),
                new PinMapping.PinAlias(AdcConnector.PinNames.A03, _mcp3004.Pins.CH3),
            });

        _mikrobus = new MikroBusConnector(
            "MIKROBUS1",
            new PinMapping
            {
                new PinMapping.PinAlias(MikroBusConnector.PinNames.AN, _mcp3004.Pins.CH3),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.RST, device.Pins.GPIO22),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.CS, device.Pins.GPIO25),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.SCK, device.Pins.GPIO21),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.CIPO, device.Pins.GPIO19),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.COPI, device.Pins.GPIO20),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.PWM, device.Pins.GPIO12),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.INT, device.Pins.GPIO27),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.RX, device.Pins.GPIO15),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.TX, device.Pins.GPIO14),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.SCL, device.Pins.I2C1_SCL),
                new PinMapping.PinAlias(MikroBusConnector.PinNames.SDA, device.Pins.I2C1_SDA),
            },
            device.PlatformOS.GetSerialPortName("serial0")!,
            new I2cBusMapping(device, 1),
            new SpiBusMapping(device, device.Pins.SPI1_SCLK, device.Pins.SPI1_MOSI, device.Pins.SPI1_MISO)
            );
    }

    /// <inheritdoc/>
    public IButton Button1
    {
        get
        {
            if (_button1 == null)
            {
                var port = _mcp23008.Pins.GP6.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                _button1 = new PushButton(port);
            }

            return _button1;
        }
    }

    /// <inheritdoc/>
    public IButton Button2
    {
        get
        {
            if (_button2 == null)
            {
                var port = _mcp23008.Pins.GP5.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                _button2 = new PushButton(port);
            }

            return _button2;
        }
    }

    /// <inheritdoc/>
    public IColorInvertableDisplay Display
    {
        get
        {
            _display ??= new Ili9341(
                    _device.CreateSpiBus(0,
                        new Frequency(10, Frequency.UnitType.Megahertz)),
                    _displayCsOutputPort,
                    _displayDcOutputPort,
                    _displayResetOutputPort,
                    240, 320);

            var backlight = _mcp23008.Pins.GP4.CreateDigitalOutputPort(true);
            backlight.State = true;

            return _display;
        }
    }

    /// <inheritdoc/>
    public ICalibratableTouchscreen Touchscreen
    {
        get => _touchscreen ?? new Xpt2046(
            _device.CreateSpiBus(1,
                new Frequency(1, Frequency.UnitType.Megahertz)),
            _device.Pins.GPIO26.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.Disabled),
            _touchChipSelectPort,
            RotationType.Normal);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _displayCsOutputPort.Dispose();
                _displayDcOutputPort.Dispose();
                _displayResetOutputPort.Dispose();
            }

            _isDisposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}