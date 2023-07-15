using FluentAssertions;
using NUnit.Framework;
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

    public static readonly double[] FirstHundredPrimes =
    {
        2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79,
        83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167,
        173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263,
        269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367,
        373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
        467, 479, 487, 491, 499, 503, 509, 521, 523, 541,
    };

    [Test]
    public void Test_FirstHundredPrimes_Mean_ShouldMatchNumpy()
    {
        // known values calculated using Python and Numpy (source file in repo dev folder)
        ScottPlot.Statistics.Descriptive.Mean(FirstHundredPrimes).Should().Be(241.33);
    }

    [Test]
    public void Test_FirstHundredPrimes_StDev_ShouldMatchNumpy()
    {
        // known values calculated using Python and Numpy (source file in repo dev folder)
        ScottPlot.Statistics.Descriptive.StDev(FirstHundredPrimes).Should().BeApproximately(160.02218939884557, precision: 1e-10);
    }
}
