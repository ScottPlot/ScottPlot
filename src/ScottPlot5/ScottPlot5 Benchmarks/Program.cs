using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlotBench;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("\nType 's' to run slow but careful tests.");
        Console.WriteLine("Press ENTER to proceed with quick and dirty tests.");
        bool slow = Console.ReadKey().Key == ConsoleKey.S;
        string configType = slow ? "slow but careful" : "quick and dirty";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Proceeding with {configType} configuration...\n");
        IConfig config = slow ? Configurations.SlowAndCareful : Configurations.QuickAndDirty;

        Type[] benchmarks = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace == "ScottPlotBench.Benchmarks")
            .ToArray();

        BenchmarkSwitcher switcher = new(benchmarks);
        switcher.Run(args, config);
    }
}
