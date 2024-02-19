namespace ScottPlot.Control;

/// <summary>
/// Represents a single item in a right-click pop-up menu
/// </summary>
public struct ContextMenuItem
{
    public bool IsSeparator { get; set; }
    public string Label { get; set; }
    public Action<IPlotControl> OnInvoke { get; set; }
}
