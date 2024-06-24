namespace ScottPlotTests.RenderTests.Plottable;

internal class SignalTests
{
    [Test]
    public void Test_Signal_HorizontalLine()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2933
        // https://github.com/ScottPlot/ScottPlot/pull/2935

        double[] data = new double[10_000];
        double[] sin = Generate.Sin(data.Length / 10);
        Array.Copy(sin, 0, data, 0, sin.Length);
        Array.Copy(sin, 0, data, data.Length - sin.Length, sin.Length);

        ScottPlot.Plot plt = new();
        plt.Add.Signal(data);
        plt.HideGrid();
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_VerticalGap()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2933
        // https://github.com/ScottPlot/ScottPlot/pull/2935
        // https://github.com/ScottPlot/ScottPlot/issues/2949

        double[] data = Generate.SquareWave(low: 10, high: 15);
        Generate.AddNoiseInPlace(data, magnitude: .001);

        ScottPlot.Plot plt = new();
        plt.Add.Signal(data);
        plt.HideGrid();
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_Offsets()
    {
        ScottPlot.Plot plt = new();
        var sig = plt.Add.Signal(ScottPlot.Generate.Sin());
        sig.Data.XOffset = 100;
        sig.Data.YOffset = 10;
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_Scale()
    {
        ScottPlot.Plot plt = new();

        var sig = plt.Add.Signal(ScottPlot.Generate.Sin());
        sig.Data.YScale = 100;

        var sig2 = plt.Add.Signal(ScottPlot.Generate.Sin(51));
        sig2.Data.YScale = 100;
        sig2.Data.XOffset = 10;
        sig2.Data.YOffset = 50;

        plt.SaveTestImage();
    }

    [Test]
    public void Test_SignalXY_Scale()
    {
        ScottPlot.Plot plt = new();

        var sig = plt.Add.SignalXY(ScottPlot.Generate.Consecutive(51), ScottPlot.Generate.Sin(51));
        sig.Data.YScale = 100;

        var sig2 = plt.Add.SignalXY(ScottPlot.Generate.Consecutive(51), ScottPlot.Generate.Sin(51));
        sig2.Data.YScale = 100;
        sig2.Data.XOffset = 10;
        sig2.Data.YOffset = 50;

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_GenericType()
    {
        UInt16[] values = { 1, 3, 2, 4 };
        double period = 1.0;
        ScottPlot.DataSources.SignalSourceGenericArray<UInt16> source = new(values, period);

        ScottPlot.Plot plt = new();
        plt.Add.Signal(source);
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_AddGenericArray()
    {
        UInt16[] values = { 1, 3, 2, 4 };

        ScottPlot.Plot plt = new();
        plt.Add.Signal(values);
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_AddGenericList()
    {
        List<UInt16> values = new() { 1, 3, 2, 4 };

        ScottPlot.Plot plt = new();
        plt.Add.Signal(values);
        plt.SaveTestImage();
    }

    [Test]
    public void Test_SignalLowDensity_InvertedX()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(Generate.Sin());
        plt.Axes.InvertX();
        plt.SaveTestImage();
    }

    [Test]
    public void Test_SignalHighDensity_InvertedX()
    {
        ScottPlot.Plot plt = new();
        plt.Add.Signal(Generate.Sin(100_000));
        plt.Axes.InvertX();
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Signal_Empty_DoubleArray()
    {
        double[] values = [];

        ScottPlot.Plot plt = new();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Signal_Empty_GenericArray()
    {
        int[] values = [];

        ScottPlot.Plot plt = new();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_Signal_Empty_GenericList()
    {
        List<double> values = [];

        ScottPlot.Plot plt = new();
        plt.Add.Signal(values);
        plt.Should().RenderInMemoryWithoutThrowing();
    }

    [Test]
    public void Test_SignalXY_SinglePoint_OffScreen()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3926

        ScottPlot.Plot plt = new();

        // single plot with single point
        for (int i = 1; i <= 4; i++)
        {
            double[] xs = Generate.Consecutive(i);
            double[] ys = Generate.Sin(i);
            plt.Add.SignalXY(xs, ys);

            // signal plot is outside the data area
            plt.Axes.SetLimits(1, 2, 1, 2);

            plt.Should().RenderInMemoryWithoutThrowing();
        }
    }

    [Test]
    public void Test_Signal_SinglePoint_OffScreen()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/3926

        ScottPlot.Plot plt = new();

        // single plot with single point
        double[] ys = [0];
        plt.Add.Signal(ys);

        // signal plot is outside the data area
        plt.Axes.SetLimits(1, 2, 1, 2);

        plt.Should().RenderInMemoryWithoutThrowing();
    }
}
