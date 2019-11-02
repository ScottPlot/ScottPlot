using System.Diagnostics;

namespace ConsoleAppFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            string outFilePath = System.IO.Path.GetFullPath("consoleAppFramework.png");

            var plot = new ScottPlot.Plot(640, 480);
            plot.Title("Console Application Quickstart (.NET Framework)");
            plot.PlotSignal(ScottPlot.DataGen.Sin(100));
            plot.PlotSignal(ScottPlot.DataGen.Cos(100));
            plot.SaveFig(outFilePath);

            Debug.WriteLine($"Wrote: {outFilePath}");
        }
    }
}
