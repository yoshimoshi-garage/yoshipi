using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using YoshiPi;

namespace DrawingTests;

public class MeadowApp : YoshiPiApp
{
    List<BenchmarkResult> benchmarkResults = default!;

    MicroGraphics graphics = default!;

    bool isRunning = false;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        benchmarkResults = new List<BenchmarkResult>();

        (Hardware.Display as Ili9341).SpiBusSpeed = new Meadow.Units.Frequency(62.5, Meadow.Units.Frequency.UnitType.Megahertz);

        graphics = new MicroGraphics(Hardware.Display);
        graphics.Rotation = Meadow.Peripherals.Displays.RotationType._270Degrees;

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        Benchmark();

        return Task.CompletedTask;

    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public void Benchmark()
    {
        if (isRunning == true)
        {
            Console.WriteLine("Benchmark already running...");
            return;
        }

        isRunning = true;

        Console.WriteLine("Run benchmarks...");

        RunBenchmarks();

        ShowResults();

        isRunning = false;
    }

    void RunBenchmarks()
    {
        benchmarkResults.Clear();

        benchmarkResults.Add(RunBenchmark(new PixelBenchmark()));
        benchmarkResults.Add(RunBenchmark(new ShapesBenchmark()));
        benchmarkResults.Add(RunBenchmark(new PathBenchmark()));
        benchmarkResults.Add(RunBenchmark(new PartialShowBenchmark(), 100));
        benchmarkResults.Add(RunBenchmark(new StarfieldBenchmark()));
        benchmarkResults.Add(RunBenchmark(new FillBenchmark()));
        benchmarkResults.Add(RunBenchmark(new GradientFillBenchmark(), 15));
        benchmarkResults.Add(RunBenchmark(new SpriteBenchmark()));
    }

    void ShowResults()
    {
        graphics.Clear();
        graphics.CurrentFont = new Font8x12();

        graphics.DrawText(5, 2, "Results", Color.LawnGreen);
        graphics.Show();

        TimeSpan totalElapsed = TimeSpan.Zero;

        int y = 35;

        foreach (var result in benchmarkResults)
        {
            Thread.Sleep(250);
            totalElapsed += result.Elapsed;

            var fps = result.NumberOfFrames / result.Elapsed.TotalSeconds;

            graphics.DrawText(5, y, result.Name, Color.LawnGreen);
            graphics.DrawText(315, y, $"{fps:n2}fps", Color.LawnGreen, alignmentH: HorizontalAlignment.Right);
            graphics.Show();

            y += 20;
        }

        Thread.Sleep(250);
        y += 30;
        graphics.DrawText(5, y, "Total time", Color.LawnGreen);
        graphics.DrawText(315, y, $"{totalElapsed.TotalSeconds:n2}s", Color.LawnGreen, alignmentH: HorizontalAlignment.Right);

        graphics.Show();
    }

    BenchmarkResult RunBenchmark(IBenchmark benchmark, int frames = 40)
    {
        benchmark.Initialize(graphics);

        Stopwatch stopwatch = new();

        Console.WriteLine($"{benchmark.Name}");
        GC.Collect();

        stopwatch.Start();
        benchmark.Run(frames);
        stopwatch.Stop();

        var fps = frames / stopwatch.Elapsed.TotalSeconds;

        Console.WriteLine($"   {fps:n2}fps");
        return new BenchmarkResult(benchmark.Name, frames, stopwatch.Elapsed);
    }

    public static byte[] LoadResource(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"DrawingTests.{filename}";

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using var ms = new MemoryStream();

        stream.CopyTo(ms);
        return ms.ToArray();
    }
}