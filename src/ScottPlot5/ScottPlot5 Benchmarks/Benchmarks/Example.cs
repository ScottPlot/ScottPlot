using BenchmarkDotNet.Attributes;
using System.Text;

namespace ScottPlotBench.Benchmarks;

/// <summary>
/// Example benchmark
/// </summary>
public class Example
{
    [Params(10, 100, 1000)]
    public int Count { get; set; }

    [Benchmark]
    public void AddString()
    {
        string segment = "scott";
        string result = "";
        for (int i = 0; i < Count; i++)
        {
            result += segment;
        }
    }

    [Benchmark]
    public void AddStringBuilder()
    {
        StringBuilder sb = new();
        string segment = "scott";
        for (int i = 0; i < Count; i++)
        {
            sb.Append(segment);
        }
    }
}
