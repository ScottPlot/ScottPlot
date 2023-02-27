using NUnit.Framework;
using System;

namespace ScottPlot.Tests.PlotTypes;

internal class Population
{
    [Test]
    public void Test_Population_Small()
    {
        // reproduces issue described by #2429
        // https://github.com/ScottPlot/ScottPlot/issues/2429

        Random rand = new(0);
        var popSeries = new ScottPlot.Statistics.PopulationSeries[10];
        for (int i = 0; i < popSeries.Length; i++)
        {
            double[] values = DataGen.RandomNormal(rand, 320);
            ScottPlot.Statistics.Population[] populations = { new(values) };
            popSeries[i] = new(populations, $"Pop {i + 1}");
        }

        var multiSeries = new ScottPlot.Statistics.PopulationMultiSeries(popSeries);

        ScottPlot.Plot plt = new(600, 400);
        plt.AddPopulations(multiSeries);
        plt.Frameless();
        plt.Render();

        plt.Resize(600, 2);
        plt.Render();
    }
}
