namespace ScottPlot5_WinForms_Demo.Demos;

public partial class SignalPerformance : Form, IDemoForm
{
    public string Title => "Signal Plot Performance";

    public string Description => "Demonstrates how Signal plots can display " +
        "millions of points interactively at high framerates";

    public SignalPerformance()
    {
        InitializeComponent();

        double[] data = ScottPlot.Generate.NoisySin(new Random(0), 1_000_000);
        formsPlot1.Plot.Add.Signal(data);
        formsPlot1.Refresh();
    }
}
