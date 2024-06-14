namespace ScottPlotTests.UnitTests;

internal class PopulationTests
{
    [Test]
    public void Test_Population_Mean()
    {
        Population pop = new(SampleData.FirstHundredPrimes);
        pop.Count.Should().Be(100);
        pop.Mean.Should().Be(241.33);
        pop.Variance.Should().BeApproximately(25865.759, precision: 1e-3);
        pop.StandardDeviation.Should().BeApproximately(160.82835, precision: 1e-5);
        pop.StandardError.Should().BeApproximately(16.082835, precision: 1e-5);
        pop.Median.Should().Be(231);
    }

    [Test]
    public void Test_Statistics_UnpairedTTest()
    {
        double[] arr1 = { 10, 20, 30, 40, 50 };
        double[] arr2 = { 1, 29, 46, 78, 99 };

        Population pop1 = new(arr1);
        Population pop2 = new(arr2);

        ScottPlot.Statistics.Tests.UnpairedTTest utt = new(pop1, pop2);
        utt.DF.Should().Be(8);
        utt.T.Should().BeApproximately(-1.09789, precision: 1e-5);
        utt.P.Should().BeApproximately(0.3042, precision: 1e-4);
    }
}
