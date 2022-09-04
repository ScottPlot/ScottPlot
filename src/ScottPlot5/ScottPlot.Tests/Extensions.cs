using System.Diagnostics;
using System.Reflection;

namespace ScottPlot.TestsV5;

internal static class Extensions
{
    internal static void SaveTestImage(this Plot plt, string subName = "", int width = 600, int height = 400)
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
