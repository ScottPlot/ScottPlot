using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Plot[] subplots = formsPlot1.Multiplot.AddPlots(4);

        for (int i = 0; i < subplots.Length; i++)
        {
            double scale = Math.Pow(10, i);
            double[] data = Generate.RandomWalk(100, scale*2);
            subplots[i].Add.Signal(data);
            subplots[i].Title($"Subplot {i + 1}");
        }

        formsPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 2);
    }
}
