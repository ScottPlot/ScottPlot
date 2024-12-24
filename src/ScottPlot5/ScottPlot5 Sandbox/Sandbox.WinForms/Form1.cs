using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // work with the plot like normal
        formsPlot1.Plot.Add.Signal(Generate.Sin());

        // add a sub-plot and interact with it
        var plot2 = formsPlot1.Multiplot.AddPlot();
        plot2.Add.Signal(Generate.Cos());

        // add another sub-plot
        var plot3 = formsPlot1.Multiplot.AddPlot();
        plot3.Add.Signal(Generate.AddNoise(Generate.Sin()));

        // use a custom layout
        formsPlot1.Multiplot.PositionedPlots[0].Position = new ScottPlot.SubplotPositions.GridCell(0, 0, 2, 1);
        formsPlot1.Multiplot.PositionedPlots[1].Position = new ScottPlot.SubplotPositions.GridCell(1, 0, 2, 2);
        formsPlot1.Multiplot.PositionedPlots[2].Position = new ScottPlot.SubplotPositions.GridCell(1, 1, 2, 2);
    }
}
