using System.Diagnostics;
using System.Reflection;

namespace ScottPlotTests;

internal class PlotAssertions(Plot plot)
{
    readonly Plot Plot = plot;

    public void SavePngWithoutThrowing(string subName = "", int width = 400, int height = 300)
    {
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = string.IsNullOrWhiteSpace(subName)
            ? $"{callingMethod}.png"
            : $"{callingMethod}-{subName}.png";

        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        Plot.SavePng(filePath, width, height);
    }

    public void RenderInMemoryWithoutThrowing(int width = 400, int height = 300)
    {
        Plot.GetImage(width, height);
    }
}
