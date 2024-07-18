using Meadow;
using Meadow.Foundation.Graphics;

namespace DrawingTests;

internal class FillBenchmark : IBenchmark
{
    public string Name => "Fill";

    private MicroGraphics graphics;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;
    }

    public void Run(int numberOfFrames)
    {
        Color color = Color.Cyan;

        for (int frame = 0; frame < numberOfFrames; frame++)
        {
            graphics.Clear(color, true);
            color = color.WithHue(color.Hue + 0.02f);
        }
    }
}