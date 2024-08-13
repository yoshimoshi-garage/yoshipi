using Meadow;
using Meadow.Hardware;
using YoshiPi;

namespace ServoSample;

public class MeadowApp : YoshiPiApp
{
    private DisplayService _displayService;
    private InputService _inputService;
    private ServoService _servoService;

    public override Task Initialize()
    {
        _displayService = new DisplayService(Hardware.Display);

        _inputService = new InputService(
            Hardware.Gpio.Pins.D00.CreateDigitalInterruptPort(
                InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
            Hardware.Adc.Pins.A01.CreateAnalogInputPort(3),
            Hardware.Adc.Pins.A00.CreateAnalogInputPort(3));

        _inputService.ButtonStateChanged += (s, e) => _displayService.SetButtonState(!e);
        _inputService.XValueChanged += OnXValueChanged;
        _inputService.YValueChanged += (s, e) => _displayService.SetYValue(e);


        _servoService = new ServoService(Hardware.MikroBus.I2cBus);

        return base.Initialize();
    }

    private void OnXValueChanged(object? sender, float e)
    {
        _displayService.SetXValue(e);
        _servoService.RotateTo(e);
    }

    public override async Task Run()
    {
        await _displayService.Start();
        _inputService.Start();
        _servoService.Start();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
