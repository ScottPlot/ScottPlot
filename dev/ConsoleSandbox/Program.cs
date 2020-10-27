using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Plotting...");

        var plt = new ScottPlot.Plot(600, 400);
        plt.PlotLine(0, 1, 1, 1.0000000000000009);
        plt.GetBitmap();

        Console.WriteLine("Done!");
    }
}
