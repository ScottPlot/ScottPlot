using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class CustomArrows : Form
{
    public CustomArrows()
    {
        InitializeComponent();

        double[] values = Generate.Sin();
        formsPlot1.Plot.Add.Signal(values);

        Coordinates arrowTip = new(25, 0);
        Coordinates arrowBase = arrowTip.WithDelta(5, .5);
        var arrow = formsPlot1.Plot.Add.Arrow(arrowBase, arrowTip);
        arrow.ArrowheadLength = 24;
        arrow.ArrowheadWidth = 36;
        arrow.ArrowLineWidth = 8;
    }
}
