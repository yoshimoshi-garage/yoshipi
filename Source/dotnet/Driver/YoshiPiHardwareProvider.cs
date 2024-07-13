using Meadow;
using System;

namespace YoshiPi;

/// <summary>
/// Provides hardware for the YoshiPi platform by creating instances of <see cref="IYoshiPiHardware"/>.
/// </summary>
public class YoshiPiHardwareProvider : IMeadowAppEmbeddedHardwareProvider<IYoshiPiHardware>
{
    private YoshiPiHardwareProvider()
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="IYoshiPiHardware"/> for the specified device.
    /// </summary>
    /// <param name="device">The device for which to create the hardware.</param>
    /// <returns>An instance of <see cref="IYoshiPiHardware"/>.</returns>
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
