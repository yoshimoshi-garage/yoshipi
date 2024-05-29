using Meadow.Hardware;

namespace YoshiPi;

public class GpioConnector : Connector<GpioPinDefinitions>
{
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
    }

    internal GpioConnector(string name, PinMapping map)
        : base(name, new GpioPinDefinitions(map))
    {
    }
}
