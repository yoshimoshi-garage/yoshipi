using Meadow;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using YoshiPi;

namespace WS2812Sample;

internal sealed class LedService
{
    private Ws2812 _leds;

    public LedService(ISpiBus spiBus, int ledCount)
    {
        spiBus.Configuration.Speed = new Meadow.Units.Frequency(2.5, Meadow.Units.Frequency.UnitType.Megahertz);
        Resolver.Log.Info($"Bus speed: {spiBus.Configuration.Speed.Megahertz} MHz");
        _leds = new Ws2812(spiBus, ledCount);
    }

    public Task Start()
    {
        return Task.Run(AnimationProc);
    }

    private void AllOff()
    {
        Resolver.Log.Info("AllOff");
        for (var i = 0; i < _leds.NumberOfLeds; i++)
        {
            if (i > 0) _leds.SetLed(i - 1, [0x00, 0x00, 0x00]);
            _leds.SetLed(i, [0x55, 0x00, 0x00]);
            Thread.Sleep(1000);
            _leds.Show();
        }
        Thread.Sleep(1000);
    }

    private void AnimationProc()
    {
        AllOff();
        while (true)
        {
            Resolver.Log.Info("Up");
            for (var i = 0; i < _leds.NumberOfLeds; i++)
            {
                _leds.SetLed(i, Color.Red);
                if (i > 0) _leds.SetLed(i - 1, Color.Black);
                _leds.Show();
                Thread.Sleep(1000);
            }
            Resolver.Log.Info("Down");
            for (var i = _leds.NumberOfLeds - 1; i > 0; i--)
            {
                _leds.SetLed(i, Color.Red);
                _leds.Show();
                Thread.Sleep(1000);
            }
        }
    }
}

public class MeadowApp : YoshiPiApp
{
    private LedService _ledService = default!;

    public override Task Initialize()
    {
        _ledService = new LedService(Hardware.MikroBus.SpiBus, 11);
        return base.Initialize();
    }

    public override async Task Run()
    {
        await _ledService.Start();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
