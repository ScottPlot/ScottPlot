using BenchmarkDotNet.Running;

namespace ScottPlot.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
