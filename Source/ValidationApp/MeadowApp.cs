using Meadow;
using Meadow.Hardware;
using System.Threading.Tasks;
using YoshiPi;

namespace Validation;

public class MeadowApp : App<RaspberryPi>
{
    private IYoshiPiHardware _hardware;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _hardware = YoshiPiHardware.Create(Device);

        TestButtons();

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");
    }

    private async Task TestRelays()
    {
        while (true)
        {
            _hardware.Relay1.Toggle();
            Resolver.Log.Info($"Relay 1: {_hardware.Relay1.State}");
            _hardware.Relay2.Toggle();
            Resolver.Log.Info($"Relay 2: {_hardware.Relay2.State}");

            await Task.Delay(1000);
        }
    }

    private void TestButtons()
    {
        _hardware.Button1.Clicked += (s, e) => { Resolver.Log.Info("Button1 clicked"); };
        _hardware.Button2.Clicked += (s, e) => { Resolver.Log.Info("Button2 clicked"); };
    }

    private async Task TestGpioOutputs()
    {
        //        var d0 = _hardware.Gpio.Pins.D00.CreateDigitalOutputPort(false);
        var d1 = _hardware.Gpio.Pins.D01.CreateDigitalOutputPort(false);
        var d2 = _hardware.Gpio.Pins.D02.CreateDigitalOutputPort(false);
        var d3 = _hardware.Gpio.Pins.D03.CreateDigitalOutputPort(false);

        while (true)
        {

            //            d0.State = !d0.State;
            d1.State = !d1.State;
            d2.State = !d2.State;
            d3.State = !d3.State;

            await Task.Delay(1000);
        }
    }

    private async Task TestGpioInputs()
    {
        var resistor = ResistorMode.InternalPullUp;

        //var d0 = _hardware.Gpio.Pins.D00.CreateDigitalInputPort(Meadow.Hardware.ResistorMode.Disabled);
        var d1 = _hardware.Gpio.Pins.D01.CreateDigitalInputPort(resistor);
        var d2 = _hardware.Gpio.Pins.D02.CreateDigitalInputPort(resistor);
        var d3 = _hardware.Gpio.Pins.D03.CreateDigitalInputPort(resistor);

        while (true)
        {
            //Resolver.Log.Info($"D0: {d0.State}");
            Resolver.Log.Info($"D1: {d1.State}");
            Resolver.Log.Info($"D2: {d2.State}");
            Resolver.Log.Info($"D3: {d3.State}");

            await Task.Delay(1000);
        }
    }

    private async Task TestGpioInterrupts()
    {
        var resistor = ResistorMode.InternalPullUp;

        //var d0 = _hardware.Gpio.Pins.D00.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, resistor);
        var d1 = _hardware.Gpio.Pins.D01.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, resistor);
        d1.Changed += (s, e) => { Resolver.Log.Info($"D1 changed to {e.New.State}"); };
        var d2 = _hardware.Gpio.Pins.D02.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, resistor);
        d2.Changed += (s, e) => { Resolver.Log.Info($"D2 changed to {e.New.State}"); };
        var d3 = _hardware.Gpio.Pins.D03.CreateDigitalInterruptPort(InterruptMode.EdgeFalling, resistor);
        d3.Changed += (s, e) => { Resolver.Log.Info($"D3 changed to {e.New.State}"); };

        while (true)
        {
            //Resolver.Log.Info($"D0: {d0.State}");
            Resolver.Log.Info($"D1: {d1.State}");
            Resolver.Log.Info($"D2: {d2.State}");
            Resolver.Log.Info($"D3: {d3.State}");

            await Task.Delay(1000);
        }
    }
}
