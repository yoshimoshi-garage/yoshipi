using Meadow;
using Meadow.Foundation.ICs.CAN;
using Meadow.Hardware;
using YoshiPi;

namespace OBD2Ecu;

public class MeadowApp : YoshiPiApp
{
    private MainController controller;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var interrupt = Hardware.MikroBus.Pins.INT.CreateDigitalInterruptPort(InterruptMode.EdgeFalling);

        var rst = Hardware.MikroBus.Pins.RST.CreateDigitalOutputPort(true);
        var cs = Hardware.MikroBus.Pins.CS.CreateDigitalOutputPort(true);

        var mcp = new Mcp2515(
            Hardware.MikroBus.SpiBus,
            cs,
            Mcp2515.CanOscillator.Osc_8MHz,
            interrupt,
            Resolver.Log);

        var bus = mcp.CreateCanBus(CanBitrate.Can_250kbps);
        bus.BusError += OnBusError; ;

        controller = new MainController(Hardware.Display, Hardware.Touchscreen, bus);

        // return Task.CompletedTask;
    }

    private void OnBusError(object? sender, CanErrorInfo e)
    {
        Resolver.Log.Error($"Bus error. Tx: {e.TransmitErrorCount}, Rx: {e.ReceiveErrorCount}");
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
