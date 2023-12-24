using FluentAssertions;

namespace ScottPlotTests.Statistics;

internal class ArrayTests
{
    readonly double[] Sample1D = { 2, 3, 5, 7 };

    readonly double[] Sample1DWithNan = { 2, 3, double.NaN, 7 };

    readonly double[,] Sample2D =
    {
        { 2, 3, 5, 7},
        { 11, 13, 17, 19},
        { 23, 29, 31, 37}
    };

    readonly double[,] Sample2DWithNaN =
    {
        { 2, 3, double.NaN, 7},
        { 11, double.NaN, double.NaN, double.NaN},
        { 23, double.NaN, double.NaN, 37}
    };

    [Test]
    public void Test_ArrayStats1D_NanMean()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        // real of all values are real
        ScottPlot.Statistics.Descriptive.Mean(Sample1D).Should().Be(4.25);

        // NaN if any values are nan
        double.IsNaN(ScottPlot.Statistics.Descriptive.Mean(Sample1DWithNan)).Should().BeTrue();

        // Nan methods ignore NaN values
        ScottPlot.Statistics.Descriptive.NanMean(Sample1DWithNan).Should().Be(4);
    }

    [Test]
    public void Test_ArrayStats1D_NanStdev()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        // real of all values are real
        ScottPlot.Statistics.Descriptive.StandardDeviation(Sample1D).Should().BeApproximately(2.217356, 1e-5);
        ScottPlot.Statistics.Descriptive.StandardError(Sample1D).Should().BeApproximately(1.1086778913, 1e-5);

        // NaN if any values are nan
        double.IsNaN(ScottPlot.Statistics.Descriptive.StandardDeviation(Sample1DWithNan)).Should().BeTrue();
        double.IsNaN(ScottPlot.Statistics.Descriptive.StandardError(Sample1DWithNan)).Should().BeTrue();

        // Nan methods ignore NaN values
        ScottPlot.Statistics.Descriptive.NanStandardDeviation(Sample1DWithNan).Should().BeApproximately(2.645751, 1e-5);
        ScottPlot.Statistics.Descriptive.NanStandardError(Sample1DWithNan).Should().BeApproximately(1.5275252317, 1e-5);
    }

    [Test]
    public void Test_ArrayStats2D_AllReal()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        double[] expectedMeans = { 12, 15, 17.66666, 21 };
        double[] expectedStandardError = { 6.0827625303, 7.5718777944, 7.5129517797, 8.7177978871 };

        double[] vMeans = ScottPlot.Statistics.Descriptive.VerticalMean(Sample2D);
        double[] vStdErrs = ScottPlot.Statistics.Descriptive.VerticalStandardError(Sample2D);

        vMeans.Length.Should().Be(4);
        vStdErrs.Length.Should().Be(4);

        for (int i = 0; i < 4; i++)
        {
            vMeans[i].Should().BeApproximately(expectedMeans[i], 1e-5);
            vStdErrs[i].Should().BeApproximately(expectedStandardError[i], 1e-5);
        }
    }

    [Test]
    public void Test_ArrayStats2D_WithNan()
    {
        // known values calculated with https://goodcalculators.com/standard-error-calculator/

        double[] expectedMeans = { 12, 3, double.NaN, 22 };
        ScottPlot.Statistics.Descriptive.VerticalNanMean(Sample2DWithNaN)
            .Should().BeEquivalentTo(expectedMeans);

        double[] expectedStandardError = { 6.082762530298219, 0, double.NaN, 15 };
        ScottPlot.Statistics.Descriptive.VerticalNanStandardError(Sample2DWithNaN)
            .Should().BeEquivalentTo(expectedStandardError);
    }
}
