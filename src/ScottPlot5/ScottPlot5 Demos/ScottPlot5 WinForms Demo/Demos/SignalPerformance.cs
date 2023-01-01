using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SignalPerformance : Form, IDemoWindow
{
    public string Title => "Signal Plot Performance";

    public string Description => "Demonstrates how Signal plots can display " +
        "millions of points interactively at high framerates";


    public SignalPerformance()
    {
        InitializeComponent();
        Replot();
    }

    private void rbSignal_CheckedChanged(object sender, EventArgs e) => Replot();
    private void rbScatter_CheckedChanged(object sender, EventArgs e) => Replot();

    private void Replot()
    {
        formsPlot1.Plot.Clear();

        if (rbSignal.Checked)
        {
            int pointCount = 1_000_000;
            double[] ys = ScottPlot.Generate.NoisySin(new Random(), pointCount);
            formsPlot1.Plot.Add.Signal(ys);
            formsPlot1.Plot.Title.Label.Text = "one million points";
        }
        else
        {
            int pointCount = 10_000;
            double[] ys = ScottPlot.Generate.NoisySin(new Random(), pointCount);
            double[] xs = ScottPlot.Generate.Consecutive(pointCount);
            var sp = formsPlot1.Plot.Add.Scatter(xs, ys);
            sp.MarkerStyle = MarkerStyle.None;
            formsPlot1.Plot.Title.Label.Text = "ten thousand points";
        }

        formsPlot1.Plot.AutoScale();
        formsPlot1.Refresh();
    }
}
