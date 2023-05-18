using NUnit.Framework;
using ScottPlotTests;
using System;

namespace ScottPlot.Tests.PlotTypes;

internal class BinnedHistogramTests
{
    [Test]
    public void Test_BinnedHistogram_Render()
    {
        ScottPlot.Plot plt = new();

        // create the experimental plot type and add it to the plot
        ScottPlot.Plottable.BinnedHistogram hist2d = new(100, 100);
        plt.Add(hist2d);

        // add sample data
        Coordinate[] flowData = DataGen.FlowCytometry();
        hist2d.AddRange(flowData);

        // add a colorbar
        plt.AddColorbar(hist2d.Colormap);

        TestTools.SaveFig(plt);
    }
}
