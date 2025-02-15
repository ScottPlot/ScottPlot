namespace ScottPlot;

/// <summary>
/// Represents a single item in a right-click pop-up menu
/// </summary>
public struct ContextMenuItem
{
    public bool IsSeparator { get; set; }
    public string Label { get; set; }
    public Action<Plot> OnInvoke { get; set; }
}
