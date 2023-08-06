namespace ScottPlot;

public struct Layout : IEquatable<Layout>
{
    /// <summary>
    /// Size of the figure this layout represents
    /// </summary>
    public readonly PixelSize FigureSize { get; }

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

    public Layout(PixelSize figureSize, PixelRect dataRect, Dictionary<IPanel, float> sizes, Dictionary<IPanel, float> offsets)
    {
        FigureSize = figureSize;
        DataRect = dataRect;
        PanelSizes = sizes;
        PanelOffsets = offsets;
    }

    public bool Equals(Layout other)
    {
        if (!FigureSize.Equals(other.FigureSize))
            return false;

        if (!DataRect.Equals(other.DataRect))
            return false;

        return true;
    }
}
