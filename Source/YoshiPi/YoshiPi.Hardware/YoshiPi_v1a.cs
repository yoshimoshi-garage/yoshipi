using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Relays;
using Meadow.Foundation.RTCs;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace YoshiPi;

public class YoshiPi_v1a : IYoshiPiHardware
{
    private readonly RaspberryPi _device;
    private readonly Mcp23008 _mcp23008;
    private readonly Mcp3004 _mcp3004;
    private readonly GpioConnector _gpio;
    private readonly AdcConnector _adc;
    private IRelay? _relay1;
    private Relay _relay2;
    private IButton? _button1;
    private IButton? _button2;
    private readonly IDigitalInterruptPort _mcpInt;
    private IRealTimeClock? _rtc;
    private IPixelDisplay? _display;

    public GpioConnector Gpio => _gpio;
    public AdcConnector Adc => _adc;

    public IRelay Relay1 => _relay1 ??= new Relay(_device.Pins.GPIO16.CreateDigitalOutputPort(false));
    public IRelay Relay2 => _relay2 ??= new Relay(_mcp23008.Pins.GP3.CreateDigitalOutputPort(false));
    public IRealTimeClock Rtc => _rtc ??= new Ds3231(_device.CreateI2cBus());
    public Mcp23008 MCP => _mcp23008;

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

    public IPixelDisplay Display
    {
        get
        {
            _display ??= new Ili9341(
                    _device.CreateSpiBus(
                        _device.Pins.GPIO11,
                        _device.Pins.GPIO10,
                        _device.Pins.GPIO9,
                        new Frequency(10, Frequency.UnitType.Megahertz)),
                    _device.Pins.GPIO4,
                    _device.Pins.GPIO23,
                    _device.Pins.GPIO24,
                    240, 320);

            var backlight = _mcp23008.Pins.GP4.CreateDigitalOutputPort(true);
            backlight.State = true;

            return _display;
        }
    }

    internal YoshiPi_v1a(RaspberryPi device)
    {
        _device = device;

        _mcpInt = _device.Pins.GPIO18.CreateDigitalInterruptPort(InterruptMode.EdgeRising, ResistorMode.Disabled);

        _mcp23008 = new Mcp23008(
            _device.CreateI2cBus(1),
            address: (byte)Mcp23008.Addresses.Default,
            interruptPort: _mcpInt,
            resetPort: _device.Pins.GPIO17.CreateDigitalOutputPort(false)
            );

        _mcp3004 = new Mcp3004(
            _device.CreateSpiBus(
                _device.Pins.GPIO21,
                _device.Pins.GPIO20,
                _device.Pins.GPIO19,
                new Frequency(2.34, Frequency.UnitType.Megahertz)),
            _device.Pins.GPIO25.CreateDigitalOutputPort(true));

        _gpio = new GpioConnector(
            "GPIO",
            new PinMapping
            {
                new PinMapping.PinAlias(GpioConnector.PinNames.D00, device.Pins.GPIO7),
                new PinMapping.PinAlias(GpioConnector.PinNames.D01, device.Pins.GPIO5),
                new PinMapping.PinAlias(GpioConnector.PinNames.D02, device.Pins.GPIO6),
                new PinMapping.PinAlias(GpioConnector.PinNames.D03, device.Pins.GPIO13),
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


    }

}