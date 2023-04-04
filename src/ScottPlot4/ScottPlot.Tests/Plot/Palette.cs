using System;
using NUnit.Framework;

namespace ScottPlotTests.Plot;

class Palette
{
    [Test]
    public void Test_Pallette_HasDefault()
    {
        var plt = new ScottPlot.Plot();
        Assert.IsNotNull(plt.Palette);
    }

    [Test]
    public void Test_Pallette_ThrowsIfSetToNull()
    {
        var plt = new ScottPlot.Plot();
        Assert.DoesNotThrow(() => { plt.Palette = ScottPlot.Palette.Category10; });
        Assert.Throws<ArgumentNullException>(() => { plt.Palette = null; });
    }

    [Test]
    public void Test_Pallette_ChangesDefaultColors()
    {
        ScottPlot.Plot plt1 = new(400, 200);
        plt1.AddSignal(ScottPlot.DataGen.Sin(51));
        plt1.AddSignal(ScottPlot.DataGen.Cos(51));

        ScottPlot.Plot plt2 = new(400, 200) { Palette = ScottPlot.Palette.Aurora };
        plt2.AddSignal(ScottPlot.DataGen.Sin(51));
        plt2.AddSignal(ScottPlot.DataGen.Cos(51));

        var mean1 = new MeanPixel(TestTools.GetLowQualityBitmap(plt1));
        var mean2 = new MeanPixel(TestTools.GetLowQualityBitmap(plt2));
        Assert.That(mean1.IsDifferentThan(mean2));
    }
}
