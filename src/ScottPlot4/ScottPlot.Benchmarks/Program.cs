using System;
using BenchmarkDotNet.Running;

namespace ScottPlot.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "menu")
            {
                BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            }
            else if (args.Length == 1 && args[0] == "run")
            {
                BenchmarkRunner.Run(typeof(Program).Assembly);
            }
            else
            {
                Console.WriteLine("Single argument required: 'menu' or 'run'");
            }
        }
    }
}
