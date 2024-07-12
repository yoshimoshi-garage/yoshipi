using Meadow;
using Meadow.Hardware;
using System;

namespace YoshiPi;

/// <summary>
/// Represents the GPIO pin definitions for a device, providing access to each pin and
/// throwing a <see cref="PlatformNotSupportedException"/> if a pin is not connected.
/// </summary>
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

    /// <summary>
    /// Gets the pin D00
    /// </summary>
    public IPin D00 => _d00 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D01
    /// </summary>
    public IPin D01 => _d01 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D02
    /// </summary>
    public IPin D02 => _d02 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D03
    /// </summary>
    public IPin D03 => _d03 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D04
    /// </summary>
    public IPin D04 => _d04 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D05
    /// </summary>
    public IPin D05 => _d05 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D06
    /// </summary>
    public IPin D06 => _d06 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D07
    /// </summary>
    public IPin D07 => _d07 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D08
    /// </summary>
    public IPin D08 => _d08 ?? throw new PlatformNotSupportedException("Pin not connected");
    /// <summary>
    /// Gets the pin D09
    /// </summary>
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
