namespace ScottPlot.Cookbook.Categories.PlotTypes;

public class Crosshair : ICategory
{
    public string Name => "Crosshair";

    public string Folder => "plottable-crosshair";

    public string Description => "The Crosshair plot type draws vertical and horizontal lines " +
        "that intersect at a point on the plot and the coordinates of those lines are displayed " +
        "on top of the axis ticks. This plot type is typically updated after MouseMove events to track the mouse.";
}
