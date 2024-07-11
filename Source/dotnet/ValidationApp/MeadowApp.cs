using Meadow;
using System.Threading.Tasks;
using YoshiPi;

namespace Validation;

public class MeadowApp : YoshiPiApp
{
    private TestService _testService;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _testService = new TestService(Hardware);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await _testService.StartTests();
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
/*
private async Task TestMikrobusUartLoopback()
{
    var port = _hardware.MikroBus.CreateSerialPort();
    port.Open();

    var tx = new byte[10];
    var rx = new byte[10];

    while (true)
    {
        Random.Shared.NextBytes(tx);

        port.Write(tx);
        await Task.Delay(100);
        Resolver.Log.Info($"Available: {port.BytesToRead}");
        var r = port.Read(rx, 0, rx.Length);
        Resolver.Log.Info($"Read: {r}");

        Resolver.Log.Info($"{BitConverter.ToString(tx)} --> {BitConverter.ToString(rx)}");

        await Task.Delay(2000);
    }
}

private async Task TestGroveI2C_BH1900()
{
    var sensor = new Bh1900Nux(
        _hardware.GroveI2c, Bh1900Nux.Addresses.Default);

    while (true)
    {
        var temp = await sensor.Read();

        Resolver.Log.Info($"Temp: {temp.Fahrenheit:N1} F");

        await Task.Delay(1000);
    }
}

private async Task TestMikrobusSpi_Gnss10()
{
    Resolver.Log.Info($"GNSS over SPI");

    var sensor = new CGNSS10(
        _hardware.MikroBus.SpiBus,
        _hardware.MikroBus.Pins.CS,
        _hardware.MikroBus.Pins.RST);

    await sensor.Reset();
    sensor.StartUpdating();
    sensor.GgaReceived += (s, e) => { Resolver.Log.Info($"GGA"); };
}

private void TestDisplay()
{
    var graphics = new MicroGraphics(_hardware.Display);
    graphics.Rotation = Meadow.Peripherals.Displays.RotationType._270Degrees;

    graphics.Clear(Color.Cyan);
    graphics.DrawText(100, 50, "HELLO YOSHIPI!", ScaleFactor.X2);
    graphics.Show();
}

private async Task TestADCs()
{
    var ch0 = _hardware.Adc.Pins.A00.CreateAnalogInputPort();
    var ch1 = _hardware.Adc.Pins.A01.CreateAnalogInputPort();
    var ch2 = _hardware.Adc.Pins.A02.CreateAnalogInputPort();
    var ch3 = _hardware.Adc.Pins.A03.CreateAnalogInputPort();
    while (true)
    {
        Resolver.Log.Info($"--------");
        Resolver.Log.Info($"Read A00: {ch0.Voltage:N2} V");
        Resolver.Log.Info($"Read A01: {ch1.Voltage:N2} V");
        Resolver.Log.Info($"Read A02: {ch2.Voltage:N2} V");
        Resolver.Log.Info($"Read A03: {ch3.Voltage:N2} V");

        Thread.Sleep(2000);
    }
}

private void TestRtc()
{
    var now = _hardware.Rtc.GetTime();
    Resolver.Log.Info($"RTC says it is: {now:yyyy-mm-dd HH:mm:ss} UTC");
    if (now < new DateTime(2024, 5, 31))
    {
        _hardware.Rtc.SetTime(new DateTimeOffset(2024, 5, 31, 17, 50, 00, TimeSpan.Zero));
        Resolver.Log.Info($"RTC set to: {now:yyyy-mm-dd HH:mm:ss} UTC");
    }
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
    var gp6 = _hardware.Button1.MCP.Pins.GP6.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
    var gp5 = _hardware.MCP.Pins.GP5.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);

    gp5.Changed += (s, e) => Resolver.Log.Info($"GP5 changed");
    gp6.Changed += (s, e) => Resolver.Log.Info($"GP6 changed");

    while (true)
    {
        Resolver.Log.Info($"GP5: {gp5.State}");
        Resolver.Log.Info($"GP6: {gp6.State}");

        Thread.Sleep(1000);
    }
    //        _hardware.Button1.Clicked += (s, e) => { Resolver.Log.Info("Button1 clicked"); };
    //        _hardware.Button2.Clicked += (s, e) => { Resolver.Log.Info("Button2 clicked"); };
}

private void TestButtonsAsGPIO()
{

    var gp6 = _hardware.MCP.Pins.GP6.CreateDigitalInputPort(ResistorMode.InternalPullUp);
    var gp5 = _hardware.MCP.Pins.GP5.CreateDigitalInputPort(ResistorMode.InternalPullUp);

    while (true)
    {
        Resolver.Log.Info($"GP5: {gp5.State}");
        Resolver.Log.Info($"GP6: {gp6.State}");

        Thread.Sleep(1000);
    }
}

private async Task TestMikrobusGpioOutputs()
{
    var d0 = _hardware.MikroBus.Pins.INT.CreateDigitalOutputPort(false);
    var d1 = _hardware.MikroBus.Pins.PWM.CreateDigitalOutputPort(false);
    var d2 = _hardware.MikroBus.Pins.RST.CreateDigitalOutputPort(false);
    var d3 = _hardware.MikroBus.Pins.CS.CreateDigitalOutputPort(false);

    while (true)
    {

        d0.State = !d0.State;
        d1.State = !d1.State;
        d2.State = !d2.State;
        d3.State = !d3.State;

        await Task.Delay(1000);
    }
}

private async Task TestGpioOutputs()
{
    var d0 = _hardware.Gpio.Pins.D00.CreateDigitalOutputPort(false);
    var d1 = _hardware.Gpio.Pins.D01.CreateDigitalOutputPort(false);
    var d2 = _hardware.Gpio.Pins.D02.CreateDigitalOutputPort(false);
    var d3 = _hardware.Gpio.Pins.D03.CreateDigitalOutputPort(false);

    while (true)
    {

        d0.State = !d0.State;
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
*/