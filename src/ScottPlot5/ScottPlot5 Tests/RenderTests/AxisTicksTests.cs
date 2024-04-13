namespace ScottPlotTests.RenderTests;

internal class AxisTicksTests
{
    [Test]
    public void Test_DateTimeTicks_Bottom()
    {
        Plot plt = new();
        plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();

        DateTime dt1 = new(2023, 01, 01);
        DateTime dt2 = new(2024, 01, 01);
        plt.Axes.SetLimitsX(dt1.ToOADate(), dt2.ToOADate());

        plt.SaveTestImage();
    }

    [Test]
    public void Test_DateTimeTicks_Left()
    {
        Plot plt = new();

        plt.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.DateTimeAutomatic();

        DateTime dt1 = new(2023, 01, 01);
        DateTime dt2 = new(2024, 01, 01);
        plt.Axes.SetLimitsY(dt1.ToOADate(), dt2.ToOADate());

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Multiline_TickLabels()
    {
        Plot plt = new();

        plt.Add.Signal(Generate.Sin());
        plt.Add.Signal(Generate.Cos());

        ScottPlot.TickGenerators.NumericManual yTicker = new();
        yTicker.AddMajor(-.5, "one line");
        yTicker.AddMajor(+.5, "two\nlines");
        yTicker.AddMinor(0);

        ScottPlot.TickGenerators.NumericManual xTicker = new();
        xTicker.AddMajor(20, "one line");
        xTicker.AddMajor(40, "two\nlines");
        xTicker.AddMinor(10);

        plt.Axes.Left.TickGenerator = yTicker;
        plt.Axes.Bottom.TickGenerator = xTicker;

        plt.SaveTestImage();
    }
}
