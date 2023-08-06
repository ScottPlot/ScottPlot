namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public SharedAxes()
    {
        InitializeComponent();

        // TODO: to ILayoutMaker add a method for getting the layout with a user defined data area

        formsPlot1.Plot.RenderManager.RenderFinished += (object? sender, ScottPlot.Rendering.RenderDetails plot1Info) =>
        {
            bool differentLayout = !plot1Info.Layout.Equals(formsPlot2.Plot.RenderManager.LastRenderInfo.Layout);
            bool differentAxisLimits = !plot1Info.AxisLimits.Equals(formsPlot2.Plot.RenderManager.LastRenderInfo.AxisLimits);

            // TODO: figure out why layouts always appear different
            // TODO: figure out why renders happen so often and remove duplicates if possible

            if (differentAxisLimits)
            {
                formsPlot2.Plot.SetAxisLimits(formsPlot1.Plot);
                formsPlot1.RefreshQueue(formsPlot2);
            }
        };

        formsPlot2.Plot.RenderManager.RenderFinished += (object? sender, ScottPlot.Rendering.RenderDetails plot2Info) =>
        {
            bool differentLayout = !plot2Info.Layout.Equals(formsPlot1.Plot.RenderManager.LastRenderInfo.Layout);
            bool differentAxisLimits = !plot2Info.AxisLimits.Equals(formsPlot1.Plot.RenderManager.LastRenderInfo.AxisLimits);

            if (differentAxisLimits)
            {
                formsPlot1.Plot.SetAxisLimits(formsPlot2.Plot);
                formsPlot2.RefreshQueue(formsPlot1);
            };
        };
    }

    public string Title => "Shared Axes";

    public string Description => "Two plot controls that share axis limits so one when changes the other updates automatically";
}
