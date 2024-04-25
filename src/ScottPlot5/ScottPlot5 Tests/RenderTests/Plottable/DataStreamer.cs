namespace ScottPlotTests.RenderTests.Plottable;

internal class DataStreamer
{
    [Test]
    public void Test_DataStreamer_ExpandToFitData()
    {
        Plot plt = new();

        var streamer = plt.Add.DataStreamer(10);
        streamer.GetAxisLimits().Top.Should().Be(double.NaN);

        streamer.Add(111);
        streamer.GetAxisLimits().Top.Should().Be(111);
        streamer.GetAxisLimits().Bottom.Should().Be(111);

        streamer.Add(999);
        streamer.GetAxisLimits().Top.Should().Be(999);
        streamer.GetAxisLimits().Bottom.Should().Be(111);

        for (int i = 0; i < streamer.Count; i++)
        {
            streamer.Add(333);
        }

        streamer.GetAxisLimits().Top.Should().Be(999);
        streamer.GetAxisLimits().Bottom.Should().Be(111);

        streamer.ContinuouslyAutoscale = true;
        streamer.GetAxisLimits().Top.Should().Be(333);
        streamer.GetAxisLimits().Bottom.Should().Be(333);
    }
}
