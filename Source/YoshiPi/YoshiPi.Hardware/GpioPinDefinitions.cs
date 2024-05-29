using Meadow;
using Meadow.Hardware;
using System;

namespace YoshiPi;

public class GpioPinDefinitions : PinDefinitionBase
{
    private readonly IPin? _d00;
    private readonly IPin? _d01;
    private readonly IPin? _d02;
    private readonly IPin? _d03;

    public IPin D00 => _d00 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D01 => _d01 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D02 => _d02 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D03 => _d03 ?? throw new PlatformNotSupportedException("Pin not connected");

    internal GpioPinDefinitions(
                PinMapping mapping
                )
    {
        foreach (var m in mapping)
        {
            switch (m.PinName)
            {
                case GpioConnector.PinNames.D00:
                    _d00 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D01:
                    _d01 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D02:
                    _d02 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D03:
                    _d03 = m.ConnectsTo;
                    break;
            }
        }
    }
}
