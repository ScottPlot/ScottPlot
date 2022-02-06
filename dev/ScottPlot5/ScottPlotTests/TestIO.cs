using System;
using System.IO;

namespace ScottPlotTests;

internal static class TestIO
{
    public static string SaveFig(ScottPlot.Plot plot)
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        string callingMethodName = stackTrace.GetFrame(1)!.GetMethod()!.Name;
        string filename = callingMethodName + ".png";
        string folder = Path.GetFullPath("RenderTest");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, filename);

        var sw = System.Diagnostics.Stopwatch.StartNew();
        plot.SaveFig(path);
        sw.Stop();

        Console.WriteLine($"Saved in {sw.Elapsed.TotalMilliseconds:N3} ms");
        Console.WriteLine(path);
        return path;
    }
}
