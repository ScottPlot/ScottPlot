using System.Windows.Forms;

namespace ScottPlot;

public partial class FormsPlotViewer : Form
{
    public FormsPlotViewer(ScottPlot.Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
    {
        InitializeComponent();
        Width = windowWidth;
        Height = windowHeight;
        Text = windowTitle;

        formsPlot1.Reset(plot);
        formsPlot1.Refresh();
    }
}
