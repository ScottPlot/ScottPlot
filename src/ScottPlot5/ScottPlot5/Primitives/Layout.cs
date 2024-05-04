namespace ScottPlot;

public readonly struct Layout : IEquatable<Layout>
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
    public readonly Dictionary<IPanel, float> PanelOffsets { get; }

    /// <summary>
    /// Size (pixels) of each panel in the dimension perpendicular to the data edge it is placed on
    /// </summary>
    public readonly Dictionary<IPanel, float> PanelSizes { get; }

    public Layout(PixelRect figureRect, PixelRect dataRect, Dictionary<IPanel, float> sizes, Dictionary<IPanel, float> offsets)
    {
        FigureRect = figureRect;
        DataRect = dataRect;
        PanelSizes = sizes;
        PanelOffsets = offsets;
    }

    public bool Equals(Layout other)
    {
        if (!FigureRect.Equals(other.FigureRect))
            return false;

        if (!DataRect.Equals(other.DataRect))
            return false;

        return true;
    }
}
