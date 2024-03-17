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
}
