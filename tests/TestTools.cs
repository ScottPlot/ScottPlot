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
    }
}
