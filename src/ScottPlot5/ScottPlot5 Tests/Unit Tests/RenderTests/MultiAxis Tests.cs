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

    [Test]
    public void Test_MultiAxis_Remove()
    {
        Plot myPlot = new();

        myPlot.Add.Signal(Generate.Sin(51, mult: 10));
        var sig2 = myPlot.Add.Signal(Generate.Cos(51, mult: 1));

        // create an additional axis and setup the second signal to use it
        IYAxis secondYAxis = myPlot.Axes.AddLeftAxis();
        sig2.Axes.YAxis = secondYAxis;
        myPlot.Should().SavePngWithoutThrowing("1");

        // remove the additional axis
        myPlot.Axes.Remove(secondYAxis);

        // tell the signal plot to use the original axis
        sig2.Axes.YAxis = myPlot.Axes.Left;
        myPlot.Should().SavePngWithoutThrowing("2");
    }

    [Test]
    public void Test_RightAxis_NoLeftAxis()
    {
        Plot myPlot = new();

        var sig = myPlot.Add.Signal(Generate.Sin());
        sig.Axes.YAxis = myPlot.Axes.Right;

        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeFalse();

        myPlot.Should().SavePngWithoutThrowing();
        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeTrue();

        myPlot.Should().SavePngWithoutThrowing();
        myPlot.Axes.Left.Range.HasBeenSet.Should().BeFalse();
        myPlot.Axes.Right.Range.HasBeenSet.Should().BeTrue();
    }
}
