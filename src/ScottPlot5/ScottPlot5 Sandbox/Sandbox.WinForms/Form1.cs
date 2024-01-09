using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // generate sample data with gaps
        List<double> xList = new();
        List<double> yList = new();
        for (int i = 0; i < 5; i++)
        {
            xList.AddRange(Generate.Consecutive(1000, first: 2000 * i));
            yList.AddRange(Generate.RandomSample(1000));
        }
        double[] xs = xList.ToArray();
        double[] ys = yList.ToArray();

        // add a SignalXY plot
        formsPlot1.Plot.Add.SignalXY(xs, ys);
        formsPlot1.Refresh();
    }
}
