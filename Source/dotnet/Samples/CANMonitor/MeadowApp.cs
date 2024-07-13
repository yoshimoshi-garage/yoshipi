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

        _mcp = new Mcp2515(
            Hardware.MikroBus.SpiBus,
            Hardware.MikroBus.Pins.RST.CreateDigitalOutputPort(true),
            CanBitrate.Can_250kbps,
            CanOscillator.Osc_8MHz,
            Resolver.Log);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        while (true)
        {

            if (_mcp.IsFrameAvailable())
            {
                Resolver.Log.Info("Frame available");
                var frame = _mcp.ReadFrame();

                if (frame != null)
                {


                    Resolver.Log.Info($"ID: 0x{frame.Value.ID:X2} contains {BitConverter.ToString(frame.Value.Payload)} bytes");
                }
            }
            else
            {
                Resolver.Log.Info("No data");
            }

            await Task.Delay(1000);
            Resolver.Log.Info("TX");
            _mcp.WriteFrame(new Mcp2515.Frame
            {
                ID = 0x0CF103000,
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
