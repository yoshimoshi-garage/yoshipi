using Meadow;
using Meadow.Foundation.ICs.CAN;
using YoshiPi;

namespace ServoSample;

public class MeadowApp : YoshiPiApp
{
    private Mcp2515 _mcp;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var interrupt = Hardware.MikroBus.Pins.INT.CreateDigitalInterruptPort(Meadow.Hardware.InterruptMode.EdgeFalling);

        interrupt.Changed += OnCANInterrupt;

        _mcp = new Mcp2515(
            Hardware.MikroBus.SpiBus,
            Hardware.MikroBus.Pins.RST.CreateDigitalOutputPort(true),
            CanBitrate.Can_250kbps,
            CanOscillator.Osc_8MHz,
            interrupt,
            Resolver.Log);

        return Task.CompletedTask;
    }

    private void OnCANInterrupt(object? sender, Meadow.Hardware.DigitalPortResult e)
    {
        Resolver.Log.Info("Interrupt");
        if (_mcp.IsFrameAvailable())
        {
            Resolver.Log.Info("Frame available");
            var frame = _mcp.ReadFrame();

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
        else
        {
            Resolver.Log.Info("No data");
        }
    }

    public override async Task Run()
    {
        while (true)
        {
            await Task.Delay(1000);
            Resolver.Log.Info("TX STD");
            _mcp.WriteFrame(new StandardDataFrame
            {
                ID = 0x0F6,
                Payload = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0x55, 0x66, 0x77 }
            }, 0);
            await Task.Delay(1000);
            Resolver.Log.Info("TX EXT");
            _mcp.WriteFrame(new ExtendedDataFrame
            {
                ID = 0x123456,
                Payload = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0x55, 0x66, 0x77 }
            }, 0);
            await Task.Delay(1000);
        }
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
