namespace ScottPlot.Common.Tests;

public class Tests
{
    [Test]
    public void Test_Generate_Consecutive()
    {
        double[] values = Generate.Consecutive(10);
        values.Should().NotBeNullOrEmpty();
        values.Should().HaveCount(10);
    }

    [Test]
    public void Test_Generate_ConsecutiveGeneric()
    {
        double[] values1 = Generate.Consecutive(10);
        float[] values2 = Generate.Consecutive<float>(10);

        for (int i = 0; i < values1.Length; i++)
        {
            values1[i].Should().Be(values2[i]);
        }
    }

    [Test]
    public void Test_Generate_SinGeneric()
    {
        double[] values1 = Generate.Sin(10);
        values1.Should().NotBeNullOrEmpty();
        values1.Should().HaveCount(10);

        float[] values2 = Generate.Sin<float>(10);

        for (int i = 0; i < values1.Length; i++)
        {
            values1[i].Should().BeApproximately(values2[i], 1e-6);
        }
    }
}
