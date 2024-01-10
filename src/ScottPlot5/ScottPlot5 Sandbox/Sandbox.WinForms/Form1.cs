namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        double[] positions1 = { 1, 4, 7, 10, 13, 16, 19 };
        double[] values1 = { 1, 4, 7, 10, 13, 16, 19 };
        double[] positions2 = { 2, 5, 8, 11, 14, 17, 20 };
        double[] values2 = { 2, 5, 8, 11, 14, 17, 20 };

        formsPlot1.Plot.Add.Bars(positions1, values1);
        formsPlot1.Plot.Add.Bars(positions2, values2);

        formsPlot1.Plot.Axes.Rules.Add(new ScottPlot.AxisRules.LockedVertical(formsPlot1.Plot.Axes.Left));
        //formsPlot1.Plot.Axes.Rules.Add(new ScottPlot.AxisRules.LockedHorizontal(formsPlot1.Plot.Axes.Bottom));
    }
}
