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

            TestTools.SaveFig(plt);
        }
    }
}
