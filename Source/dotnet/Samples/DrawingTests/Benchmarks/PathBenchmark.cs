using Meadow;
using Meadow.Foundation.Graphics;
using System;

namespace DrawingTests;

internal class PathBenchmark : IBenchmark
{
    private const string BenchmarkName = "Path";

    private const int PathPointsCount = 65;
    private const double DegreesIncrement = 11.25;
    private const int XIncrement = 5;
    private const int YBase = 120;
    private const int YAmplitude = 100;
    private const int StrokeThickness = 2;
    private const int LineStartX = 0;
    private const int LineStartY = YBase;
    private const int LineEndX = 320;
    private const int LineEndY = YBase;

    private MicroGraphics graphics;

    public string Name => BenchmarkName;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics ?? throw new ArgumentNullException(nameof(Graphics));
    }

    public void Run(int numberOfFrames)
    {
        for (int i = 0; i < numberOfFrames; i++)
        {
            graphics.Clear();

            DrawPaths(i / 10.0);

            graphics.Show();
        }
        graphics.Stroke = 1;
    }

    void DrawPaths(double offset)
    {
        var pathSin = new GraphicsPath();
        var pathCos = new GraphicsPath();

        for (int i = 0; i < PathPointsCount; i++)
        {
            if (i == 0)
            {
                pathSin.MoveTo(LineStartX, YBase + (int)(Math.Sin(offset + i * DegreesIncrement * Math.PI / 180) * YAmplitude));
                pathCos.MoveTo(LineStartX, YBase + (int)(Math.Cos(offset + i * DegreesIncrement * Math.PI / 180) * YAmplitude));
                continue;
            }

            pathSin.LineTo(i * XIncrement, YBase + (int)(Math.Sin(offset + i * DegreesIncrement * Math.PI / 180) * YAmplitude));
            pathCos.LineTo(i * XIncrement, YBase + (int)(Math.Cos(offset + i * DegreesIncrement * Math.PI / 180) * YAmplitude));
        }

        graphics.Stroke = StrokeThickness;
        graphics.DrawLine(LineStartX, LineStartY, LineEndX, LineEndY, Color.White);
        graphics.DrawPath(pathSin, Color.Cyan);
        graphics.DrawPath(pathCos, Color.LawnGreen);
    }
}
