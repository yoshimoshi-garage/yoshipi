using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using System.Threading.Tasks;

namespace Validation;

public class MeadowApp : App<RaspberryPi>
{
    private IYoshiPiHardware _hardware;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _hardware = YoshiPi.Create(Device);

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            _hardware.Relay1.Toggle();
            Resolver.Log.Info($"Relay 1: {_hardware.Relay1.State}");
            await Task.Delay(1000);
        }
    }
}

public interface IYoshiPiHardware
{
    IRelay Relay1 { get; }
}

public class YoshiPi
{
    public static IYoshiPiHardware Create(RaspberryPi device)
    {
        return new YoshiPi_v1a(device);
    }
}

public class YoshiPi_v1a : IYoshiPiHardware
{
    private RaspberryPi _device;
    private IRelay? _relay1;

    public IRelay Relay1 => _relay1 ??= CreateRelay(0);

    internal YoshiPi_v1a(RaspberryPi device)
    {
        _device = device;
    }

    private IRelay CreateRelay(int index)
    {
        return new Relay(_device.Pins.GPIO16.CreateDigitalOutputPort(false));
    }
}