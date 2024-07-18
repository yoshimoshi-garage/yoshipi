using Meadow.Cloud;

namespace YoshiMaker.WateringCan;

public class RunPumpCommand : IMeadowCommand
{
    public int PumpNumber { get; set; }
}
