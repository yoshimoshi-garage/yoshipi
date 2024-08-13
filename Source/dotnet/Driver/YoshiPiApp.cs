using Meadow;
using Meadow.Foundation.Displays;
using System.Threading.Tasks;

namespace YoshiPi;

/// <summary>
/// Represents the base application class for YoshiPi devices, inheriting from the generic App class.
/// </summary>
public abstract class YoshiPiApp : App<RaspberryPi, YoshiPiHardwareProvider, IYoshiPiHardware>
{
    public virtual bool InvertDisplayColors => true;

    public override Task Initialize()
    {
        if (InvertDisplayColors)
        {
            (Hardware.Display as Ili9341)?.InvertDisplayColor(true);
        }

        return base.Initialize();
    }
}
