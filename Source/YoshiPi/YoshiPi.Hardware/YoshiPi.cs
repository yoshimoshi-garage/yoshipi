using Meadow;

namespace YoshiPi;

public class YoshiPiHardware
{
    public static IYoshiPiHardware Create(RaspberryPi device)
    {
        return new YoshiPi_v1a(device);
    }
}
