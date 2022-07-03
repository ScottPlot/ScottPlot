using System;
using BenchmarkDotNet.Running;

namespace ScottPlot.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "benchmark")
            {
                BenchmarkRunner.Run(typeof(Program).Assembly);
            }
            else
            {
                BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            }
        }
    }
}
