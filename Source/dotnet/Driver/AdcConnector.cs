using Meadow.Hardware;

namespace YoshiPi;

/// <summary>
/// Represents a ADC connector that provides access to ADC pins through defined pin names.
/// </summary>
public class AdcConnector : Connector<AdcPinDefinitions>
{
    /// <summary>
    /// Contains the names of the ADC pins.
    /// </summary>
    public static class PinNames
    {
        /// <summary>
        /// Pin A00
        /// </summary>
        public const string A00 = "A00";
        /// <summary>
        /// Pin A01
        /// </summary>
        public const string A01 = "A01";
        /// <summary>
        /// Pin A02
        /// </summary>
        public const string A02 = "A02";
        /// <summary>
        /// Pin A03
        /// </summary>
        public const string A03 = "A03";
    }

    internal AdcConnector(string name, PinMapping map)
        : base(name, new AdcPinDefinitions(map))
    {
    }
}
