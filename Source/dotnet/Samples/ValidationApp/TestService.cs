using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Foundation.Hmi;
using Meadow.Foundation.Sensors.Light;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;
using System.IO;
using System.Threading.Tasks;
using YoshiPi;

namespace Validation;

public enum TestResult
{
    Pass,
    Fail,
    Skip
}

public class TestService
{
    private IYoshiPiHardware _hardware;
    private DisplayService _display;
    private TestResult? _lastResult = null;

    private void DisplayResultClicked(object? sender, TestResult e)
    {
        _lastResult = e;
    }

    public TestService(IYoshiPiHardware hardware)
    {
        _hardware = hardware;
    }

    private int _passCount = 0;
    private int _failCount = 0;
    private int _skipCount = 0;

    private async Task RunTest(Func<Task<TestResult>> test)
    {
        _lastResult = null;
        var result = await test();

        switch (result)
        {
            case TestResult.Pass: _passCount++; break;
            case TestResult.Fail: _failCount++; break;
            default: _skipCount++; break;
        };

        _display.ClearTestLabels();
    }

    private async Task RunTest(Func<bool, Task<TestResult>> test, bool param)
    {
        _lastResult = null;
        var result = await test(param);

        switch (result)
        {
            case TestResult.Pass: _passCount++; break;
            case TestResult.Fail: _failCount++; break;
            default: _skipCount++; break;
        };

        _display.ClearTestLabels();
    }

    public async Task StartTests()
    {
        await RunTest(TestDisplay);
        await RunTest(TestQwiic);
        await RunTest(TestRtc);
        await RunTest(TestButtons);
        await RunTest(TestRelays);
        await RunTest(TestGpioOutputs);
        await RunTest(TestGpioInputs, true);
        await RunTest(TestGpioInputs, false);

        _display.ShowResults(_passCount, _failCount, _skipCount);
    }

