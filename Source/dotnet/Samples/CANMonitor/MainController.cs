using Meadow;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace ServoSample;

public class MainController
{
    private ICanBus CanBus { get; }
    private DisplayController Display { get; }

    public MainController(IPixelDisplay display, ITouchScreen touchscreen, ICanBus bus)
    {
        CanBus = bus;

        Display = new DisplayController(display, touchscreen);
        Display.SendFrameRequested += OnSendFrameRequested;
        Display.FilterEnabledChanged += OnFilterEnabledChanged;
    }

    private void OnFilterEnabledChanged(object? sender, bool e)
    {
        if (e)
        {
            CanBus.AcceptanceFilters.Add(CanAcceptanceFilter.CreateStandardFilter(0x7ff));
        }
        else
        {
            CanBus.AcceptanceFilters.Clear();
        }
    }

    public async Task Run()
    {
        CanBus.FrameReceived += OnFrameReceived;

        await Display.Start();
    }

    private void OnSendFrameRequested(object? sender, (int ID, byte[] Data) e)
    {
        DataFrame frame;

        if (e.ID <= 0x7ff)
        {
            frame = new StandardDataFrame
            {
                ID = e.ID,
                Payload = e.Data
            };
        }
        else
        {
            frame = new ExtendedDataFrame
            {
                ID = e.ID,
                Payload = e.Data
            };
        }

        CanBus.WriteFrame(frame);
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
