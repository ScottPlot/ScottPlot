using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[,] data = SampleData.MonaLisa();

        var hm1 = formsPlot1.Plot.Add.Heatmap(data);
        hm1.Colormap = new ScottPlot.Colormaps.Turbo();

        formsPlot1.Plot.Add.ColorBar(hm1);
    }
}
