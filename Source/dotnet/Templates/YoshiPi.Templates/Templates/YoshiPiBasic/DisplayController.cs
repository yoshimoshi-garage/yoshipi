using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YoshiPiApplication.Template;

public class DisplayController
{
    private IColorInvertableDisplay display;
    private ICalibratableTouchscreen touchscreen;
    private DisplayScreen displayScreen;
    private Label label;

    private int count = 0;

    public DisplayController(IColorInvertableDisplay display, ICalibratableTouchscreen touchscreen)
    {
        this.display = display;
        this.touchscreen = touchscreen;

        displayScreen = new DisplayScreen((IPixelDisplay)display, RotationType._270Degrees, touchscreen);
    }

    public async Task Start()
    {
        await CheckTouchscreenCalibration();
        CreateLayouts();
    }

    private async Task CheckTouchscreenCalibration()
    {
        var calfile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ts.cal"));

        Resolver.Log.Info($"Using calibration data at {calfile.FullName}");

        var cal = new TouchscreenCalibrationService(displayScreen, calfile);

        var existing = cal.GetSavedCalibrationData();

        if (existing != null)
        {
            touchscreen.SetCalibrationData(existing);
        }
        else
        {
            await cal.Calibrate(true);
        }
    }

    private void CreateLayouts()
    {
        displayScreen.BackgroundColor = Color.FromHex("FFFFFF");

        var assembly = typeof(Program).Assembly;
        var resourceName = $"{assembly.GetName().Name}.Resources.image.bmp";
        var image = Image.LoadFromResource(resourceName);
        displayScreen.Controls.Add(new Picture(
            left: 99,
            top: 18,
            width: 122,
            height: 129,
            image: image));

        label = new Label(
            left: 0,
            top: 162,
            width: displayScreen.Width,
            height: 16)
        {
            Text = "Hello, World",
            Font = new Font12x16(),
            TextColor = Color.FromHex("1E2834"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        displayScreen.Controls.Add(label);

        var button = new Button(
            left: 92,
            top: 193,
            width: 140,
            height: 28)
        {
            Text = "Click Me",
            Font = new Font12x16(),
            ForeColor = Color.White,
            TextColor = Color.FromHex("1E2834"),
            ShadowColor = Color.FromHex("1E2834"),
            HighlightColor = Color.FromHex("1E2834"),
        };
        button.Clicked += ButtonClicked;
        displayScreen.Controls.Add(button);
    }

    private void ButtonClicked(object sender, EventArgs e)
    {
        count++;
        string message = $"Clicked {count} times!";
        label.Text = message;
        Resolver.Log.Info(message);
    }
}