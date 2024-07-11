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
    private readonly IPin? _d04;
    private readonly IPin? _d05;
    private readonly IPin? _d06;
    private readonly IPin? _d07;
    private readonly IPin? _d08;
    private readonly IPin? _d09;

    public IPin D00 => _d00 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D01 => _d01 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D02 => _d02 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D03 => _d03 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D04 => _d04 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D05 => _d05 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D06 => _d06 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D07 => _d07 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D08 => _d08 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin D09 => _d09 ?? throw new PlatformNotSupportedException("Pin not connected");

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
                case GpioConnector.PinNames.D04:
                    _d04 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D05:
                    _d05 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D06:
                    _d06 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D07:
                    _d07 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D08:
                    _d08 = m.ConnectsTo;
                    break;
                case GpioConnector.PinNames.D09:
                    _d09 = m.ConnectsTo;
                    break;
            }
        }
    }
}
