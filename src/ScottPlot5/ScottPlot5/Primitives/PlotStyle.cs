namespace ScottPlot;

/// <summary>
/// This object holds many common plot style customizations
/// and facilitates switching between styles or copying styles
/// from one plot to another.
/// </summary>
public class PlotStyle
{
    // plot
    public IPalette? Palette { get; set; } = null;
    public Color? FigureBackground { get; set; } = null;
    public Color? DataBackGround { get; set; } = null;

    // axes and grids
    public Color? Axes { get; set; } = null;
    public Color? GridMajorLine { get; set; } = null;

    // legend
    public Color? LegendBackground { get; set; } = null;
    public Color? LegendFont { get; set; } = null;
    public Color? LegendOutline { get; set; } = null;

    /// <summary>
    /// Apply these style settings to the given plot
    /// </summary>
    public void Apply(Plot plot)
    {
        // plot
        if (FigureBackground.HasValue) plot.FigureBackground.Color = FigureBackground.Value;
        if (DataBackGround.HasValue) plot.DataBackground.Color = DataBackGround.Value;
        if (Palette is not null) plot.Add.Palette = Palette;

        // axes and grids
        if (Axes.HasValue) plot.Axes.Color(Axes.Value);
        if (GridMajorLine.HasValue) plot.Grid.MajorLineColor = GridMajorLine.Value;

        // legend
        if (LegendBackground.HasValue) plot.Legend.BackgroundColor = LegendBackground.Value;
        if (LegendFont.HasValue) plot.Legend.FontColor = LegendFont.Value;
        if (LegendOutline.HasValue) plot.Legend.OutlineColor = LegendOutline.Value;
    }

    /// <summary>
    /// Return the styles represented by the given plot
    /// </summary>
    public static PlotStyle FromPlot(Plot plot)
    {
        return new PlotStyle()
        {
            // plot
            Palette = plot.Add.Palette,
            FigureBackground = plot.FigureBackground.Color,
            DataBackGround = plot.DataBackground.Color,

            // axes and grids
            Axes = plot.Axes.GetAxes().First().FrameLineStyle.Color,
            GridMajorLine = plot.Grid.MajorLineColor,

            // legend
            LegendBackground = plot.Legend.BackgroundColor,
            LegendFont = plot.Legend.FontColor,
            LegendOutline = plot.Legend.OutlineColor,
        };
    }

    /// <summary>
    /// Return the styles represented by the given plot,
    /// excluding styles which have been manually changed from the reference style.
    /// </summary>
    public static PlotStyle FromPlot(Plot plot, PlotStyle referenceStyle)
    {
        throw new NotImplementedException();
    }
}
