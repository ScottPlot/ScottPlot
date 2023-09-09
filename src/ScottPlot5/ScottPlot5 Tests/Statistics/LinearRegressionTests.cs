using FluentAssertions;

namespace ScottPlotTests.Statistics;

internal class LinearRegressionTests
{
    [Test]
    public void Test_LinearRegression_MatchesExcel()
    {
        double[] x = new double[] { 1, 2, 3, 4, 5, 6, 7 };
        double[] y = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };
        ScottPlot.Statistics.LinearRegression reg = new (x, y);
        reg.Slope.Should().BeApproximately(0.4, precision: 0.0001);
        reg.Offset.Should().BeApproximately(1.5429, precision: 0.0001);
        reg.Rsquared.Should().BeApproximately(0.9074, precision: 0.0001);
    }
}
