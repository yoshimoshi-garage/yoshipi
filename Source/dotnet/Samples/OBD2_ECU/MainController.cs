using Meadow.Foundation.Telematics.OBD2;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace OBD2Ecu;

public class MainController
{
    private Ecu Ecu { get; }
    private DisplayController Display { get; }

    public MainController(IColorInvertableDisplay display, ITouchScreen touchscreen, ICanBus bus)
    {
        Ecu = new Ecu(bus);
        Ecu.PidRequestHandlers.Add(Pid.EngineCoolantTemperature, GetCoolantTemp);

        Display = new DisplayController(display, touchscreen);
    }

    private byte[] GetCoolantTemp(ushort pid)
    {
        return new byte[2] { 0x01, 0x02 };
    }

    public async Task Run()
    {
        await Display.Start();
    }
}
