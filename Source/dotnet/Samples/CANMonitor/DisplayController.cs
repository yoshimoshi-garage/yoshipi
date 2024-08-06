using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System.Net;
using static Meadow.Resolver;

namespace ServoSample;

public class DisplayController
{
    private DisplayScreen Screen { get; }

    private AbsoluteLayout txLayout;
    private AbsoluteLayout rxLayout;

    private Label[] rxlabels = new Label[5];

    private int tx1ID = 0x7ff;
    private int tx2ID = 0x12345ab;
    private long tx1FrameData = 0x1122334455667788;
    private long tx2FrameData = 0x7766554433221100;

    public event EventHandler<(int ID, byte[] Data)>? SendFrameRequested;

    public DisplayController(IPixelDisplay display, ITouchScreen? touchscreen)
    {
        (display as Ili9341)!.InvertDisplayColor(true);

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
        var font = new Font8x12();

        txLayout = new AbsoluteLayout(0, 5, Screen.Width, Screen.Height / 2)
        {
            BackgroundColor = Color.DarkGray
        };

        var tx1IDLabel = new Label(5, 2, 70, 30)
        {
            Text = $"{tx1ID:x}h",
            BackColor = Color.GhostWhite,
            TextColor = Color.Black,
            Font = font,
        };
        var tx1DataLabel = new Label(tx1IDLabel.Right + 2, tx1IDLabel.Top, 190, 30)
        {
            Text = BitConverter.ToString(PayloadLongToBytes(tx1FrameData)),
            BackColor = Color.GhostWhite,
            TextColor = Color.Black,
            Font = font,
        };
        var tx1SendButton = new Button(txLayout.Width - 55, tx1DataLabel.Top, 50, 30)
        {
            Text = "send",
            Font = font,
        };
        tx1SendButton.Clicked += (s, e) =>
        {
            var bytes = PayloadLongToBytes(tx1FrameData);
            SendFrameRequested?.Invoke(this, (tx1ID, bytes));

            tx1FrameData++;
            bytes = PayloadLongToBytes(tx1FrameData);
            tx1DataLabel.Text = BitConverter.ToString(bytes);
        };

        var tx2IDLabel = new Label(5, tx1IDLabel.Bottom + 5, 70, 30)
        {
            Text = $"{tx2ID:x}h",
            BackColor = Color.GhostWhite,
            TextColor = Color.Black,
            Font = font,
        };
        var tx2DataLabel = new Label(tx2IDLabel.Right + 2, tx2IDLabel.Top, 190, 30)
        {
            Text = BitConverter.ToString(PayloadLongToBytes(tx2FrameData)),
            BackColor = Color.GhostWhite,
            TextColor = Color.Black,
            Font = font,
        };
        var tx2SendButton = new Button(txLayout.Width - 55, tx2DataLabel.Top, 50, 30)
        {
            Text = "send",
            Font = font,
        };
        tx2SendButton.Clicked += (s, e) =>
        {
            var bytes = PayloadLongToBytes(tx2FrameData);
            SendFrameRequested?.Invoke(this, (tx2ID, bytes));
            tx2FrameData--;
            bytes = PayloadLongToBytes(tx2FrameData);
            tx2DataLabel.Text = BitConverter.ToString(bytes);
        };

        txLayout.Controls.Add(tx1IDLabel, tx1DataLabel, tx1SendButton, tx2IDLabel, tx2DataLabel, tx2SendButton);


        rxLayout = new AbsoluteLayout(0, Screen.Height / 2, Screen.Width, Screen.Height / 2)
        {
        };

        var rowheight = 16;

        var rxTitle = new Label(5, 2, rxLayout.Width - 10, rowheight)
        {
            TextColor = Color.Black,
            Font = font,
            BackColor = Color.BlanchedAlmond,
            Text = "Receive"
        };

        rxLayout.Controls.Add(rxTitle);

        var y = 20;

        for (var i = 0; i < rxlabels.Length; i++)
        {
            rxlabels[i] = new Label(5, y, rxLayout.Width - 10, rowheight)
            {
                TextColor = Color.LightGreen,
                Font = font,
                BackColor = Color.Black
            };

            y += rowheight;

            rxLayout.Controls.Add(rxlabels[i]);
        }

        Screen.Controls.Add(txLayout, rxLayout);
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

    public void DisplayReceivedFrame(DataFrame frame)
    {
        var existing = rxlabels.FirstOrDefault(i => (i.Context as FrameInfo)?.ID == frame.ID);

        if (existing == null)
        {
            var info = new FrameInfo
            {
                ID = frame.ID,
                Count = 1,
                LastData = frame.Payload,
            };

            var label = rxlabels.First(l => l.Context == null);
            label.Context = info;
            label.Text = info.ToString();
        }
        else
        {
            var info = existing.Context as FrameInfo;

            info.ID = frame.ID;
            info.Count++;
            info.LastData = frame.Payload;

            existing.Text = info.ToString();
        }
    }

    private class FrameInfo
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public byte[] LastData { get; set; }

        public override string ToString()
        {
            return $"{ID:x}h  {BitConverter.ToString(LastData)}  {Count}";
        }
    }
}
