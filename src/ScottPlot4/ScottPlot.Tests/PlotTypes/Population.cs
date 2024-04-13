using NUnit.Framework;
using ScottPlotTests;
using System;
using System.Linq;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;

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

    [Test]
    public void Test_Population_ErrorBarAlignment()
    {
        // https://github.com/ScottPlot/ScottPlot/pull/2502

        Random rand = new(0);
        double[] valuesA = DataGen.RandomNormal(rand, 35, 85, 5);
        double[] valuesB = DataGen.RandomNormal(rand, 42, 87, 3);
        double[] valuesC = DataGen.RandomNormal(rand, 23, 92, 3);

        var popA = new Statistics.Population(valuesA);
        var popB = new Statistics.Population(valuesB);
        var popC = new Statistics.Population(valuesC);

        var poulations = new Statistics.Population[] { popA, popB, popC };

        ScottPlot.Plot plt = new(600, 400);
        var pop = plt.AddPopulations(poulations);
        pop.ErrorBarAlignment = HorizontalAlignment.Center;
        TestTools.SaveFig(plt);
    }

    [Test]
    public void Test_Population_Empty()
    {
        // reproduces issue where empty populations crash during render
        // https://github.com/ScottPlot/ScottPlot/issues/2727

        Random rand = new(0);
        double[] valuesA = DataGen.RandomNormal(rand, 35, 85, 5);
        double[] valuesB = { };
        double[] valuesC = DataGen.RandomNormal(rand, 23, 92, 3);

        var popA = new Statistics.Population(valuesA);
        var popB = new Statistics.Population(valuesB);
        var popC = new Statistics.Population(valuesC);

        var poulations = new Statistics.Population[] { popA, popB, popC };

        ScottPlot.Plot plt = new();
        plt.AddPopulations(poulations);
        Assert.DoesNotThrow(() => plt.Render());
    }

    [Test]
    public void Test_Population_CustomOpacity()
    {
        // issue #2967 - custom alpha values for population plots
        // https://github.com/ScottPlot/ScottPlot/issues/2967

        // Redscale: the argbs contain different alpha values.
        var seriesColors = new Color[] {
        Color.FromArgb(225, 255, 0, 0),
        Color.FromArgb(175, 255, 0, 0),
        Color.FromArgb(125, 255, 0, 0),
        Color.FromArgb(75, 255, 0, 0),
        Color.FromArgb(25, 255, 0, 0)};

        // 3 groups
        var groupNames = new[] { "Group A", "Group B", "Group C" };

        // Get some data
        var seriesData = GetSeriesData(
            seriesCount: seriesColors.Length,
            groupCount: groupNames.Length);

        Plot plt = new(600, 400);

        plt.XTicks(groupNames);
        plt.Legend();

        var populations = seriesData.Select(series => series.Select(seriesData => new Statistics.Population(seriesData)).ToArray()).ToArray();
        var populationSeries = populations.Select((p, i) => new Statistics.PopulationSeries(p, seriesLabel: $"Series {i + 1}")).ToArray();
        var populationMultiSeries = new Statistics.PopulationMultiSeries(populationSeries.ToArray());
        var populationPlot = plt.AddPopulations(populationMultiSeries);

        // Set the colours
        for (var i = 0; i < populationPlot.MultiSeries.seriesCount; i++)
        {
            populationPlot.MultiSeries.multiSeries[i].color = seriesColors[i];
        }

        var bmp1 = TestTools.GetLowQualityBitmap(plt);
        // Turn off automatic opacity - colors should now match seriesColors alpha values
        populationPlot.AutomaticOpacity = false;
        var bmp2 = TestTools.GetLowQualityBitmap(plt);

        var before = new MeanPixel(bmp1);
        var after = new MeanPixel(bmp2);
        // more transparent red -> lighter image
        Assert.That(before.IsDarkerThan(after));

        // Use fraction to make markers half the opacity of the boxes
        populationPlot.MarkerOpacityRatio = 0.5;
        var bmp3 = TestTools.GetLowQualityBitmap(plt);

        before = new MeanPixel(bmp2);
        after = new MeanPixel(bmp3);
        // more transparent red -> lighter image
        Assert.That(before.IsDarkerThan(after));

        // Turn Automatic Opacity back on - MarkerOpacityRatio should no longer have an effect
        populationPlot.AutomaticOpacity = true;
        var bmp4 = TestTools.GetLowQualityBitmap(plt);

        before = new MeanPixel(bmp1);
        after = new MeanPixel(bmp4);
        // Should be back to default values
        Assert.That(before.IsEqualTo(after));

        // TestTools.SaveFig(bmp1, "1");
        // TestTools.SaveFig(bmp2, "2");
        // TestTools.SaveFig(bmp3, "3");
        // TestTools.SaveFig(bmp4, "4");

        double[][][] GetSeriesData(int seriesCount, int groupCount)
        {
            return Enumerable.Range(1, seriesCount)
                .Select(si => Enumerable.Range(1, groupCount)
                    .Select(gi => Enumerable.Range(gi + si, 3).Select(x => Convert.ToDouble(x)).ToArray()).ToArray()).ToArray();
        }
    }
}
