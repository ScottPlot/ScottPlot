using System.Windows.Forms;

namespace ScottPlot;

#nullable enable

public partial class FormsPlotViewer : Form
{
    private readonly FormsPlot? ParentControl = null;

    /// <summary>
    /// Launch an interactive plot viewer from an existing plot.
    /// </summary>
    public FormsPlotViewer(ScottPlot.Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
    {
        InitializeComponent();
        Width = windowWidth;
        Height = windowHeight;
        Text = windowTitle;

        formsPlot1.Reset(plot);
        formsPlot1.Refresh();
    }

    /// <summary>
    /// Launch an interactive plot viewer from an existing FormsPlot control.
    /// This overload allows syncing where updates to one plot may affect the other.
    /// </summary>
    public FormsPlotViewer(FormsPlot parentControl, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer",
        bool childUpdatesParent = false, bool parentUpdatesChld = false)
    {
        InitializeComponent();

        Width = windowWidth;
        Height = windowHeight;
        Text = windowTitle;
        ParentControl = parentControl;

        if (ParentControl is not null)
        {
            if (childUpdatesParent)
            {
                formsPlot1.Rendered += (s, e) =>
                {
                    ParentControl.EnableRenderedEvent = false;
                    ParentControl.Refresh();
                    ParentControl.EnableRenderedEvent = true;
                };
            }

            if (parentUpdatesChld)
            {
                ParentControl.Rendered += (s, e) =>
                {
                    formsPlot1.EnableRenderedEvent = false;
                    formsPlot1.Refresh();
                    formsPlot1.EnableRenderedEvent = true;
                };
            }
        }

        formsPlot1.Reset(parentControl.Plot);
        formsPlot1.Refresh();
    }
}
