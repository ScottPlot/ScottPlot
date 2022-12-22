using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;

namespace ScottPlotTests
{
    public static class TestTools
    {
        public static System.Drawing.Bitmap GetLowQualityBitmap(ScottPlot.Plot plt) =>
            new System.Drawing.Bitmap(plt.Render(lowQuality: true));

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
            string callingClass = stackTrace.GetFrame(1).GetMethod().DeclaringType.ToString();
            string prefix = callingClass + "." + callingMethod;

            if (subName != "")
                subName = "_" + subName;

            string fileName = prefix + subName + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            plt.SaveFig(filePath);

            DisplayRenderInfo(prefix, subName, plt.GetSettings(false).BenchmarkMessage.MSec);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();

            if (artifact)
                SaveArtifact(plt, prefix + subName);

            return filePath;
        }

        public static void SaveBitmap(System.Drawing.Bitmap bmp, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            string fileName = callingMethod + subName + ".png";
            string filePath = System.IO.Path.GetFullPath(fileName);
            bmp.Save(filePath, ImageFormat.Png);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
        }

        public static void SaveText(string txt, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            string fileName = callingMethod + subName + ".txt";
            string filePath = System.IO.Path.GetFullPath(fileName);
            System.IO.File.WriteAllText(filePath, txt);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
        }

        public static void SaveHtml(string body, string subName = "")
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            string fileName = callingMethod + subName + ".html";
            string filePath = System.IO.Path.GetFullPath(fileName);
            string html = $"<html><head></head><body>{body}</body></html>";
            System.IO.File.WriteAllText(filePath, html);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
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

            DisplayRenderInfo(callingMethod, subName, 0);
            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();

            return filePath;
        }

        private static void DisplayRenderInfo(string callingMethod, string subName, double renderTimeMs)
        {
            Console.WriteLine($"{callingMethod}() {subName}");
            Console.WriteLine($"Rendered in {renderTimeMs} ms");
        }

        public static string HashedFig(ScottPlot.Plot plt, string subName = "")
        {
            string hash = ScottPlot.Tools.BitmapHash(plt.Render(lowQuality: true));

            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            DisplayRenderInfo(callingMethod, subName, plt.GetSettings(false).BenchmarkMessage.MSec);
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
            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);

            return plt;
        }

        public static (double A, double R, double G, double B) MeanPixel(System.Drawing.Bitmap bmp)
        {
            byte[] bytes = ScottPlot.Tools.BitmapToBytes(bmp);
            int bytesPerPixel = 4;
            int pixelCount = bytes.Length / bytesPerPixel;

            double R = 0;
            double G = 0;
            double B = 0;
            double A = 0;

            for (int i = 0; i < pixelCount; i++)
            {
                B += bytes[i * bytesPerPixel + 0];
                G += bytes[i * bytesPerPixel + 1];
                R += bytes[i * bytesPerPixel + 2];
                A += bytes[i * bytesPerPixel + 3];
            }

            return (A / pixelCount, R / pixelCount, G / pixelCount, B / pixelCount);
        }

        /// <summary>
        /// Return an array containing names that appear more than once in the given array.
        /// An empty array is returned if there are no duplicates.
        /// </summary>
        public static string[] GetDuplicates(string[] names, bool ignoreCase = true)
        {
            HashSet<string> duplicates = new();
            HashSet<string> seen = new();
            foreach (string name in names)
            {
                if (seen.Add(ignoreCase ? name.ToLowerInvariant() : name))
                    duplicates.Add(name);
            }
            return duplicates.ToArray();
        }

        /// <summary>
        /// Return a string containing comma-separated labels for all visible horizontal ticks
        /// </summary>
        public static string GetXTickString(ScottPlot.Plot plt)
        {
            // enable manual tick measurement so tick density calculations are consistent across operating systems
            //plt.XAxis.TickMeasurement(manual: true);

            // perform a render to reset the layout
            plt.Layout(20, 20, 20, 20);
            plt.Render();

            // capture the ticks that were just rendered
            var limits = plt.GetAxisLimits();
            string[] labels = plt.XAxis.GetTicks(limits.XMin, limits.XMax)
                .Select(x => x.Label)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            return string.Join(", ", labels);
        }

        public static string ArrayToString(double[] values)
        {
            return string.Join(", ", values.Select(x => x.ToString()));
        }

        public static void WriteLine(double[] values, string title = null)
        {
            string prefix = string.IsNullOrWhiteSpace(title) ? "" : $"{title}: ";
            Console.WriteLine(prefix + ArrayToString(values));
        }
    }
}
