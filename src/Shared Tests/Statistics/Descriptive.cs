namespace SharedTests.Statistics;

public class Tests
{
    [Test]
    public void Test_Mean()
    {
        double[] values = { 1, 2, 3 };
        double mean = ScottPlot.Statistics.Descriptive.Mean(values);
        mean.Should().Be(2.0);
    }

    [Test]
    public void Test_StDev()
    {
        double[] values = { 1, 2, 3 };
        double stDev = ScottPlot.Statistics.Descriptive.StDev(values);
        stDev.Should().BeApproximately(0.81649658092773, precision: 1e-10);
    }
}