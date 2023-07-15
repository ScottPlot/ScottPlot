using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SignalPerformance : Form, IDemoWindow
{
    public string Title => "ScottPlot Performance";

    public string Description => "Demonstrates how Signal plots and " +
        "OpenGL-accelerated Scatter plots can display " +
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
        label1.Text = "Generating random data...";
        Application.DoEvents();

        int pointCount = 1_000_000;
        double[] ys = ScottPlot.Generate.NoisySin(new Random(), pointCount);
        double[] xs = ScottPlot.Generate.Consecutive(pointCount);

        if (rbSignal.Checked)
        {
            formsPlot1.Plot.Add.Signal(ys);
            formsPlot1.Plot.TitlePanel.Label.Text = $"Signal Plot with {ys.Length:N0} Points";
            label1.Text = "Signal plots are very performant for large datasets";
        }
        else if (rbScatter.Checked)
        {
            var sp = formsPlot1.Plot.Add.Scatter(xs, ys);
            formsPlot1.Plot.TitlePanel.Label.Text = $"Scatter Plot with {ys.Length:N0} Points";
            sp.MarkerStyle = MarkerStyle.None;
            label1.Text = "Traditional Scatter plots are not performant for large datasets";
        }
        else if (rbScatterGL.Checked)
        {
            var spGL = formsPlot1.Plot.Add.ScatterGL(formsPlot1, xs, ys);
            formsPlot1.Plot.TitlePanel.Label.Text = $"OpenGL Scatter Plot with {ys.Length:N0} Points";
            spGL.MarkerStyle = MarkerStyle.None;
            label1.Text = "OpenGL accelerated scatter plots are very performant even for large datasets";
        }

        formsPlot1.Plot.AutoScale();
        formsPlot1.Refresh();
    }
}
