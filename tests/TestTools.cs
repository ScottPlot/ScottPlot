using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    public static class TestTools
    {
        public static void SaveFig(ScottPlot.Plot plt, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            string fileName = callingMethod + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            plt.SaveFig(filePath);

            DisplayRenderInfo(callingMethod, subName, plt.GetTotalPoints(), plt.GetSettings(false).benchmark.msec);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
        }

        public static void SaveFig(ScottPlot.MultiPlot mplt, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            string fileName = callingMethod + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            mplt.SaveFig(filePath);

            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
        }

        private static void DisplayRenderInfo(string callingMethod, string subName, int totalPoints, double renderTimeMs)
        {
            Console.WriteLine($"{callingMethod}() {subName}");
            Console.WriteLine($"Rendered {totalPoints} points in {renderTimeMs} ms");
        }

        public static string HashedFig(ScottPlot.Plot plt, string subName = "")
        {
            string hash = ScottPlot.Tools.BitmapHash(plt.GetBitmap(true));

            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            DisplayRenderInfo(callingMethod, subName, plt.GetTotalPoints(), plt.GetSettings(false).benchmark.msec);
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine();

            return hash;
        }

        public static ScottPlot.Plot SamplePlotScatter(int width = 600, int height = 400)
        {
            double[] dataXs = ScottPlot.DataGen.Consecutive(50);
            double[] dataSin = ScottPlot.DataGen.Sin(50);
            double[] dataCos = ScottPlot.DataGen.Cos(50);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            return plt;
        }
    }
}
