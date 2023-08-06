using ScottPlot.Control;

namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public SharedAxes()
    {
        InitializeComponent();

        // TODO: to ILayoutMaker add a method for getting the layout with a user defined data area

        formsPlot1.Plot.RenderManager.RenderFinished += (object? sender, ScottPlot.Rendering.RenderDetails details) =>
        {
            if (details.LayoutChanged || details.AxisLimitsChanged)
            {
                formsPlot2.Plot.SetAxisLimits(formsPlot1.Plot);
                //formsPlot2.RefreshRequest();
            };
        };
    }

    public string Title => "Shared Axes";

    public string Description => "Two plot controls that share axis limits so one when changes the other updates automatically";
}
