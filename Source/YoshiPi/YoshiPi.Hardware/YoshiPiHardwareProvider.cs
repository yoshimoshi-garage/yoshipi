using Meadow;
using System;

namespace YoshiPi;

public class YoshiPiHardwareProvider : IMeadowAppEmbeddedHardwareProvider<IYoshiPiHardware>
{
    private YoshiPiHardwareProvider()
    {
    }

    public IYoshiPiHardware Create(IMeadowDevice device)
    {
        if (device is RaspberryPi pi)
        {
            return new YoshiPi_v1a(pi);
        }

        // this method is called my MeadowOS, so we should never get here
        throw new Exception("Invalid IMeadowDevice provided");
    }
}
