using BenchmarkDotNet.Attributes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotBench.Benchmarks
{
    public class Histogram
    {
        [Params(1_000, 10_000, 100_000, 1_000_000)]
        public int Points { get; set; }

        [Params(100, 200, 500)]
        public int BinCount { get; set; }

        public double[] Values { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            ScottPlot.RandomDataGenerator gen = new(0);
            Values = gen.RandomSample(Points);
        }

        [Benchmark]
        public void CreateHistogram()
        {
            _ = ScottPlot.Statistics.Histogram.WithBinCount(BinCount, Values);
        }
    }
}
