using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        int pointCount = 1_000_000;
        double[] ys = Generate.NoisySin(Random.Shared, pointCount);
        double[] xs = Generate.Consecutive(pointCount);

        var sp = formsPlot1.Plot.Add.ScatterGLCustom(formsPlot1, xs, ys);
        sp.LineStyle.Width = 5;
        sp.MarkerStyle = new MarkerStyle(MarkerShape.OpenSquare, 9, Colors.Red);
        sp.MarkerStyle.Outline.Width = 3;

        formsPlot1.Refresh();
    }
}
