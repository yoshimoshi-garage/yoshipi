using Meadow.Peripherals.Relays;
using Meadow.Units;

namespace YoshiMaker.WateringCan;

public class PumpService
{
    private IRelay _pump1;
    private IRelay? _pump2;

    public PumpService(IRelay pump1, IRelay? pump2)
    {
        _pump1 = pump1;
        _pump2 = pump2;

        _pump1.State = RelayState.Open;
        if (_pump2 != null)
        {
            _pump1.State = RelayState.Open;
        }
    }

    public async Task RunAllPumps(TimePeriod time)
    {
        _pump1.State = RelayState.Closed;
        if (_pump2 != null)
        {
            _pump1.State = RelayState.Closed;
        }

        await Task.Delay((TimeSpan)time);

        _pump1.State = RelayState.Open;
        if (_pump2 != null)
        {
            _pump1.State = RelayState.Open;
        }
    }
}
