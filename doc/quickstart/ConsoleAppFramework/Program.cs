using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFilePath = System.IO.Path.GetFullPath("consoleAppFramework.png");

            var plt = new ScottPlot.Plot(640, 480);
            plt.Title("ScottPlot QuickStart: Console Application (.NET Framework)");
            plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            plt.SaveFig(outputFilePath);

            Console.WriteLine($"Saved: {outputFilePath}");
        }
    }
}
