using Meadow;
using Meadow.Foundation.Graphics;

namespace DrawingTests;

internal class GradientFillBenchmark : IBenchmark
{
    public string Name => "GradientFill";

    private MicroGraphics graphics;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;
    }

    public void Run(int numberOfFrames)
    {
        Color color1 = Color.Cyan;
        Color color2 = Color.Purple;

        for (int frame = 0; frame < numberOfFrames; frame++)
        {
            graphics.DrawVerticalGradient(0, 0, graphics.Width, graphics.Height, color1, color2);
            graphics.Show();
            color1 = color1.WithHue(color1.Hue + 0.05f);
            color2 = color2.WithHue(color2.Hue + 0.05f);
        }
    }
}