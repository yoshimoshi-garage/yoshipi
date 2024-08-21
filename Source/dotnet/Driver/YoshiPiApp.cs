using Meadow;

namespace YoshiPi;

/// <summary>
/// Represents the base application class for YoshiPi devices, inheriting from the generic App class.
/// </summary>
public abstract class YoshiPiApp : App<RaspberryPi, YoshiPiHardwareProvider, IYoshiPiHardware>
{

}