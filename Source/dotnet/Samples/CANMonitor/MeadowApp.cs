using Meadow;
using Meadow.Foundation.ICs.CAN;
using Meadow.Hardware;
using YoshiPi;

namespace ServoSample;

/*
public class DisplayController
{
    private DisplayScreen Screen { get; }

    private AbsoluteLayout txLayout;
    private AbsoluteLayout rxLayout;
    private ListBox rxList;

    public DisplayController(IPixelDisplay display)
    {
        var font = new Font8x12();

        Screen = new DisplayScreen(display, RotationType._270Degrees);

        txLayout = new AbsoluteLayout(0, 0, Screen.Width, Screen.Height / 2);
        rxLayout = new AbsoluteLayout(0, txLayout.Bottom, Screen.Width, Screen.Height / 2);

        rxList = new ListBox(0, 0, rxLayout.Width, rxLayout.Height, font);

        Screen.Controls.Add(txLayout, rxLayout);
    }

    public void DisplayReceivedFrame(DataFrame frame)
    {
        var existing = rxList.Items.FirstOrDefault(i => (i as FrameInfo)!.ID == frame.ID) as FrameInfo;

        if (existing == null)
        {
            rxList.Items.Add(new FrameInfo
            {
                ID = frame.ID,
                Count = 1,
                LastData = frame.Payload
            });
        }
        else
        {
            existing.ID = frame.ID;
            existing.Count++;
            existing.LastData = frame.Payload;

            rxList.Invalidate();
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

public class MainController
{
    private ICanBus CanBus { get; }
    private DisplayController Display { get; }

    public MainController(IPixelDisplay display, ICanBus bus)
    {
        CanBus = bus;
        Display = new DisplayController(display);
    }

    public async Task Run()
    {
        CanBus.FrameReceived += OnFrameReceived;

        while (true)
        {
            await Task.Delay(1000);
            Resolver.Log.Info("TX STD");
            CanBus.WriteFrame(new StandardDataFrame
            {
                ID = 0x0F6,
                Payload = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0x55, 0x66, 0x77 }
            });
            await Task.Delay(1000);
            Resolver.Log.Info("TX EXT");
            CanBus.WriteFrame(new ExtendedDataFrame
            {
                ID = 0x123456,
                Payload = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0x55, 0x66, 0x77 }
            });
            await Task.Delay(1000);
        }
    }

    private void OnFrameReceived(object? sender, ICanFrame frame)
    {
        if (frame is DataFrame df)
        {
            if (df is RemoteTransferRequestFrame rtr)
            {
                Resolver.Log.Info($"RTR: 0x{df.ID:X2}  {BitConverter.ToString(rtr.Payload)}");
            }
            else if (df is StandardDataFrame sdf)
            {
                Resolver.Log.Info($"SDF: 0x{df.ID:X2}  {BitConverter.ToString(sdf.Payload)}");
            }
            else if (df is ExtendedDataFrame edf)
            {
                Resolver.Log.Info($"EDF: 0x{df.ID:X2}  {BitConverter.ToString(edf.Payload)}");
            }
        }
    }
}
*/
public class MeadowApp : YoshiPiApp
{
    private MainController controller;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var interrupt = Hardware.MikroBus.Pins.INT.CreateDigitalInterruptPort(InterruptMode.EdgeFalling);

        var mcp = new Mcp2515(
            Hardware.MikroBus.SpiBus,
            Hardware.MikroBus.Pins.RST.CreateDigitalOutputPort(true),
            Mcp2515.CanOscillator.Osc_8MHz,
            interrupt,
            Resolver.Log);

        var bus = mcp.CreateCanBus(CanBitrate.Can_125kbps);

        controller = new MainController(Hardware.Display, Hardware.Touchscreen, bus);

        return Task.CompletedTask;
    }


    public override Task Run()
    {
        return controller.Run();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
