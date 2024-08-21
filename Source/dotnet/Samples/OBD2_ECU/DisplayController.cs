using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System.Net;
using static Meadow.Resolver;

namespace OBD2Ecu;

public class DisplayController
{
    private DisplayScreen Screen { get; }

    public DisplayController(IColorInvertableDisplay display, ITouchScreen? touchscreen)
    {
        display.InvertDisplayColor(true);

        Screen = new DisplayScreen(display, RotationType._270Degrees, touchScreen: touchscreen)
        {
            BackgroundColor = Color.DarkGray
        };
    }

    public async Task Start()
    {
        await CheckTouchscreenCalibration();
        CreateLayouts();
    }

    private byte[] PayloadLongToBytes(long data)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
    }

    private void CreateLayouts()
    {
        var titleLabel = new Label(0, 0, Screen.Width, 30)
        {
            Text = $"YoshiPi ECU Simulator",
            TextColor = Color.White,
            Font = new Font8x12(),
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        Screen.Controls.Add(titleLabel);
    }

    private async Task CheckTouchscreenCalibration()
    {
        if (Screen.TouchScreen == null) return;
        var calibratableTouchscreen = Screen.TouchScreen as ICalibratableTouchscreen;
        if (calibratableTouchscreen == null) return;

        var calfile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ts.cal"));

        Log.Info($"Using calibration data at {calfile.FullName}");

        var cal = new TouchscreenCalibrationService(Screen, calfile);

        var existing = cal.GetSavedCalibrationData();

        if (existing != null)
        {
            calibratableTouchscreen.SetCalibrationData(existing);
        }
        else
        {
            await cal.Calibrate(true);
        }
    }

}
