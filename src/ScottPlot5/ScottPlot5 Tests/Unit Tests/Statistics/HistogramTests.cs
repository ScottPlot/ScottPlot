namespace ScottPlotTests.Statistics;

internal class HistogramTests
{
    [Test]
    public void Test_Histogram_IgnoringOutliers()
    {
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(5, 100, 200);
        hist.IgnoreOutliers = true;

        hist.Bins.Should().BeEquivalentTo(new double[] { 100, 120, 140, 160, 180 });
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 0, 0 });

        hist.Add(173);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 0, 1, 0 });

        hist.Add(123);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 2, 0, 1, 0 });

        hist.Clear();
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });
    }

    [Test]
    public void Test_Histogram_IncludingOutliers()
    {
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(5, 100, 200);
        hist.IgnoreOutliers = false;

        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(50);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 0 });

        hist.Add(250);
        hist.Counts.Should().BeEquivalentTo(new double[] { 1, 0, 0, 0, 1 });
    }

    [Test]
    public void Test_Histogram_Normalization()
    {
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(5, 100, 200);

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
        var hist = ScottPlot.Statistics.Histogram.WithBinCount(5, 100, 200);
        hist.Bins.Should().BeEquivalentTo(new double[] { 100, 120, 140, 160, 180 });
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0 });

        hist.Add(125);
        hist.Add(145);
        hist.Add(145);
        hist.Add(165);
        hist.Counts.Should().BeEquivalentTo(new double[] { 0, 1, 2, 1, 0 });

        hist.GetCumulativeCounts().Should().BeEquivalentTo(new double[] { 0, 1, 3, 4, 4 });

        hist.GetCumulativeProbability().Should().BeEquivalentTo(new double[] { 0, .25, .75, 1, 1 });
    }

    [Test]
    public void Test_Histogram_FixedBinSize()
    {
        var hist1 = ScottPlot.Statistics.Histogram.WithBinSize(1, 0, 10);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10);
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void Test_Histogram_FixedBinCount()
    {
        var hist1 = ScottPlot.Statistics.Histogram.WithBinCount(10, 0, 10);

        hist1.Bins.Should().BeEquivalentTo(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        hist1.Add(10);
        hist1.Counts.Should().BeEquivalentTo(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    [Test]
    public void Test_Histogram_FractionalBinSize()
    {
        var hist1 = ScottPlot.Statistics.Histogram.WithBinCount(10, 0, 1);

        double[] expectedBins = { 0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 };
        for (int i = 0; i < expectedBins.Length; i++)
        {
            hist1.Bins[i].Should().BeApproximately(expectedBins[i], 1e-10);
        }
    }
}
