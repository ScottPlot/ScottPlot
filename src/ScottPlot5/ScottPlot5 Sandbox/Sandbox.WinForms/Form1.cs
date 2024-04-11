using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Func<double, double> func = new((x) => Math.Sin(x) * Math.Sin(x / 2));

        var f = formsPlot1.Plot.Add.Function(func);
        // f.MinX = -3;
        // f.MaxX = 3;
    }
}