namespace ScottPlotTests.Statistics;

/* known values obtained from an online calculator:
 * https://www.calculatorsoup.com/calculators/statistics/variance-calculator.php 
 */

public class DescriptiveTests
{
    [Test]
    public void Test_Sum_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .Sum(SampleData.FirstHundredPrimes)
            .Should().Be(24133);
    }

    [Test]
    public void Test_Mean_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .Mean(SampleData.FirstHundredPrimes)
            .Should().Be(241.33);
    }

    [Test]
    public void Test_Variance_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .Variance(SampleData.FirstHundredPrimes)
            .Should().BeApproximately(25865.759, 1e-3);
    }

    [Test]
    public void Test_VarianceP_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .VarianceP(SampleData.FirstHundredPrimes)
            .Should().BeApproximately(25607.101, 1e-3);
    }

    [Test]
    public void Test_StandardDeviation_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .StandardDeviation(SampleData.FirstHundredPrimes)
            .Should().BeApproximately(160.82835, precision: 1e-5);
    }

    [Test]
    public void Test_StandardDeviationP_MatchesKnownValue()
    {
        ScottPlot.Statistics.Descriptive
            .StandardDeviationP(SampleData.FirstHundredPrimes)
            .Should().BeApproximately(160.02219, precision: 1e-5);
    }
}
