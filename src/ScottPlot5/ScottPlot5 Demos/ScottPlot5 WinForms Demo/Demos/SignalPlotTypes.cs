using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SignalPlotTypes : Form, IDemoWindow
{
    public string Title => "Signal Plot Types";

    public string Description => "Signal plots display evenly-spaced points, " +
        "but different types of signal plots may offer superior performance in some situations.";

    public SignalPlotTypes()
    {
        InitializeComponent();

        double[] data = Generate.RandomWalk(10_000_000);
        SetupSignal(formsPlot1, data);
        SetupSignalConstGeneric(formsPlot2, data);
        SetupSignalConstDouble(formsPlot3, data);
        SetupSignalBinned(formsPlot4, data);
    }

    private static void SetupSignal(IPlotControl formsPlot, double[] data)
    {
        formsPlot.Plot.Title("Signal with 10 million points");
        formsPlot.Plot.Benchmark.IsVisible = true;
        formsPlot.Plot.Add.Signal(data);
    }

    private static void SetupSignalConstGeneric(IPlotControl formsPlot, double[] data)
    {
        formsPlot.Plot.Title("SignalConst (Generic) with 10 million points");
        formsPlot.Plot.Benchmark.IsVisible = true;
        formsPlot.Plot.Add.SignalConst<double>(data);
    }

    private static void SetupSignalConstDouble(IPlotControl formsPlot, double[] data)
    {
        formsPlot.Plot.Title("SignalConst (double) with 10 million points");
        formsPlot.Plot.Benchmark.IsVisible = true;
        formsPlot.Plot.Add.SignalConst(data);
    }

    private static void SetupSignalBinned(IPlotControl formsPlot, double[] data)
    {
        formsPlot.Plot.Title("Binned signal (1k bin size) with 10 million points");
        formsPlot.Plot.Benchmark.IsVisible = true;

        ScottPlot.DataSources.FastSignalSourceDouble source = new(data, period: 1, cachePeriod: 1000);
        formsPlot.Plot.Add.Signal(source);
    }
}
