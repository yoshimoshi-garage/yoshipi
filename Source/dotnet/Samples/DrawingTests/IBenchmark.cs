using Meadow.Foundation.Graphics;

namespace DrawingTests;

internal interface IBenchmark
{
    public string Name { get; }

    public void Initialize(MicroGraphics Graphics);
    public void Run(int numberOfFrames);
}