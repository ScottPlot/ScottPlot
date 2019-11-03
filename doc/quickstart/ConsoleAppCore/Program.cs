using System;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFilePath = System.IO.Path.GetFullPath("consoleAppCore.png");

            var plt = new ScottPlot.Plot(640, 480);
            plt.Title("ScottPlot QuickStart: Console Application (.NET Core)");
            plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            plt.SaveFig(outputFilePath);

            Console.WriteLine($"Saved: {outputFilePath}");
        }
    }
}
