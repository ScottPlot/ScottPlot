using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests
{
    public static class TestTools
    {
        [Obsolete("WARNING: LaunchFig() is just for testing by developers")]
        public static void LaunchFig(ScottPlot.Plot plt)
        {
            string filePath = SaveFig(plt);
            ScottPlot.Tools.LaunchBrowser(filePath);
        }

        public static string SaveFig(ScottPlot.Plot plt, string subName = "", bool artifact = false)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            if (subName != "")
                subName = "_" + subName;

            string fileName = callingMethod + subName + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            plt.SaveFig(filePath);

            DisplayRenderInfo(callingMethod, subName, plt.GetTotalPoints(), plt.GetSettings(false).benchmark.msec);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();

            if (artifact)
                SaveArtifact(plt, callingMethod + subName);

            return filePath;
        }

        private static void SaveArtifact(ScottPlot.Plot plt, string name)
        {
            string osNameShort = ScottPlot.Tools.GetOsName(details: false);
            string artifactFolder = System.IO.Path.GetFullPath("./artifacts/");
            if (!System.IO.Directory.Exists(artifactFolder))
                System.IO.Directory.CreateDirectory(artifactFolder);
            string artifactFilePath = System.IO.Path.Combine(artifactFolder, $"{name}_{osNameShort}.png");

            plt.SaveFig(artifactFilePath);

            Console.WriteLine($"Saved artifact: {artifactFilePath}");
            Console.WriteLine();
        }

        public static string SaveFig(System.Drawing.Bitmap bmp, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            if (subName != "")
                subName = "_" + subName;

            string fileName = callingMethod + subName + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

            DisplayRenderInfo(callingMethod, subName, 0, 0);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();

            return filePath;
        }

        public static string SaveFig(ScottPlot.MultiPlot mplt, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            string fileName = callingMethod + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            mplt.SaveFig(filePath);

            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();

            return filePath;
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
