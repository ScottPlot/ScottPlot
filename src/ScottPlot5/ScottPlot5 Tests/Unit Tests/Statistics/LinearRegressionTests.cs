using FluentAssertions;

namespace ScottPlotTests.Statistics;

internal class LinearRegressionTests
{
    [Test]
    public void Test_LinearRegression_MatchesExcel()
    {
        double[] xs = new double[] { 1, 2, 3, 4, 5, 6, 7 };
        double[] ys = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };
        ScottPlot.Statistics.LinearRegression reg = new(xs, ys);
        reg.Slope.Should().BeApproximately(0.4, precision: 0.0001);
        reg.Offset.Should().BeApproximately(1.5429, precision: 0.0001);
        reg.Rsquared.Should().BeApproximately(0.9074, precision: 0.0001);
    }

    [Test]
    public void Test_LinearRegression_Coordinates()
    {
        Coordinates[] coordinates =
        {
            new Coordinates(1, 2),
            new Coordinates(2, 2),
            new Coordinates(3, 3),
            new Coordinates(4, 3),
            new Coordinates(5, 3.8),
            new Coordinates(6, 4.2),
            new Coordinates(7, 4),
        };

        ScottPlot.Statistics.LinearRegression reg = new(coordinates);
        reg.Slope.Should().BeApproximately(0.4, precision: 0.0001);
        reg.Offset.Should().BeApproximately(1.5429, precision: 0.0001);
        reg.Rsquared.Should().BeApproximately(0.9074, precision: 0.0001);
    }

    [Test]
    public void Test_LinearRegression_YsOnly()
    {
        double[] ys = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };
        ScottPlot.Statistics.LinearRegression reg = new(ys, firstX: 1, xSpacing: 1);
        reg.Slope.Should().BeApproximately(0.4, precision: 0.0001);
        reg.Offset.Should().BeApproximately(1.5429, precision: 0.0001);
        reg.Rsquared.Should().BeApproximately(0.9074, precision: 0.0001);
    }
}
