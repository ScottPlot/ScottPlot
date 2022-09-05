namespace ScottPlot5_WinForms_Demo.Demos;

internal class SignalPerformance : Form, IDemoForm
{
    readonly ScottPlot.WinForms.FormsPlot formsPlot1;

    public string Title => "Signal Plot Performance";

    public string Description => "Demonstrates how Signal plots can display " +
        "millions of points interactively at high framerates";

    public SignalPerformance()
    {
        Width = 800;
        Height = 600;
        Text = "Signal Plot with one million points";

        formsPlot1 = new() { Dock = DockStyle.Fill };
        Controls.Add(formsPlot1);

        double[] data = ScottPlot.Generate.NoisySin(new Random(0), 1_000_000);
        formsPlot1.Plot.Plottables.AddSignal(data);
        formsPlot1.Refresh();
    }
}
