namespace ScottPlot.Plottable.DataLoggerViews;

/// <summary>
/// Slide the view one full screen to the right when the data runs outside the view.
/// This is similar to a "sweep" or strip chart.
/// </summary>
public class Sweep : Latest
{
    public Sweep() { PaddingFraction = 1; }
}
