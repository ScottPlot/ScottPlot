using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotSharedAxis : Form, IDemoWindow
{
    public string Title => "Multiplot with shared axis limits";

    public string Description => "Updates to axis limits of a subplot may be applied to all other subplots in the multiplot.";

    public MultiplotSharedAxis()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        formsPlot1.Multiplot.AddPlots(3);

        // add sample data to each subplot
        foreach (Plot plot in formsPlot1.Multiplot.GetPlots())
        {
            plot.Add.Signal(Generate.AddNoise(Generate.Sin()));
        }

        btnShareXY.Click += (s, e) =>
        {
            formsPlot1.Multiplot.SharedAxes.ShareX(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Multiplot.SharedAxes.ShareY(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Refresh();
        };

        btnShareX.Click += (s, e) =>
        {
            formsPlot1.Multiplot.SharedAxes.ShareX(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Multiplot.SharedAxes.ShareY([]);
            formsPlot1.Refresh();
        };

        btnShareY.Click += (s, e) =>
        {
            formsPlot1.Multiplot.SharedAxes.ShareX([]);
            formsPlot1.Multiplot.SharedAxes.ShareY(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Refresh();
        };

        btnShareNone.Click += (s, e) =>
        {
            formsPlot1.Multiplot.SharedAxes.ShareX([]);
            formsPlot1.Multiplot.SharedAxes.ShareY([]);
        };
    }
}
