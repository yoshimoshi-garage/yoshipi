using Meadow;
using Meadow.Foundation.Graphics;

namespace DrawingTests;

public class PixelBenchmark : IBenchmark
{
    public string Name => "Pixel";

    private MicroGraphics graphics;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;
    }

    public void Run(int numberOfFrames)
    {
        graphics.Clear();

        for (int i = 0; i < numberOfFrames; i++)
        {
            graphics.DrawPixel(i % graphics.Width, i / graphics.Height, Color.White);
            graphics.Show();
        }
    }
}