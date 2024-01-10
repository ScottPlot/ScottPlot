namespace ScottPlotTests.RenderTests;

internal class MultiAxis_Tests
{
    [Test]
    public void Test_MultiAxis_Memory()
    {
        Plot myPlot = new();

        myPlot.Add.Signal(Generate.Sin(51, mult: 0.01));

        var sig2 = myPlot.Add.Signal(Generate.Cos(51, mult: 100));
        sig2.Axes.YAxis = myPlot.Axes.AddLeftAxis();

        myPlot.SaveTestImage();

        myPlot.LastRender.AxisLimitsByAxis.Should().NotBeNullOrEmpty();
        myPlot.LastRender.AxisLimitsByAxis.Count.Should().Be(5);

        foreach ((IAxis axis, CoordinateRange range) in myPlot.LastRender.AxisLimitsByAxis)
        {
            Console.WriteLine($"{axis} {range}");
        }
    }
}
