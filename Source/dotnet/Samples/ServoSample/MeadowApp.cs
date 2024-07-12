using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Peripherals;
using Meadow.Units;
using YoshiPi;

namespace ServoSample;

public class MeadowApp : YoshiPiApp
{
    private ContinuousRotationServo? _crServo;
    private AngularServo? _aServo;

    //private Servo? _servo;
    private Pca9685 _pca9685;
    private IPwmPort _pwm;

    private double _speed = 0;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _pca9685 = new Pca9685(Hardware.GroveI2c);
        _pwm = _pca9685.CreatePwmPort(_pca9685.Pins.LED0, 0.5f, false);

        //CreateContinuousRotationServo();
        CreateAngularServo();

        //_servo = new Servo(_pwm, NamedServoConfigs.SG90);


        //_servo = new ContinuousRotationServo(_pwm, config)
        //{
        //    TrimDuration = TimeSpan.FromMicroseconds(100)
        //};
        //_servo?.Rotate(RotationDirection.None, 0);

        //_servo = new Servo(_pwm, NamedServoConfigs.SG90);
        //_servo.RotateTo(new Angle(0)).Wait();

        Hardware.Button1.PressStarted += CcwButtonPressStarted;
        Hardware.Button1.PressEnded += ButtonPressEnded;
        Hardware.Button2.PressStarted += CwButtonPressStarted;
        Hardware.Button2.PressEnded += ButtonPressEnded;

        return Task.CompletedTask;
    }

    private void CreateAngularServo()
    {
        _aServo = new Mg996(_pwm)
        {
            //            TrimOffset = TimeSpan.FromMicroseconds(-140)
        };
    }

    private void CreateContinuousRotationServo()
    {
        _crServo = new Fs90r(_pwm)
        {
            TrimOffset = TimePeriod.FromMicroseconds(100)
        };
    }

    private double pwmCalibration = 100d;

    private async void ButtonPressEnded(object? sender, EventArgs e)
    {
        //_pwm.Duration = TimeSpan.FromMicroseconds(1500 + pwmCalibration);

        //await _servo.RotateTo(new Angle(0));

        _crServo?.Neutral();
        _aServo?.Neutral();
    }

    private async void CcwButtonPressStarted(object? sender, EventArgs e)
    {
        //await _servo.RotateTo(new Angle(-90));

        //_pwm.Duration = TimeSpan.FromMicroseconds(900 + pwmCalibration);
        _crServo?.Rotate(RotationDirection.CounterClockwise, 1);
        _aServo?.RotateTo(_aServo.MinimumAngle);
    }

    private async void CwButtonPressStarted(object? sender, EventArgs e)
    {
        if (_speed < 1) _speed += 0.1;
        if (_speed > 1) _speed = 1;

        //await _servo.RotateTo(new Angle(90));
        //_pwm.Duration = TimeSpan.FromMicroseconds(2100 + pwmCalibration);
        _crServo?.Rotate(RotationDirection.Clockwise, 1);
        _aServo?.RotateTo(_aServo.MaximumAngle);
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}
