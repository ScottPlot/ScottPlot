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

        // share the X axis for all subplots
        formsPlot1.Multiplot.ShareX(subplots);

        // share the Y axis for select subplots
        formsPlot1.Multiplot.ShareY([subplots[0], subplots[1]]);

        // used manual padding to ensure subplot alignment
        PixelPadding padding = new(50, 10, 25, 45);
        formsPlot1.Multiplot.ApplyFixedPadding(padding);
    }
}
