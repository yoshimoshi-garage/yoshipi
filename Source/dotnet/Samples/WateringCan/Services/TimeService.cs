using Meadow;
using YoshiPi;

namespace YoshiMaker.WateringCan;

public class TimeService
{
    private DateTime? _lastClock;
    private Timer _clockTimer;

    public event EventHandler<DateTime>? MinuteChanged;

    private IYoshiPiHardware Hardware { get; }

    public TimeService(IYoshiPiHardware hardware)
    {
        Hardware = hardware;
        Hardware.ComputeModule.PlatformOS.NtpClient.TimeChanged += NtpClient_TimeChanged;
    }

    private void NtpClient_TimeChanged(DateTime utcTime)
    {
        Resolver.Log.Info($"NTP Set time to: {utcTime.ToString()}");
        Hardware.ComputeModule.PlatformOS.SetClock(Hardware.Rtc.GetTime().DateTime);
    }

    public void Start()
    {
        Hardware.ComputeModule.PlatformOS.NtpClient.Synchronize();

        _clockTimer = new Timer(ClockTickProc, null, 0, -1);
    }

    private void ClockTickProc(object? _)
    {
        var now = DateTime.Now;

        if (_lastClock == null)
        {
            MinuteChanged?.Invoke(this, now);
        }
        else
        {
            if (now.Minute != _lastClock.Value.Minute)
            {
                MinuteChanged?.Invoke(this, now);
            }
        }

        _lastClock = now;

        _clockTimer.Change(TimeSpan.FromSeconds(15), TimeSpan.FromMilliseconds(-1));
    }
}
