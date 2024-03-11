using ScottPlot;
using System.Diagnostics;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(51);
        double[] sin = Generate.Sin(51);
        double[] cos = Generate.Cos(51);

        formsPlot1.Plot.Add.Markers(xs, sin, MarkerShape.OpenCircle, 15, Colors.Green);
        var mk1 = formsPlot1.Plot.Add.Markers(xs, cos);
        mk1.MarkerStyle.Shape = MarkerShape.OpenSquare;
        mk1.MarkerStyle.Outline.Width = 2;
        mk1.Color = Colors.DarkOrange;
    }
}
