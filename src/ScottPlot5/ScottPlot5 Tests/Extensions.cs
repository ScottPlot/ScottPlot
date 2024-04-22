using SkiaSharp;
using System.Diagnostics;
using System.Reflection;

namespace ScottPlotTests;

internal static class Extensions
{
    // TODO: deprecate save methods in favor of Should() methods

    internal static void SaveTestImage(this Image img)
    {
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + ".png";
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        img.SavePng(filePath);
    }

    internal static void SaveTestImage(this SKSurface surface)
    {
        Image img = new(surface);

        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + ".png";
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        img.SavePng(filePath);
    }

    internal static void SaveTestImage(this Plot plt, int width = 600, int height = 400, string subName = "")
    {
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        if (!string.IsNullOrWhiteSpace(subName))
            subName = "_" + subName;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + subName + ".png";
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        plt.SavePng(filePath, width, height);
    }

    internal static void SaveTestSvg(this Plot plt, int width = 600, int height = 400, string subName = "")
    {
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("unknown caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("unknown method");
        string callingMethod = method.Name;

        if (!string.IsNullOrWhiteSpace(subName))
            subName = "_" + subName;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + subName + ".svg";
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        plt.SaveSvg(filePath, width, height);
    }

    internal static void SaveTestImage(this SKBitmap bmp, string subName = "")
    {
        // determine filename based on name of calling function
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("bad caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("bad method");
        string callingMethod = method.Name;

        if (!string.IsNullOrWhiteSpace(subName))
            subName = "_" + subName;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + subName + ".png";
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        // actually save the thing
        using SKFileWStream fs = new(filePath);
        bmp.Encode(fs, SKEncodedImageFormat.Png, quality: 100);
    }

    internal static void SaveTestString(this string s, string extension = ".html")
    {
        // determine filename based on name of calling function
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1) ?? throw new InvalidOperationException("bad caller");
        MethodBase method = frame.GetMethod() ?? throw new InvalidDataException("bad method");
        string callingMethod = method.Name;

        string saveFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
        if (!Directory.Exists(saveFolder))
            Directory.CreateDirectory(saveFolder);

        string fileName = callingMethod + extension;
        string filePath = Path.Combine(saveFolder, fileName);
        Console.WriteLine(filePath);

        // actually save the thing
        File.WriteAllText(filePath, s);
    }

    internal static PlotAssertions Should(this Plot plot)
    {
        return new PlotAssertions(plot);
    }
}
