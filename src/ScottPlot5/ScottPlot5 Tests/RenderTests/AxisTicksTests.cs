namespace ScottPlotTests.RenderTests;

internal class AxisTicksTests
{
    [Test]
    public void Test_DateTimeTicks_Bottom()
    {
        Plot plt = new();
        plt.BottomAxis.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();

        DateTime dt1 = new(2023, 01, 01);
        DateTime dt2 = new(2024, 01, 01);
        plt.SetAxisLimitsX(dt1.ToOADate(), dt2.ToOADate());

        plt.SaveTestImage();
    }

    [Test]
    public void Test_DateTimeTicks_Left()
    {
        Plot plt = new();

        plt.LeftAxis.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();

        DateTime dt1 = new(2023, 01, 01);
        DateTime dt2 = new(2024, 01, 01);
        plt.SetAxisLimitsY(dt1.ToOADate(), dt2.ToOADate());

        plt.SaveTestImage();
    }
}
