using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var sp1 = formsPlot1.Plot.Add.Signal(Generate.Sin(51));
        sp1.Label = "Sin";

        var sp2 = formsPlot1.Plot.Add.Signal(Generate.Cos(51));
        sp2.Label = "Cos";

        formsPlot1.Refresh();
    }
}
