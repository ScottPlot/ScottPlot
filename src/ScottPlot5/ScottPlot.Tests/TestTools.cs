using System.Diagnostics;
using System.Reflection;

namespace ScottPlot.Tests;

internal static class TestTools
{
    public static void SaveImage(Plot plt, int width = 600, int height = 400, string subName = "", bool artifact = false)
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("bad caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("bad method");
        string callingMethod = method.Name;

        if (!string.IsNullOrWhiteSpace(subName))
            subName = "_" + subName;

        string fileName = callingMethod + subName + ".png";
        string filePath = Path.GetFullPath(fileName);
        plt.SaveImage(width, height, filePath);
        Console.WriteLine($"Saved: {filePath}");
    }
}
