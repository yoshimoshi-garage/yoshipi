using Meadow;
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


    private void OnFrameReceived(object? sender, ICanFrame frame)
    {
        if (frame is DataFrame df)
        {
            if (df is RemoteTransferRequestFrame rtr)
            {
                Resolver.Log.Info($"RTR: 0x{df.ID:X2}  {BitConverter.ToString(rtr.Payload)}");

                // create a response frame
                var response = new StandardDataFrame
                {
                    ID = rtr.ID,
                    Payload = new byte[] { 0x00, 0x00, 0xde, 0xad, 0xbe, 0xef, 0x00, 0x00 }
                };
                CanBus.WriteFrame(response);
            }
            else if (df is StandardDataFrame sdf)
            {
                Resolver.Log.Info($"SDF: 0x{df.ID:X2}  {BitConverter.ToString(sdf.Payload)}");
            }
            else if (df is ExtendedDataFrame edf)
            {
                Resolver.Log.Info($"EDF: 0x{df.ID:X2}  {BitConverter.ToString(edf.Payload)}");
            }

            Display.DisplayReceivedFrame(df);
        }
    }
}
