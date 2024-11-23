using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[,] data = ScottPlot.SampleData.MonaLisa();
        var hm = formsPlot1.Plot.Add.Heatmap(data);
        hm.Extent = new(-3500, 3500, -3500, 3500);
    }
}
