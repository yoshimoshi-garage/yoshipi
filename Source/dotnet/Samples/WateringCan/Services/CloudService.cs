using Meadow.Units;
using static Meadow.Resolver;

namespace YoshiMaker.WateringCan;

public class CloudService
{
    private int _lastRunPumpCommandTime = int.MinValue;

    public event EventHandler<int>? RunPumpRequested;

    public CloudService()
    {
        try
        {
            CommandService.Subscribe<RunPumpCommand>(OnRunPumpCommandReceived);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
        }
    }

    private void OnRunPumpCommandReceived(RunPumpCommand command)
    {
        Log.Trace($"Received RunPump Cloud command");

        var now = Environment.TickCount;

        // rate limit the call
        if (Math.Abs(now - _lastRunPumpCommandTime) < 10000)
        {
            Log.Warn("RunPump command rate limit exceeded.  Ignoring request");
            return;
        }

        RunPumpRequested?.Invoke(this, command.PumpNumber);
    }

    public void PublishTankLevel(int percent)
    {
        Log.Trace($"Sending tank level...");

        try
        {
            MeadowCloudService.SendEvent(201, "WateringCan Data", new Dictionary<string, object>
            {
                {"Tank Level", percent }
            });
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to send tank level to cloud: {ex.Message}");
        }
    }

    public void LogPumpRun(int pumpNumber, TimePeriod runTime, string trigger)
    {
        Log.Trace($"Logging pump run...");

        try
        {
            MeadowCloudService.SendLog(Meadow.Logging.LogLevel.Information, $"{trigger} caused pump {pumpNumber} to run for {runTime.Seconds} s");
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to send startup log to cloud: {ex.Message}");
        }
    }

    public void LogStartupEvent()
    {
        Log.Trace($"Sending startup log...");

        try
        {
            MeadowCloudService.SendLog(Meadow.Logging.LogLevel.Information, "WateringCan Startup");
        }
        catch (Exception ex)
        {
            Log.Error($"Unable to send startup log to cloud: {ex.Message}");
        }
    }
}
