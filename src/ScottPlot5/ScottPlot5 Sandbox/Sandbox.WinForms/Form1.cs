namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        for (int i = 0; i < 10; i++)
        {
            formsPlot1.Plot.Add.Marker(i, i);
        }

        ScottPlot.Plottables.IsoLines isoLines = new();
        formsPlot1.Plot.Add.Plottable(isoLines);

        // space major ticks farther apart so isolines aren't too dense
        ScottPlot.TickGenerators.NumericAutomatic tickGenX = new() { MinimumTickSpacing = 50 };
        formsPlot1.Plot.Axes.Bottom.TickGenerator = tickGenX;
        ScottPlot.TickGenerators.NumericAutomatic tickGenY = new() { MinimumTickSpacing = 50 };
        formsPlot1.Plot.Axes.Left.TickGenerator = tickGenY;

        formsPlot1.Refresh();
    }
}
