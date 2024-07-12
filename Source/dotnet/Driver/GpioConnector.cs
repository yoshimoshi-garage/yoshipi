using Meadow.Hardware;

namespace YoshiPi;

/// <summary>
/// Represents a GPIO connector that provides access to GPIO pins through defined pin names.
/// </summary>
public class GpioConnector : Connector<GpioPinDefinitions>
{
    /// <summary>
    /// Contains the names of the GPIO pins.
    /// </summary>
    public static class PinNames
    {
        /// <summary>
        /// Pin D00
        /// </summary>
        public const string D00 = "D00";
        /// <summary>
        /// Pin D01
        /// </summary>
        public const string D01 = "D01";
        /// <summary>
        /// Pin D02
        /// </summary>
        public const string D02 = "D02";
        /// <summary>
        /// Pin D03
        /// </summary>
        public const string D03 = "D03";
        /// <summary>
        /// Pin D04
        /// </summary>
        public const string D04 = "D04";
        /// <summary>
        /// Pin D05
        /// </summary>
        public const string D05 = "D05";
        /// <summary>
        /// Pin D06
        /// </summary>
        public const string D06 = "D06";
        /// <summary>
        /// Pin D07
        /// </summary>
        public const string D07 = "D07";
        /// <summary>
        /// Pin D08
        /// </summary>
        public const string D08 = "D08";
        /// <summary>
        /// Pin D09
        /// </summary>
        public const string D09 = "D09";
    }

    internal GpioConnector(string name, PinMapping map)
        : base(name, new GpioPinDefinitions(map))
    {
    }
}
