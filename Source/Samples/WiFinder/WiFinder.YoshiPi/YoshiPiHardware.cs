using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WiFinder.Core;
using WiFinder.Core.Contracts;
using YoshiPi;

namespace WiFinder;

internal class YoshiPiHardware : IWiFinderHardware
{
    private readonly IYoshiPiHardware device;

    public RotationType DisplayRotation => RotationType.Default;
    public INetworkController NetworkController { get; }
    public IPixelDisplay Display => device.Display;
    public IButton? UpButton => null;
    public IButton? DownButton => null;
    public IButton? LeftButton => null;
    public IButton? RightButton => null;

    public YoshiPiHardware(IYoshiPiHardware device)
    {
        this.device = device;

        var wifi = device.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        NetworkController = new NetworkController(wifi!);
    }
}
