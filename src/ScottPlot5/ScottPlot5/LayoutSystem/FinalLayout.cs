namespace ScottPlot.LayoutSystem;

public struct FinalLayout
{
    /// <summary>
    /// Size of the figure this layout represents
    /// </summary>
    public readonly PixelRect FigureRect { get; }

    /// <summary>
    /// Final size of the data area
    /// </summary>
    public readonly PixelRect DataRect { get; }

    /// <summary>
    /// Distance (pixels) each panel is to be placed from the edge of the data rectangle
    /// </summary>
    public readonly Dictionary<IPanel, float> PanelOffset { get; }

    public FinalLayout(PixelRect figureRect, PixelRect dataRect, Dictionary<IPanel, float> panelOffset)
    {
        FigureRect = figureRect;
        DataRect = dataRect;
        PanelOffset = panelOffset;
    }
}
