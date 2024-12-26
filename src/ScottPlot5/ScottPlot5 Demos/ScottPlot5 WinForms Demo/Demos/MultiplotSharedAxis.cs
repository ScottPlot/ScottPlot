using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotSharedAxis : Form
{
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
            formsPlot1.Multiplot.ShareX(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Multiplot.ShareY(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Refresh();
        };

        btnShareX.Click += (s, e) =>
        {
            formsPlot1.Multiplot.ShareX(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Multiplot.ShareY([]);
            formsPlot1.Refresh();
        };

        btnShareY.Click += (s, e) =>
        {
            formsPlot1.Multiplot.ShareX([]);
            formsPlot1.Multiplot.ShareY(formsPlot1.Multiplot.GetPlots());
            formsPlot1.Refresh();
        };

        btnShareNone.Click += (s, e) =>
        {
            formsPlot1.Multiplot.ShareX([]);
            formsPlot1.Multiplot.ShareY([]);
        };
    }
}
