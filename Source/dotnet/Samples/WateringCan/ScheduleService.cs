namespace YoshiMaker.WateringCan;

public class ScheduleService
{
    public event EventHandler<int>? RunPumpRequested;
    public event EventHandler? ReportTankLevel;

    private DateTime? _lastRun = null;
    private readonly Timer _scheduleTimer;

    public ScheduleService()
    {
        // TODO: get persisted last run

        var now = DateTime.Now;
        var nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
        var nextCheck = (nextHour - now);

        _scheduleTimer = new Timer(ScheduleTimerProc);
    }

    public void Run()
    {
        // schedule the check on the next hour
        ScheduleNextCheck();
    }

    private void ScheduleNextCheck()
    {
        TimeSpan nextCheck;

        do
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var now = DateTime.Now;
            var nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
            nextCheck = (nextHour - now);
        } while (nextCheck.TotalSeconds < 60);

        // schedule the check on the next hour
        _scheduleTimer.Change(nextCheck, TimeSpan.FromMilliseconds(-1));

        // report tank level on every check (so at startup and at every hour)
        ReportTankLevel?.Invoke(this, EventArgs.Empty);
    }

    private void ScheduleTimerProc(object? o)
    {
        var now = DateTime.Now;

        // TODO: allow multiple runs/days
        if (now.DayOfWeek == DayOfWeek.Sunday && now.Hour == 13)
        {
            RunPumpRequested?.Invoke(this, -1);
            _lastRun = now;

            // TODO: persist last run
        }

        ScheduleNextCheck();
    }
}
