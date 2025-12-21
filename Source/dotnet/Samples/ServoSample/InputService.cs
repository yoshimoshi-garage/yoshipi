using Meadow;
using Meadow.Hardware;

namespace ServoSample;

public class InputService
{
    public event EventHandler<bool>? ButtonStateChanged;
    public event EventHandler<float>? YValueChanged;
    public event EventHandler<float>? XValueChanged;

    private readonly IDigitalInterruptPort _buttonPort;
    private readonly IObservableAnalogInputPort _xInput;
    private readonly IObservableAnalogInputPort _yInput;

    public InputService(IDigitalInterruptPort buttonPort, IObservableAnalogInputPort xInput, IObservableAnalogInputPort yInput)
    {
        _buttonPort = buttonPort;
        _xInput = xInput;
        _yInput = yInput;

        _buttonPort.Changed += OnButtonChanged;
        _xInput.Updated += OnXInputUpdated;
        _yInput.Updated += OnYInputUpdated;
    }

    public void Start()
    {
        _xInput.StartUpdating(TimeSpan.FromMilliseconds(200));
        _yInput.StartUpdating(TimeSpan.FromMilliseconds(200));
    }

    private void OnXInputUpdated(object? sender, IChangeResult<Meadow.Units.Voltage> e)
    {
        XValueChanged?.Invoke(this, (float)e.New.Volts);
    }

    private void OnYInputUpdated(object? sender, IChangeResult<Meadow.Units.Voltage> e)
    {
        YValueChanged?.Invoke(this, (float)e.New.Volts);
    }

    private void OnButtonChanged(object? sender, DigitalPortResult e)
    {
        ButtonStateChanged?.Invoke(this, e.New.State);
    }
}
