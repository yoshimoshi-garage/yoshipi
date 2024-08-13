using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Peripherals.Servos;
using Meadow.Units;

namespace ServoSample;

public class ServoService
{
    private float _lastCommandedPosition = 0f;
    private Pca9685 _pca;
    private IAngularServo _servo;

    public ServoService(II2cBus i2c)
    {
        try
        {
            Resolver.Log.Info($"Creating PCA9685");
            _pca = new Pca9685(i2c, 50.Hertz());
            _servo = new Ms24(_pca.Pins.LED0);
        }
        catch (Exception ex)
        {
            Resolver.Log.Info($"ERROR: {ex.Message}");
        }
    }

    public void Start()
    {
        RotateTo(0f);
    }

    public void RotateTo(float value)
    {
        if (value == _lastCommandedPosition) return;

        var mid = 3.3 / 2d;
        var midl = mid - 0.02;
        var midh = mid + 0.02;

        if (value > midh)
        { // right
            var percentage = (float)((mid - value) / mid);
            _servo.RotateTo(_servo.MaximumAngle * percentage);
        }
        else if (value < midl)
        { // left
            var percentage = (float)((value - mid) / mid);
            _servo.RotateTo(_servo.MinimumAngle * percentage);
        }
        else
        {
            _servo.RotateTo(Angle.Zero);
        }
        _lastCommandedPosition = value;
    }
}
