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

    [Test]
    public void Test_MultilineRotated_TickLabels()
    {
        Plot myPlot = new();

        var sig1 = myPlot.Add.Signal(Generate.Sin(mult: 50));
        sig1.Axes.XAxis = myPlot.Axes.Bottom;
        sig1.Axes.YAxis = myPlot.Axes.Left;

        var sig2 = myPlot.Add.Signal(Generate.Cos(mult: 50));
        sig2.Axes.XAxis = myPlot.Axes.Top;
        sig2.Axes.YAxis = myPlot.Axes.Right;

        ScottPlot.TickGenerators.NumericManual tickGen = new();
        tickGen.AddMajor(25, "line one\nline two");

        myPlot.Axes.Bottom.TickGenerator = tickGen;
        myPlot.Axes.Bottom.TickLabelStyle.Rotation = 45;
        myPlot.Axes.Bottom.TickLabelStyle.FontSize = 22;
        myPlot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleLeft;
        myPlot.Axes.Bottom.MinimumSize = 150;

        myPlot.Axes.Top.TickGenerator = tickGen;
        myPlot.Axes.Top.TickLabelStyle.Rotation = -45;
        myPlot.Axes.Top.TickLabelStyle.FontSize = 22;
        myPlot.Axes.Top.TickLabelStyle.Alignment = Alignment.MiddleLeft;
        myPlot.Axes.Top.MinimumSize = 150;

        myPlot.Axes.Left.TickGenerator = tickGen;
        myPlot.Axes.Left.TickLabelStyle.Rotation = 45;
        myPlot.Axes.Left.TickLabelStyle.FontSize = 22;
        myPlot.Axes.Left.TickLabelStyle.Alignment = Alignment.MiddleRight;
        myPlot.Axes.Left.MinimumSize = 150;

        myPlot.Axes.Right.TickGenerator = tickGen;
        myPlot.Axes.Right.TickLabelStyle.Rotation = 45;
        myPlot.Axes.Right.TickLabelStyle.FontSize = 22;
        myPlot.Axes.Right.TickLabelStyle.Alignment = Alignment.MiddleLeft;
        myPlot.Axes.Right.MinimumSize = 150;

        myPlot.SaveTestImage(800, 600);
    }

    [Test]
    public void Test_Ticks_LargeFontSize()
    {
        Plot myPlot = new();
        myPlot.Axes.Bottom.TickLabelStyle.FontSize = 96;
        myPlot.SaveTestImage();
    }

    [Test]
    public void Test_Ticks_LongLabels()
    {
        Plot plt = new();
        plt.Axes.SetLimitsY(0, 1e9);
        plt.SaveTestImage();
        //plt.SaveTestImage();
    }

    [Test]
    public void Test_Ticks_Null()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3736

        Plot plt = new();

        plt.Add.Signal(Generate.Sin());
        plt.Add.Signal(Generate.Cos());

        ScottPlot.TickGenerators.NumericManual tickGen = new();
        for (int i = 1; i <= 10; i++)
            tickGen.AddMajor(i, null!);

        plt.Axes.Bottom.TickGenerator = tickGen;

        plt.Should().RenderInMemoryWithoutThrowing();
    }
}
