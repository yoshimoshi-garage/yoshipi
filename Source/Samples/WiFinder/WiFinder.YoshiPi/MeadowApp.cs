using Meadow;
using Meadow.Logging;
using System.Threading.Tasks;
using WiFinder.Core;
using YoshiPi;

namespace WiFinder;

internal class MeadowApp : YoshiPiApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        // output log messages to the VS debug window
        Resolver.Log.AddProvider(new DebugLogProvider());

        mainController = new MainController();
        return mainController.Initialize(new YoshiPiHardware(Hardware));
    }

    public override async Task Run()
    {
        await mainController.Run();
    }
}