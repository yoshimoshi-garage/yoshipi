﻿using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using static Meadow.Resolver;

namespace YoshiMaker.WateringCan;

public class DisplayService
{
    public event EventHandler? CallForPumping;

    private IPixelDisplay _display;
    private DisplayScreen _screen;
    private Box _emptyArea;
    private Box _waterLevelBox;
    private Button _waterNowButton;
    private Label _lastWaterTime;
    private Label _lastWaterSource;
    private Label _currentTimeLabel;
    private ICalibratableTouchscreen _touchscreen;

    public DisplayService(IPixelDisplay display, ICalibratableTouchscreen touchscreen)
    {
        _display = display;
        (display as Ili9341)?.InvertDisplayColor(true);

        _touchscreen = touchscreen;
        _screen = new DisplayScreen(_display, RotationType._270Degrees, touchscreen);
    }

    public async Task Start()
    {
        await CheckTouchscreenCalibration();
        CreateLayouts();
    }

    private async Task CheckTouchscreenCalibration()
    {
        var calfile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ts.cal"));

        Log.Info($"Using calibration data at {calfile.FullName}");

        var cal = new TouchscreenCalibrationService(_screen, calfile);

        var existing = cal.GetSavedCalibrationData();

        if (existing != null)
        {
            _touchscreen.SetCalibrationData(existing);
        }
        else
        {
            await cal.Calibrate(true);
        }
    }

    private void CreateLayouts()
    {

        var homeLayout = new AbsoluteLayout(_screen.Width, _screen.Height)
        {
            BackgroundColor = Color.DarkGray
        };

        var font = new Font12x20();

        var titleLabel = new Label(110, 10, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "Watering Can",
            Font = font,
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

        var actionArealeft = 125;

        _waterNowButton = new Button(actionArealeft + 15, 50, 150, 50)
        {
            ForeColor = Color.Blue,
            PressedColor = Color.DarkBlue,
            HighlightColor = Color.LightBlue,
            TextColor = Color.WhiteSmoke,
            Text = "WATER NOW",
            Font = font
        };

        _waterNowButton.Clicked += OnWaterNowClicked;

        var lastWaterTitle = new Label(110, 120, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "Last watered:",
            Font = font,
            HorizontalAlignment = HorizontalAlignment.Center,
        };


        _lastWaterTime = new Label(110, 140, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "<unknown>",
            Font = font,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        _lastWaterSource = new Label(110, 160, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "",
            Font = font,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        _currentTimeLabel = new Label(110, _screen.Height - 30, _screen.Width - 110, 30)
        {
            TextColor = Color.DarkBlue,
            Text = "",
            Font = font,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        homeLayout.Controls.Add(
            titleLabel,
            outline,
            _emptyArea,
            _waterLevelBox,
            _waterNowButton,
            lastWaterTitle,
            _lastWaterTime,
            _lastWaterSource,
            _currentTimeLabel
            );

        _screen.Controls.Add(homeLayout);
    }

    public void ShowTime(DateTime time)
    {
        _currentTimeLabel.Text = $"{time:t}";
    }

    private void OnWaterNowClicked(object? sender, EventArgs e)
    {
        Log.Info("WATER NOW");
        CallForPumping?.Invoke(this, EventArgs.Empty);

        SetLastWater(DateTime.Now, "manual");
    }

    public void SetLastWater(DateTime time, string source)
    {
        _lastWaterTime.Text = $"{time:MM/dd HH:mm}";
        _lastWaterSource.Text = source;
    }

    public void SetWaterLevel(int percent)
    {
        Log.Info($"WATER LEVEL {percent}% ({_waterLevelBox == null})");

        if (_waterLevelBox == null) return;

        var height = 216 * percent / 100;
        _waterLevelBox.Height = height;
        _waterLevelBox.Top = _emptyArea.Top + (_emptyArea.Height - height);
    }
}
