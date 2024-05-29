using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Relays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors.Buttons;

namespace YoshiPi;

public class YoshiPi_v1a : IYoshiPiHardware
{
    private readonly RaspberryPi _device;
    private readonly Mcp23008 _mcp23008;
    private readonly Mcp3004 _mcp3004;
    private IRelay? _relay1;
    private Relay _relay2;
    private GpioConnector _gpio;
    private IButton? _button1;
    private IButton? _button2;

    public IRelay Relay1 => _relay1 ??= new Relay(_device.Pins.GPIO16.CreateDigitalOutputPort(false));
    public IRelay Relay2 => _relay2 ??= new Relay(_mcp23008.Pins.GP3.CreateDigitalOutputPort(false));

    public IButton Button1
    {
        get
        {
            if (_button1 == null)
            {
                var port = _mcp23008.Pins.GP6.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.ExternalPullUp);
                port.Changed += (s, e) => { Resolver.Log.Info("GP6 Interrupt"); };
                _button1 = new PushButton(port);

            }

            return _button1;
        }
    }

    public IButton Button2 => _button2 ??= new PushButton(_mcp23008.Pins.GP5, ResistorMode.ExternalPullUp);

    public GpioConnector Gpio => _gpio;

    private IDigitalInterruptPort _mcpInt;

    internal YoshiPi_v1a(RaspberryPi device)
    {
        _device = device;

        _mcpInt = _device.Pins.GPIO18.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, ResistorMode.InternalPullUp);
        _mcpInt.Changed += (s, e) => { Resolver.Log.Info("MCP Interrupt"); };

        _mcp23008 = new Mcp23008(
            _device.CreateI2cBus(1),
            address: (byte)Mcp23008.Addresses.Default,
            interruptPort: _mcpInt,
            resetPort: _device.Pins.GPIO17.CreateDigitalOutputPort(false)
            );

        _gpio = new GpioConnector(
            "GPIO",
            new PinMapping
            {
                new PinMapping.PinAlias(GpioConnector.PinNames.D00, device.Pins.GPIO7),
                new PinMapping.PinAlias(GpioConnector.PinNames.D01, device.Pins.GPIO5),
                new PinMapping.PinAlias(GpioConnector.PinNames.D02, device.Pins.GPIO6),
                new PinMapping.PinAlias(GpioConnector.PinNames.D03, device.Pins.GPIO13),
            });


    }

}