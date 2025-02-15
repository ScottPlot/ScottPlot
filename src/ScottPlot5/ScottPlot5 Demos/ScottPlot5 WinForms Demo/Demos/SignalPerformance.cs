using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SignalPerformance : Form, IDemoWindow
{
    public string Title => "Scatter Plot, Signal Plot, and SignalConst";

    public string Description => "Demonstrates performance of Scatter plots, " +
        "Signal Plots, and SignalConst on large datasets.";

    public SignalPerformance()
    {
        InitializeComponent();

        cbPointCount.Items.Add("1,000");
        cbPointCount.Items.Add("100,000");
        cbPointCount.Items.Add("1,000,000");
        cbPointCount.Items.Add("10,000,000");
        cbPointCount.SelectedIndex = 1;

        rbScatter.CheckedChanged += (s, e) => Replot();
        rbSignal.CheckedChanged += (s, e) => Replot();
        rbSignalConst.CheckedChanged += (s, e) => Replot();
        cbPointCount.SelectedIndexChanged += (s, e) => Replot();

        Replot();
    }

    private void Replot()
    {
        formsPlot1.Plot.Clear();

        (double[] xs, double[] ys) = GetData(int.Parse(cbPointCount.Text.Replace(",", "")));

        if (rbScatter.Checked)
        {

            formsPlot1.Plot.Add.ScatterLine(xs, ys);
            formsPlot1.Plot.Title($"Scatter Plot with {ys.Length:N0} Points");
        }
        else if (rbSignal.Checked)
        {
            formsPlot1.Plot.Add.Signal(ys);
            formsPlot1.Plot.Title($"Signal Plot with {ys.Length:N0} Points");
        }
        else if (rbSignalConst.Checked)
        {
            formsPlot1.Plot.Add.SignalConst(ys);
            formsPlot1.Plot.Title($"SignalConst with {ys.Length:N0} Points");
        }

        formsPlot1.Plot.Axes.AutoScale();
        formsPlot1.Refresh();
    }

    private (double[] xs, double[] ys) GetData(int count = 1_000_000)
    {
        double[] xs = Generate.Consecutive(count);
        double[] ys = Generate.Sin(count);
        Generate.AddNoiseInPlace(ys);
        return (xs, ys);
    }
}
