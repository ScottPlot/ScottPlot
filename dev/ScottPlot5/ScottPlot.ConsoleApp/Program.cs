using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace ScottPlot.ConsoleApp;

public static class Program
{
    public static void Main()
    {
        using SkiaBitmapExportContext context = new(400, 300, 1.0f);

        var plt = new ScottPlot.Plot();
        plt.AddDemoSinAndCos();
        plt.Draw(context.Canvas, context.Width, context.Height);

        string filePath = Path.GetFullPath("test.png");
        using FileStream fs = new("test.png", FileMode.Create);
        context.WriteToStream(fs);
        Console.WriteLine(filePath);
    }
}