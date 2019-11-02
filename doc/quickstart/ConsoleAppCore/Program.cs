using System;
using System.Diagnostics;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string outFilePath = System.IO.Path.GetFullPath("consoleAppCore.png");

            var plot = new ScottPlot.Plot(640, 480);
            plot.Title("Console Application Quickstart (.NET Core)");
            plot.PlotSignal(ScottPlot.DataGen.Sin(100));
            plot.PlotSignal(ScottPlot.DataGen.Cos(100));
            plot.SaveFig(outFilePath);

            Debug.WriteLine($"Wrote: {outFilePath}");
        }
    }
}
