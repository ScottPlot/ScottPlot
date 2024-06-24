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
}
