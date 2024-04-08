using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Func<double, double> func = new((x) => Math.Sin(x) * Math.Sin(x / 2));

        ScottPlot.DataSources.FunctionSource fs = new(x => x)
        {
            RangeX = new CoordinateRange(0, Math.PI * 2),
            Function = func,
        };

        var f = formsPlot1.Plot.Add.Function(fs);
    }
}
