using NUnit.Framework;
using ScottPlotTests;
using System;
using System.Runtime.Intrinsics.X86;

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
    public void Test_Population_MarkerAlpha()
    {
        // issue #2967 - custom alpha values for population plots
        // https://github.com/ScottPlot/ScottPlot/issues/2967

        Random rand = new(0);
        double[] valuesA = DataGen.RandomNormal(rand, 35, 85, 5);
        double[] valuesB = DataGen.RandomNormal(rand, 42, 87, 3);
        double[] valuesC = DataGen.RandomNormal(rand, 23, 92, 3);

        var popA = new Statistics.Population(valuesA);
        var popB = new Statistics.Population(valuesB);
        var popC = new Statistics.Population(valuesC);

        var populations = new Statistics.Population[] { popA, popB, popC };

        ScottPlot.Plot plt = new(600, 400);
        var pplt = plt.AddPopulations(populations);
    
        // plot with default semitransparency
        var bmp1 = TestTools.GetLowQualityBitmap(plt);

        // plot with no transparency
        pplt.MarkerAlpha = 255;
        var bmp2 = TestTools.GetLowQualityBitmap(plt);

        // measure what changed
        // TestTools.SaveFig(bmp1, "1");
        // TestTools.SaveFig(bmp2, "2");
        var before = new MeanPixel(bmp1);
        var after = new MeanPixel(bmp2);
        Console.WriteLine($"Before: {before}");
        Console.WriteLine($"After: {after}");

        // less transparent markers -> darker image
        Assert.That(after.IsDarkerThan(before));
    }

    [Test]
    public void Test_Population_BoxAlphaOverride()
    {
        // issue #2967 - custom alpha values for population plots
        // https://github.com/ScottPlot/ScottPlot/issues/2967

        Random rand = new(0);
        double[] valuesA = DataGen.RandomNormal(rand, 35, 85, 5);
        double[] valuesB = DataGen.RandomNormal(rand, 42, 87, 3);
        double[] valuesC = DataGen.RandomNormal(rand, 23, 92, 3);

        var popA = new Statistics.Population(valuesA);
        var popB = new Statistics.Population(valuesB);
        var popC = new Statistics.Population(valuesC);

        var populations = new Statistics.Population[] { popA, popB, popC };

        ScottPlot.Plot plt = new(600, 400);
        var pplt = plt.AddPopulations(populations);
        
        // plot with default semitransparency
        var bmp1 = TestTools.GetLowQualityBitmap(plt);

        // plot nearly transparent boxes
        pplt.BoxAlphaOverride = 20;
        var bmp2 = TestTools.GetLowQualityBitmap(plt);

        // measure what changed
        // TestTools.SaveFig(bmp1, "1");
        // TestTools.SaveFig(bmp2, "2");
        var before = new MeanPixel(bmp1);
        var after = new MeanPixel(bmp2);
        Console.WriteLine($"Before: {before}");
        Console.WriteLine($"After: {after}");

        // more transparent boxes -> lighter image
        Assert.That(before.IsDarkerThan(after));
    }
}
