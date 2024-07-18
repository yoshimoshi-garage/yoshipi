using Meadow;
using Meadow.Foundation.Graphics;

namespace DrawingTests;

internal class ShapesBenchmark : IBenchmark
{
    public string Name => "Shapes";

    private MicroGraphics graphics;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;
    }

    public void Run(int numberOfFrames)
    {
        for (int frame = 0; frame < numberOfFrames; frame++)
        {
            int index = frame % 40;

            graphics.Clear();

            graphics.DrawRoundedRectangle(10 + index, 10 + index, 120, 80, 10, Color.Purple, true);
            graphics.DrawRectangle(index * 2, index * 2, 60, 30, Color.LawnGreen, true);
            graphics.DrawTriangle(40 + index, index, index, index + 80, index + 80, 80 + index, Color.Yellow, true);
            graphics.DrawCircle(160 + index * 3, 60 + index * 3, 30, Color.Cyan, true);

            graphics.Show();
        }
    }
}