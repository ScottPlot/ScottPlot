using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace SharedTests.Statistics;

internal class HistogramTests
{
    [Test]
    public void Test_Histogram_IgnoringOutliers()
    {
        ScottPlot.Statistics.Histogram hist = new(min: 100, max: 200, binCount: 5, addOutliersToEdgeBins: false, addFinalBin: false);

        hist.Min.Should().Be(100);
        hist.Max.Should().Be(200);
        hist.Bins.First().Should().Be(100);
        hist.Bins.Last().Should().Be(180);

        hist.Bins.Should().BeEquivalentTo(new double[] { 100, 120, 140, 160, 180 });
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 0, 0 });

        hist.Add(173);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 1, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 2, 0, 1, 0 });

        hist.Sum.Should().Be(123 + 173 + 123);

        hist.Clear();
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Sum.Should().Be(0);
    }

    [Test]
    public void Test_Histogram_IncludingOutliers()
    {
        ScottPlot.Statistics.Histogram hist = new(min: 100, max: 200, binCount: 5, addOutliersToEdgeBins: true, addFinalBin: false);

        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 1 });

        hist.Sum.Should().Be(50 + 250);
    }

    [Test]
    public void Test_Histogram_Normalization()
    {
        ScottPlot.Statistics.Histogram hist = new(min: 100, max: 200, binCount: 5, addFinalBin: false);

        hist.Add(125);
        hist.Add(145);
        hist.Add(145);
        hist.Add(165);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 2, 1, 0 });

        hist.GetProbability().Should().BeEquivalentTo(new double[] { 0, .25, .5, .25, 0 });

        hist.GetNormalized().Should().BeEquivalentTo(new double[] { 0, .5, 1, .5, 0 });

        hist.GetNormalized(256).Should().BeEquivalentTo(new double[] { 0, 128, 256, 128, 0 });
    }

    [Test]
    public void Test_Histogram_CPH()
    {
        ScottPlot.Statistics.Histogram hist = new(min: 100, max: 200, binCount: 5, addFinalBin: false);

        hist.Add(125);
        hist.Add(145);
        hist.Add(145);
        hist.Add(165);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 2, 1, 0 });

        hist.GetCumulative().Should().BeEquivalentTo(new double[] { 0, 1, 3, 4, 4 });

        hist.GetCumulativeProbability().Should().BeEquivalentTo(new double[] { 0, .25, .75, 1, 1 });
    }

    [Test]
    public void Test_Histogram_FixedBinSize()
    {
        // Extending conversation in #2403, this test confirms bins meet expectations
        // https://github.com/ScottPlot/ScottPlot/issues/2403

        var hist1 = ScottPlot.Statistics.Histogram.WithFixedBinSize(min: 0, max: 10, binSize: 1);

        hist1.BinSize.Should().Be(1);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10); // since bins are max-exclusive, this counts as an outlier
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void Test_Histogram_FixedBinCount()
    {
        // Extending conversation in #2403, this test confirms bins meet expectations
        // https://github.com/ScottPlot/ScottPlot/issues/2403

        var hist1 = ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 0, max: 10, binCount: 10);

        hist1.BinSize.Should().Be(1);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10); // since bins are max-exclusive, this counts as an outlier
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void Test_Histogram_FractionalBinSize()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2490

        var hist1 = ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 0, max: 1, binCount: 10);

        hist1.BinSize.Should().Be(0.1);

        double[] expectedBins = new double[] { 0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
        for (int i = 0; i < expectedBins.Length; i++)
        {
            hist1.Bins[i].Should().BeApproximately(expectedBins[i], 1e-10);
        }
    }

    [Test]
    public void Test_Histogram_MinMaxValidation()
    {
        FluentActions
            .Invoking(() => ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 0, max: 1, binCount: 1))
            .Should()
            .NotThrow();

        FluentActions
            .Invoking(() => ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 1, max: 0, binCount: 10))
            .Should()
            .Throw<ArgumentException>();

        FluentActions
            .Invoking(() => ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 1, max: 1, binCount: 10))
            .Should()
            .Throw<ArgumentException>();

        FluentActions
            .Invoking(() => ScottPlot.Statistics.Histogram.WithFixedBinCount(min: 0, max: 1, binCount: 0))
            .Should()
            .Throw<ArgumentException>();
    }
}