    public async Task<TestResult> TestDisplay()
    {
        // we'll test the display and touchscreen together

        _display = new DisplayService(
            new DisplayScreen(
                (IPixelDisplay)_hardware.Display,
                RotationType._270Degrees,
                _hardware.Touchscreen)
            );

        var ts = new TouchscreenCalibrationService(_display.Screen, new FileInfo("calibration.dat"));

        ts.EraseCalibrationData();

        var calData = ts.GetSavedCalibrationData();
        if (calData != null)
        {
            _hardware.Touchscreen.SetCalibrationData(calData);
        }
        else
        {
            Resolver.Log.Info("Calibrating");

            await ts.Calibrate(true);
        }

        _display.CreateValidationControls();

        _display.ResultClicked += DisplayResultClicked;

        _display.SetTestName("Display and Touch");
        _display.SetQuestionText("Is this text visible?");

        while (_lastResult == null)
        {
            await Task.Delay(250);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestRelays()
    {
        _display.SetTestName("Relays");
        _display.SetQuestionText("Are both relays toggling?");

        while (_lastResult == null)
        {
            _hardware.Relay1.Toggle();
            _hardware.Relay2.Toggle();

            await Task.Delay(1000);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestGpioOutputs()
    {
        using var d0 = _hardware.Gpio.Pins.D00.CreateDigitalOutputPort(false);
        using var d1 = _hardware.Gpio.Pins.D01.CreateDigitalOutputPort(true);
        using var d2 = _hardware.Gpio.Pins.D02.CreateDigitalOutputPort(false);
        using var d3 = _hardware.Gpio.Pins.D03.CreateDigitalOutputPort(true);
        using var d4 = _hardware.Gpio.Pins.D04.CreateDigitalOutputPort(false);
        using var d5 = _hardware.Gpio.Pins.D05.CreateDigitalOutputPort(true);
        using var d6 = _hardware.Gpio.Pins.D06.CreateDigitalOutputPort(false);
        using var d7 = _hardware.Gpio.Pins.D07.CreateDigitalOutputPort(true);

        _display.SetTestName("GPIO Output");
        _display.SetQuestionText("Are all GPIOs toggling?");

        while (_lastResult == null)
        {
            d0.State = !d0.State;
            d1.State = !d1.State;
            d2.State = !d2.State;
            d3.State = !d3.State;
            d4.State = !d4.State;
            d5.State = !d5.State;
            d6.State = !d6.State;
            d7.State = !d7.State;

            await Task.Delay(1000);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestGpioInputs(bool pullUp)
    {
        var resistor = pullUp ? ResistorMode.InternalPullUp : ResistorMode.InternalPullDown;

        using var d0 = _hardware.Gpio.Pins.D00.CreateDigitalInputPort(resistor);
        using var d1 = _hardware.Gpio.Pins.D01.CreateDigitalInputPort(resistor);
        using var d2 = _hardware.Gpio.Pins.D02.CreateDigitalInputPort(resistor);
        using var d3 = _hardware.Gpio.Pins.D03.CreateDigitalInputPort(resistor);
        using var d4 = _hardware.Gpio.Pins.D04.CreateDigitalInputPort(resistor);
        using var d5 = _hardware.Gpio.Pins.D05.CreateDigitalInputPort(resistor);
        using var d6 = _hardware.Gpio.Pins.D06.CreateDigitalInputPort(resistor);
        using var d7 = _hardware.Gpio.Pins.D07.CreateDigitalInputPort(resistor);

        if (pullUp)
        {
            _display.SetTestName("GPIO Input to Ground");
        }
        else
        {
            _display.SetTestName("GPIO Input to 3.3V");
        }
        _display.SetQuestionText("Are all inputs working?");

        while (_lastResult == null)
        {
            string s = string.Empty;
            s += d0.State ? "1" : "0";
            s += d1.State ? "1" : "0";
            s += d2.State ? "1" : "0";
            s += d3.State ? "1" : "0";
            s += " ";
            s += d4.State ? "1" : "0";
            s += d5.State ? "1" : "0";
            s += d6.State ? "1" : "0";
            s += d7.State ? "1" : "0";

            _display.SetInputsLabel(s);

            await Task.Delay(1000);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestButtons()
    {
        var state1 = false;
        var state2 = false;

        _hardware.Button1.PressStarted += (s, e) => state1 = true;
        _hardware.Button1.PressEnded += (s, e) => state1 = false;
        _hardware.Button2.PressStarted += (s, e) => state2 = true;
        _hardware.Button2.PressEnded += (s, e) => state2 = false;

        _display.SetTestName("Button Clicks");
        _display.SetQuestionText("Are both buttons working?");
        _display.SetInstructionText("(click to change state)");

        while (_lastResult == null)
        {
            _display.SetInputsLabel($"{state1}  {state2}");

            await Task.Delay(500);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestRtc()
    {
        _hardware.Rtc.SetTime(DateTime.Now.ToUniversalTime());

        _display.SetTestName("RTC Time");
        _display.SetQuestionText("Does this (UTC) time look correct?");

        while (_lastResult == null)
        {
            var rtc = _hardware.Rtc.GetTime();
            _display.SetInputsLabel($"{rtc:HH:mm:ss}");

            await Task.Delay(1000);
        }

        return _lastResult.Value;
    }

    public async Task<TestResult> TestQwiic()
    {
        Veml7700? sensor = null;
        try
        {
            sensor = new Veml7700(_hardware.Qwiic);
            sensor.StartUpdating();
        }
        catch (Exception ex)
        {
            Resolver.Log.Warn($"Unable to create Light Sensor: {ex.Message}");
        }

        _display.SetTestName("Qwiic Light Sensor");

        if (sensor == null)
        {
            _display.SetQuestionText("Light sensor missing. Press SKIP");
        }
        else
        {
            _display.SetQuestionText("Does this light look correct?");
        }

        while (_lastResult == null)
        {
            if (sensor != null)
            {
                try
                {
                    var read = await sensor.Read();
                    _display.SetInputsLabel($"{read.Lux:n0} lux");
                }
                catch (Exception ex)
                {
                    _display.SetQuestionText("Light sensor missing. Press SKIP");
                    Resolver.Log.Warn($"Unable to read Light Sensor: {ex.Message}");
                }
            }

            await Task.Delay(1000);
        }

        return _lastResult.Value;
    }

}
