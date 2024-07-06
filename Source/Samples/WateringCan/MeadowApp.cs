using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Sensors.Volume;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using YoshiPi;

namespace YoshiMaker.WateringCan;

public class DisplayService
{
    private IPixelDisplay _display;
    private DisplayScreen _screen;
    private Box _emptyArea;
    private Box _waterLevelBox;

    public DisplayService(IPixelDisplay display)
    {
        _display = display;

        CreateLayouts();
    }

    private void CreateLayouts()
    {
        _screen = new DisplayScreen(_display, RotationType._270Degrees);

        var homeLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            BackgroundColor = Color.DarkGray
        };

        var titleLabel = new Label(110, 10, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "Watering Can",
            Font = new Font12x20(),
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        var outline = new Box(10, 10, 100, 220)
        {
            IsFilled = true,
            ForeColor = Color.WhiteSmoke
        };

        _emptyArea = new Box(12, 12, 96, 216)
        {
            IsFilled = true,
            ForeColor = Color.Black
        };

        _waterLevelBox = new Box(12, 12, 96, 216)
        {
            IsFilled = true,
            ForeColor = Color.Blue
        };

        homeLayout.Controls.Add(
            titleLabel,
            outline,
            _emptyArea,
            _waterLevelBox
            );

        _screen.Controls.Add(homeLayout);
    }

    public void SetWaterLevel(int percent)
    {
        // 216
        var height = 216 * percent / 100;
        _waterLevelBox.Height = height;
        _waterLevelBox.Top = _emptyArea.Top + (_emptyArea.Height - height);
    }
}

public class MeadowApp : YoshiPiApp
{
    private ResistiveTankLevelSender _levelSensor;
    private DisplayService _display;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _display = new DisplayService(Hardware.Display);

        _levelSensor = new ResistiveTankLevelSender_12in_33_240(Hardware.Adc.Pins.A00, 4.65.Volts());
        _levelSensor.FillLevelChanged += (s, e) => _display.SetWaterLevel(e);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        _levelSensor.StartUpdating();

        return base.Run();
    }


    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
