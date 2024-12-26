namespace WinForms_Demo.Demos;

public partial class MultiplotAdvancedLayout : Form, IDemoWindow
{
    public string Title => "Multiplot with Advanced Layout";

    public string Description => "Custom multi-plot layouts may be achieved " +
        "by assigning fractional rectangle dimensions to each subplot";

    public MultiplotAdvancedLayout()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        formsPlot1.Multiplot.AddPlots(3);

        // add sample data to each subplot
        for (int i = 0; i < formsPlot1.Multiplot.Count; i++)
        {
            double[] ys = ScottPlot.Generate.Sin(oscillations: i + 1);
            formsPlot1.Multiplot.GetPlot(i).Add.Signal(ys);
        }

        // manually set the position for each plot
        formsPlot1.Multiplot.SetPosition(0, new ScottPlot.SubplotPositions.GridCell(0, 0, 2, 1));
        formsPlot1.Multiplot.SetPosition(1, new ScottPlot.SubplotPositions.GridCell(1, 0, 2, 2));
        formsPlot1.Multiplot.SetPosition(2, new ScottPlot.SubplotPositions.GridCell(1, 1, 2, 2));

    }
}
