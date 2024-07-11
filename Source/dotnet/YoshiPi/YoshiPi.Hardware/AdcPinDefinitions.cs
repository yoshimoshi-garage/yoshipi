using Meadow;
using Meadow.Hardware;
using System;

namespace YoshiPi;

public class AdcPinDefinitions : PinDefinitionBase
{
    private readonly IPin? _a00;
    private readonly IPin? _a01;
    private readonly IPin? _a02;
    private readonly IPin? _a03;

    public IPin A00 => _a00 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin A01 => _a01 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin A02 => _a02 ?? throw new PlatformNotSupportedException("Pin not connected");
    public IPin A03 => _a03 ?? throw new PlatformNotSupportedException("Pin not connected");

    internal AdcPinDefinitions(
                PinMapping mapping
                )
    {
        foreach (var m in mapping)
        {
            switch (m.PinName)
            {
                case AdcConnector.PinNames.A00:
                    _a00 = m.ConnectsTo;
                    break;
                case AdcConnector.PinNames.A01:
                    _a01 = m.ConnectsTo;
                    break;
                case AdcConnector.PinNames.A02:
                    _a02 = m.ConnectsTo;
                    break;
                case AdcConnector.PinNames.A03:
                    _a03 = m.ConnectsTo;
                    break;
            }
        }
    }
}
