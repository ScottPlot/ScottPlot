using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class OS
    {
        [Test]
        public void Test_Plot_WithOsInfo()
        {
            string osName = "unknown OS";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                osName = $"Linux ({System.Environment.OSVersion})";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                osName = $"MacOS ({System.Environment.OSVersion})";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                osName = $"Windows ({System.Environment.OSVersion})";

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(ScottPlot.DataGen.Sin(100), label: "sin");
            plt.PlotSignal(ScottPlot.DataGen.Cos(100), label: "cos");
            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");
            plt.Title(osName);
            plt.Legend();

            string osNameShort = osName.Split(" ")[0];
            TestTools.SaveFig(plt, osNameShort);

            string artifactFolder = System.IO.Path.GetFullPath("./artifacts/");
            if (!System.IO.Directory.Exists(artifactFolder))
                System.IO.Directory.CreateDirectory(artifactFolder);
            string artifactFilePath = System.IO.Path.Combine(artifactFolder, $"SamplePlot_{osNameShort}.png");
            plt.SaveFig(artifactFilePath);
            Console.WriteLine($"Saved artifact: {artifactFilePath}");
        }
    }
}
