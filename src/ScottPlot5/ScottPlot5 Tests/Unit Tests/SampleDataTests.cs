namespace SharedTests;

internal class SampleData
{
    [Test]
    public void Test_SampleData_Primes()
    {
        ScottPlot.SampleData.FirstHundredPrimes.Should().NotBeNull();
        ScottPlot.SampleData.FirstHundredPrimes.Should().NotBeEmpty();
        ScottPlot.SampleData.FirstHundredPrimes.Should().HaveCount(100);
        ScottPlot.SampleData.FirstHundredPrimes.Should().HaveElementAt(42 - 1, 181);
    }
}
