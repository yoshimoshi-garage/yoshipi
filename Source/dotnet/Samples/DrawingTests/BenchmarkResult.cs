using System;

namespace DrawingTests;

internal record struct BenchmarkResult(string Name, int NumberOfFrames, TimeSpan Elapsed);
