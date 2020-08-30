using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests
{
    public static class TestTools
    {
        public static void SaveFig(ScottPlot.Plot plt, string suffix = "default", int width = 600, int height = 400)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            string outFolder = System.IO.Path.GetFullPath("output");
            if (!System.IO.Directory.Exists(outFolder))
                System.IO.Directory.CreateDirectory(outFolder);

            string fileName = $"{callingMethod}_{suffix}.bmp";
            string filePath = System.IO.Path.Combine(outFolder, fileName);
            plt.SaveFig(filePath);

            Console.WriteLine($"Saved: {filePath}");
            Console.WriteLine();
        }
    }
}
