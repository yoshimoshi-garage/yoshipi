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
            return new YoshiPi_v1b(pi);
        }

        // this method is called by MeadowOS, so we should never get here
        throw new Exception("Invalid IMeadowDevice provided");
    }
}
