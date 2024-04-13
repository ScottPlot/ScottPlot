namespace ScottPlotTests.UnitTests;

internal class SignalDataTests
{
    [Test]
    public void Test_Signal_DataLimits()
    {
        double[] data = Generate.Zeros(51);
        ScottPlot.DataSources.SignalSourceDouble ss = new(data, period: 1);

        AxisLimits limits = ss.GetLimits();
        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().Be(0);
        limits.Top.Should().Be(0);

        data[5] = -2;
        data[6] = 3;
        limits = ss.GetLimits();
        limits.Left.Should().Be(0);
        limits.Right.Should().Be(50);
        limits.Bottom.Should().Be(-2);
        limits.Top.Should().Be(3);

        ss.XOffset = 0.3;
        ss.YOffset = 0.5;
        limits = ss.GetLimits();
        limits.Left.Should().Be(0.3);
        limits.Right.Should().Be(50.3);
        limits.Bottom.Should().Be(-1.5);
        limits.Top.Should().Be(3.5);
    }
}
