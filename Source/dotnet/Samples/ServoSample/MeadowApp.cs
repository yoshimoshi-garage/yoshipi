using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using YoshiPi;
using static Meadow.Resolver;

namespace ServoSample;

public class DisplayService
{
    private IPixelDisplay _display;
    private DisplayScreen _screen;
    private Box _upBox;
    private Box _downBox;
    private Box _leftBox;
    private Box _rightBox;
    private Box _centerBox;

    public DisplayService(IPixelDisplay display)
    {
        _display = display;
        _screen = new DisplayScreen(_display, RotationType._270Degrees)
        {
            BackgroundColor = Color.Black
        };
    }

    public async Task Start()
    {
        CreateLayouts();
    }

    private void CreateLayouts()
    {
        var uiWidth = 50;

        var y = (_screen.Height - uiWidth) / 2;

        _centerBox = new Box(
            (_screen.Width - uiWidth) / 2,
            (_screen.Height - uiWidth) / 2,
            uiWidth,
            uiWidth)
        {
            IsFilled = false,
            ForeColor = Color.Cyan
        };

        _upBox = new Box(
            (_screen.Width - uiWidth) / 2,
            (int)(_centerBox.Top - uiWidth * 1.5),
            uiWidth,
            (int)(uiWidth * 1.5))
        {
            IsFilled = false,
            ForeColor = Color.Cyan
        };

        _downBox = new Box(
            (_screen.Width - uiWidth) / 2,
            _centerBox.Bottom,
            uiWidth,
            (int)(uiWidth * 1.5))
        {
            IsFilled = false,
            ForeColor = Color.Cyan
        };

        _leftBox = new Box(
            (int)(_centerBox.Left - uiWidth * 1.5),
            _centerBox.Top,
            (int)(uiWidth * 1.5),
            uiWidth)
        {
            IsFilled = false,
            ForeColor = Color.Cyan
        };

        _rightBox = new Box(
            _centerBox.Right,
            _centerBox.Top,
            (int)(uiWidth * 1.5),
            uiWidth)
        {
            IsFilled = true,
            ForeColor = Color.Cyan
        };

        _screen.Controls.Add(_centerBox, _upBox, _downBox, _leftBox, _rightBox);
    }

    public void SetButtonState(bool isDown)
    {
        _centerBox.IsFilled = isDown;
    }

    public void SetXValue(float xValue)
    {
        _screen.BeginUpdate();

        var mid = 3.3 / 2d;
        var midl = mid - 0.1;
        var midh = mid + 0.1;

        _leftBox.IsFilled = false;
        _rightBox.IsFilled = false;

        if (xValue > midh)
        { // right
            var magnitude = (float)((xValue - mid) / (2 * mid));
            var color = Color.FromHsba(180, 1, magnitude);
            _rightBox.ForeColor = color;
            _rightBox.IsFilled = true;
        }
        else if (xValue < midl)
        { // left
            var magnitude = (float)((mid - xValue) / (2 * mid));
            var color = Color.FromHsba(180, 1, magnitude);
            _leftBox.ForeColor = color;
            _leftBox.IsFilled = true;
        }
        else
        {
            _leftBox.ForeColor = Color.Cyan;
            _rightBox.ForeColor = Color.Cyan;
        }

        _screen.EndUpdate();
    }

    public void SetYValue(float yValue)
    {
        _screen.BeginUpdate();

        var mid = 3.3 / 2d;
        var midl = mid - 0.1;
        var midh = mid + 0.1;

        _upBox.IsFilled = false;
        _downBox.IsFilled = false;

        if (yValue > midh)
        { // up
            var magnitude = (float)((yValue - mid) / (2 * mid));
            var color = Color.FromHsba(180, 1, magnitude);
            _upBox.ForeColor = color;
            _upBox.IsFilled = true;
        }
        else if (yValue < midl)
        { // down
            var magnitude = (float)((mid - yValue) / (2 * mid));
            var color = Color.FromHsba(180, 1, magnitude);
            _downBox.ForeColor = color;
            _downBox.IsFilled = true;
        }
        else
        {
            _upBox.ForeColor = Color.Cyan;
            _downBox.ForeColor = Color.Cyan;
        }

        _screen.EndUpdate();
    }
}

public class InputService
{
    public event EventHandler<bool>? ButtonStateChanged;
    public event EventHandler<float>? YValueChanged;
    public event EventHandler<float>? XValueChanged;

    public InputService(IDigitalInterruptPort buttonPort, IAnalogInputPort xInput, IAnalogInputPort yInput)
    {
        buttonPort.Changed += OnButtonChanged;
        xInput.Updated += OnXInputUpdated;
        yInput.Updated += OnYInputUpdated;

        xInput.StartUpdating(TimeSpan.FromMilliseconds(200));
        yInput.StartUpdating(TimeSpan.FromMilliseconds(200));
    }

    private void OnXInputUpdated(object? sender, IChangeResult<Meadow.Units.Voltage> e)
    {
        XValueChanged?.Invoke(this, (float)e.New.Volts);
    }

    private void OnYInputUpdated(object? sender, IChangeResult<Meadow.Units.Voltage> e)
    {
        YValueChanged?.Invoke(this, (float)e.New.Volts);
    }

    private void OnButtonChanged(object? sender, DigitalPortResult e)
    {
        Log.Info($"Button is {e.New.State}");

        ButtonStateChanged?.Invoke(this, e.New.State);
    }
}

public class MeadowApp : YoshiPiApp
{
    private DisplayService _displayService;
    private InputService _inputService;

    public override Task Initialize()
    {
        _displayService = new DisplayService(Hardware.Display);
        _inputService = new InputService(
            Hardware.Gpio.Pins.D00.CreateDigitalInterruptPort(
                InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
            Hardware.Adc.Pins.A01.CreateAnalogInputPort(1),
            Hardware.Adc.Pins.A00.CreateAnalogInputPort());

        _inputService.ButtonStateChanged += (s, e) => _displayService.SetButtonState(!e);
        _inputService.XValueChanged += (s, e) => _displayService.SetXValue(e);
        _inputService.YValueChanged += (s, e) => _displayService.SetYValue(e);

        return base.Initialize();
    }

    public override async Task Run()
    {
        await _displayService.Start();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
