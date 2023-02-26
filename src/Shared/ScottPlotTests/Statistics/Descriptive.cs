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

    [Test]
    public void Test_FirstHundredPrimes_Mean_ShouldMatchNumpy()
    {
        // known values calculated using Python and Numpy (source file in repo dev folder)
        ScottPlot.Statistics.Descriptive.Mean(ScottPlot.SampleData.FirstHundredPrimes).Should().Be(241.33);
    }

    [Test]
    public void Test_FirstHundredPrimes_StDev_ShouldMatchNumpy()
    {
        // known values calculated using Python and Numpy (source file in repo dev folder)
        ScottPlot.Statistics.Descriptive.StDev(ScottPlot.SampleData.FirstHundredPrimes).Should().BeApproximately(160.02218939884557, precision: 1e-10);
    }
}