using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotAlignment : Form, IDemoWindow
{
    public string Title => "Multiplot Layout Alignment";

    public string Description => "Plots automatically resize the data area to accommodate tick labels of varying length, " +
        "but this may lead to misaligned data areas in multiplot figures. Using a fixed padding resolves this issue.";

    public MultiplotAlignment()
    {
        InitializeComponent();

        // setup a multiplot with 2 subplots
        formsPlot1.Multiplot.AddPlots(2);
        Plot plot1 = formsPlot1.Multiplot.GetPlot(0);
        Plot plot2 = formsPlot1.Multiplot.GetPlot(1);

        // add sample data to each subplot
        plot1.Add.Signal(Generate.RandomWalk(100, mult: 1));
        plot2.Add.Signal(Generate.RandomWalk(100, mult: 10_000));

        btnFixed.Click += (s, e) =>
        {
            PixelPadding padding = new(left: 60, right: 10, bottom: 30, top: 10);
            foreach (var plot in formsPlot1.Multiplot.GetPlots())
                plot.Layout.Fixed(padding);

            formsPlot1.Refresh();
        };

        btnDefault.Click += (s, e) =>
        {
            foreach (var plot in formsPlot1.Multiplot.GetPlots())
                plot.Layout.Default();

            formsPlot1.Refresh();
        };
    }
}
