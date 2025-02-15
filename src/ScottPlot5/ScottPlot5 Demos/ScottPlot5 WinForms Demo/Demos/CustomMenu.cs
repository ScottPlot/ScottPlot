using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class CustomMenu : Form, IDemoWindow
{
    public string Title => "Custom Right-Click Context Menu";

    public string Description => "Demonstrates how to replace the default " +
        "right-click menu with a user-defined one that performs custom actions.";

    public CustomMenu()
    {
        InitializeComponent();
        btnCustom_Click(this, EventArgs.Empty);
    }

    private void btnDefault_Click(object sender, EventArgs e)
    {
        // Reset menu to default options
        formsPlot1.Menu?.Reset();

        formsPlot1.Plot.Title("Default Right-Click Menu");
        formsPlot1.Refresh();
    }

    private void btnCustom_Click(object sender, EventArgs e)
    {
        // clear existing menu items
        formsPlot1.Menu?.Clear();

        // add menu items with custom actions
        formsPlot1.Menu?.Add("Add Scatter", (plot) =>
        {
            plot.Add.Scatter(Generate.RandomCoordinates(5));
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        formsPlot1.Menu?.Add("Add Line", (plot) =>
        {
            var line = plot.Add.Line(Generate.RandomLine());
            line.LineWidth = 2;
            line.MarkerSize = 20;
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        formsPlot1.Menu?.Add("Add Text", (plot) =>
        {
            var txt = plot.Add.Text("Test", Generate.RandomLocation());
            txt.LabelFontSize = 10 + Generate.RandomInteger(20);
            txt.LabelFontColor = Generate.RandomColor(128);
            txt.LabelBold = true;
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        formsPlot1.Menu?.AddSeparator();

        formsPlot1.Menu?.Add("Clear", (plot) =>
        {
            plot.Clear();
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        formsPlot1.Plot.Title("Custom Right-Click Menu");
        formsPlot1.Refresh();
    }
}
