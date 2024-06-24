namespace ScottPlotTests.Statistics;

internal class TTestTests
{

    [Test]
    //[TestCase(2, 0.6839986556042016, 0.564592904143043)] // require different algorithm
    //[TestCase(3, 1.2737845484273527, 0.2717212823478658)] // require different algorithm
    [TestCase(4, 1.072428971670766, 0.32475536384592874)]
    [TestCase(5, 1.7133674270900519, 0.12499632369287829)]
    [TestCase(6, 1.2367926723027869, 0.24442223269988042)]
    [TestCase(7, 1.0727726053204316, 0.3044732791357301)]
    [TestCase(8, 0.9002146452562914, 0.38323002917609694)]
    [TestCase(9, 1.261754116953713, 0.2251219935000761)]
    [TestCase(10, 0.9996992098406023, 0.3307065660247912)]
    [TestCase(11, 0.8172433376842712, 0.4234184287962507)]
    [TestCase(12, 1.0572301203168777, 0.3018780762191577)]
    [TestCase(13, 1.337270069235878, 0.1936715043753276)]
    [TestCase(14, 1.0390652822828303, 0.30834135755575226)]
    [TestCase(15, 0.7201094247712603, 0.4774263688268924)]
    public void Test_Statistics_UnpairedTTest(int n, double expectedT, double expectedP)
    {
        /* Tested with Python:
         
        import scipy.stats as stats

        sample1 = [71, 99, 67, 34, 79, 22, 16, 36, 73, 22, 26, 72, 87, 58, 54]
        sample2 = [ 91, 38, 2, 43, 19, 52, 22, 54, 30, 48, 46, 41, 46, 88, 92]

        for i in range(len(sample1)):
            t, p = stats.ttest_ind(sample1[:i+1], sample2[:i+1])
            print(f"[TestCase({i+1},{t},{p})]")
        */

        double[] sample1 = [71, 99, 67, 34, 79, 22, 16, 36, 73, 22, 26, 72, 87, 58, 54];
        double[] sample2 = [91, 38, 2, 43, 19, 52, 22, 54, 30, 48, 46, 41, 46, 88, 92];

        Population pop1 = new(sample1[..n]);
        Population pop2 = new(sample2[..n]);
        pop1.Count.Should().Be(n);
        pop2.Count.Should().Be(n);

        ScottPlot.Statistics.Tests.UnpairedTTest utt = new(pop1, pop2);
        utt.T.Should().BeApproximately(expectedT, precision: 1e-6);
        utt.P.Should().BeApproximately(expectedP, precision: 1e-6);
    }
}
