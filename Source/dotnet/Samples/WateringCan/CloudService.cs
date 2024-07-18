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

    public void PublishTankLevel()
    {
    }
}
