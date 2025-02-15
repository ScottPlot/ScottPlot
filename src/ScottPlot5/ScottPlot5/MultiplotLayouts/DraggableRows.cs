namespace ScottPlot.MultiplotLayouts;

public class DraggableRows() : IMultiplotLayout
{
    /// <summary>
    /// Height for each plot.
    /// The <see cref="ExpandingPlotIndex"/> will override the value for one of these elements.
    /// </summary>
    readonly List<float> PlotHeights = [];

    /// <summary>
    /// The plot with this index will be resized automatically
    /// to occupy remaining space.
    /// </summary>
    public int ExpandingPlotIndex { get; set; } = 0;

    /// <summary>
    /// Plots cannot be resized to be smaller than this number of pixels
    /// </summary>
    public float MinimumHeight { get; set; } = 100;

    /// <summary>
    /// Allow resizing when the cursor is this many pixels away from a divider
    /// </summary>
    public float SnapDistance { get; set; } = 5;

    /// <summary>
    /// Set the initial height for each plot. 
    /// These heights will be modified automatically as dividers are moved or the plot is resized.
    /// </summary>
    public void SetHeights(IEnumerable<float> heights)
    {
        PlotHeights.Clear();
        PlotHeights.AddRange(heights);
    }

    float[] GetDividerPositions()
    {
        if (PlotHeights.Count == 1)
            return [PlotHeights[0]];

        float[] positions = new float[PlotHeights.Count - 1];

        positions[0] = PlotHeights[0];
        for (int i = 1; i < positions.Length; i++)
        {
            positions[i] = positions[i - 1] + PlotHeights[i];
        }

        return positions;
    }

    void ResizeExpandingPlot(float totalHeight)
    {
        PlotHeights[ExpandingPlotIndex] = 0;
        float heightOfOtherPlots = PlotHeights.Sum();
        PlotHeights[ExpandingPlotIndex] = totalHeight - heightOfOtherPlots;
    }


    /// <summary>
    /// Resizes plots as necessary to place a divider at the given location
    /// </summary>
    public void SetDivider(int index, float yPixel)
    {
        // prevent dragging too far up or down
        float limit1 = PlotHeights.Take(index).Sum() + MinimumHeight;
        float limit2 = PlotHeights.Take(index + 2).Sum() - MinimumHeight;
        yPixel = NumericConversion.Clamp(yPixel, limit1, limit2);

        // resize only the plot above and below the divider
        yPixel -= PlotHeights.Take(index).Sum();
        int plotHeightAbove = index;
        int plotHeightBelow = index + 1;
        float doubleHeight = PlotHeights[plotHeightAbove] + PlotHeights[plotHeightBelow];
        PlotHeights[plotHeightAbove] = yPixel;
        PlotHeights[plotHeightBelow] = doubleHeight - yPixel;
    }

    /// <summary>
    /// Returns the index of the divider between two plots at the given pixel
    /// </summary>
    public int? GetDivider(float yPixel)
    {
        if (PlotHeights.Count < 2)
            return null;

        float[] dividerPositions = GetDividerPositions();
        for (int i = 0; i < dividerPositions.Length; i++)
        {
            float distance = Math.Abs(dividerPositions[i] - yPixel);
            if (distance < SnapDistance)
                return i;
        }

        return null;
    }

    /// <summary>
    /// Clear existing rectangles and setup 3 evenly size stacked plots
    /// </summary>
    private void ResetLayout(PixelRect figureRect, int plotCount)
    {
        PlotHeights.Clear();
        for (int i = 0; i < plotCount; i++)
        {
            PlotHeights.Add(figureRect.Height / plotCount);
        }
    }

    public PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect)
    {
        if (PlotHeights.Count != subplots.Count)
            ResetLayout(figureRect, subplots.Count);

        PixelRect[] rectangles = new PixelRect[subplots.Count];

        ResizeExpandingPlot(figureRect.Height);
        float[] dividers = GetDividerPositions();

        for (int i = 0; i < subplots.Count; i++)
        {
            bool isTopPlot = i == 0;
            bool isBottomPlot = i == subplots.Count - 1;

            rectangles[i] = new(
                left: figureRect.Left,
                right: figureRect.Right,
                bottom: isBottomPlot ? figureRect.Bottom : dividers[i],
                top: isTopPlot ? figureRect.Top : dividers[i - 1]);
        }

        return rectangles;
    }
}
