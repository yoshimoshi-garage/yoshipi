using Meadow;
using Meadow.Foundation.Sensors.Volume;
using Meadow.Units;
using YoshiPi;

namespace YoshiMaker.WateringCan;

public class MeadowApp : YoshiPiApp
{
    private ResistiveTankLevelSender _levelSensor;
    private DisplayService _display;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _display = new DisplayService(Hardware.Display, Hardware.Touchscreen);

        _levelSensor = new ResistiveTankLevelSender_12in_33_240(Hardware.Adc.Pins.A00, 4.65.Volts());
        _levelSensor.FillLevelChanged += (s, e) => _display.SetWaterLevel(e);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        _levelSensor.StartUpdating();

        await _display.Start();

        //        return base.Run();
    }


    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
