using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        multiFormsPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 3);

        RandomDataGenerator gen = new();

        for (int i = 0; i < 6; i++)
        {
            Plot plot = new();
            plot.Add.Signal(gen.RandomWalk(100));
            plot.Title($"Plot {i + 1}");
            multiFormsPlot1.Multiplot.Add(plot);
        }

        // make all plots match layout of the first
        multiFormsPlot1.Multiplot.SharedLayoutSourcePlot = multiFormsPlot1.Multiplot.GetPlots().First();
    }
}
