using FluentAssertions;
using ScottPlot;
using ScottPlot.WinForms;

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
        formsPlot1.Menu.Reset();

        formsPlot1.Plot.Title("Default Right-Click Menu");
        formsPlot1.Refresh();
    }

    private void btnCustom_Click(object sender, EventArgs e)
    {
        // clear existing menu items
        formsPlot1.Menu.Clear();

        // add menu items with custom actions
        formsPlot1.Menu.Add("Add Scatter", (formsplot1) =>
        {
            formsplot1.Plot.Add.Scatter(Generate.RandomCoordinates(5));
            formsplot1.Plot.Axes.AutoScale();
            formsplot1.Refresh();
        });

        formsPlot1.Menu.Add("Add Line", (formsplot1) =>
        {
            var line = formsplot1.Plot.Add.Line(Generate.RandomLine());
            line.LineWidth = 2;
            line.MarkerSize = 20;
            formsplot1.Plot.Axes.AutoScale();
            formsplot1.Refresh();
        });

        formsPlot1.Menu.Add("Add Text", (formsplot1) =>
        {
            var txt = formsplot1.Plot.Add.Text("Test", Generate.RandomLocation());
            txt.LabelFontSize = 10 + Generate.RandomInteger(20);
            txt.LabelFontColor = Generate.RandomColor(128);
            txt.LabelBold = true;
            formsplot1.Plot.Axes.AutoScale();
            formsplot1.Refresh();
        });

        formsPlot1.Menu.AddSeparator();

        formsPlot1.Menu.Add("Clear", (formsplot1) =>
        {
            formsplot1.Plot.Clear();
            formsplot1.Plot.Axes.AutoScale();
            formsplot1.Refresh();
        });

        formsPlot1.Plot.Title("Custom Right-Click Menu");
        formsPlot1.Refresh();
    }
}
