using Meadow;
using Meadow.Foundation.Sensors.Volume;
using Meadow.Units;
using YoshiPi;

namespace YoshiMaker.WateringCan;

public class MeadowApp : YoshiPiApp
{
    private ResistiveTankLevelSender _levelSensor;
    private DisplayService _display;
    private PumpService _pumpService;
    private CloudService _cloudService;
    private ScheduleService _scheduleService;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Hardware.ComputeModule.PlatformOS.SetClock(Hardware.Rtc.GetTime().DateTime);

        _pumpService = new PumpService(Hardware.Relay1, null);

        _display = new DisplayService(Hardware.Display, Hardware.Touchscreen);
        _display.CallForPumping += (s, e) => RunPump("manual");

        _levelSensor = new ResistiveTankLevelSender_12in_33_240(Hardware.Adc.Pins.A00, 4.65.Volts());
        _levelSensor.FillLevelChanged += (s, e) => _display.SetWaterLevel(e);

        _cloudService = new CloudService();
        _cloudService.RunPumpRequested += (s, e) => RunPump("remote", e);

        _scheduleService = new ScheduleService();
        _scheduleService.RunPumpRequested += (s, e) => RunPump("scheduled", e);

        return Task.CompletedTask;
    }

    private void RunPump(string source, int pumpNumber = -1)
    {
        _display.SetLastWater(DateTime.Now, source);
        _ = _pumpService.RunAllPumps(TimePeriod.FromSeconds(5));
    }

    public override async Task Run()
    {
        await _display.Start();

        Resolver.Log.Info("Starting level sensor");
        _levelSensor.StartUpdating();

        //        return base.Run();
    }


    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
