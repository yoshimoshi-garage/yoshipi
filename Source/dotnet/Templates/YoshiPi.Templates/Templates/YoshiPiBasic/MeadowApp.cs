using Meadow;
using System.Threading.Tasks;
using YoshiPi;

namespace YoshiPiApplication.Template;

public class MeadowApp : YoshiPiApp
{
    private DisplayController? displayController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Hardware.Display.InvertDisplayColor(true);

        displayController = new DisplayController(Hardware.Display, Hardware.Touchscreen);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await displayController?.Start();
    }
}