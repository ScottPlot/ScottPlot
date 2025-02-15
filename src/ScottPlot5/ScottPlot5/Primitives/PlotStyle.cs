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
    public Color? FigureBackgroundColor { get; set; } = null;
    public Color? DataBackgroundColor { get; set; } = null;

    // axes and grids
    public Color? AxisColor { get; set; } = null;
    public Color? GridMajorLineColor { get; set; } = null;

    // legend
    public Color? LegendBackgroundColor { get; set; } = null;
    public Color? LegendFontColor { get; set; } = null;
    public Color? LegendOutlineColor { get; set; } = null;

    /// <summary>
    /// Apply these style settings to the given plot
    /// </summary>
    public void Apply(Plot plot)
    {
        // plot
        if (FigureBackgroundColor.HasValue) plot.FigureBackground.Color = FigureBackgroundColor.Value;
        if (DataBackgroundColor.HasValue) plot.DataBackground.Color = DataBackgroundColor.Value;
        if (Palette is not null) plot.Add.Palette = Palette;

        // axes and grids
        if (AxisColor.HasValue) plot.Axes.Color(AxisColor.Value);
        if (GridMajorLineColor.HasValue) plot.Grid.MajorLineColor = GridMajorLineColor.Value;

        // legend
        if (LegendBackgroundColor.HasValue) plot.Legend.BackgroundColor = LegendBackgroundColor.Value;
        if (LegendFontColor.HasValue) plot.Legend.FontColor = LegendFontColor.Value;
        if (LegendOutlineColor.HasValue) plot.Legend.OutlineColor = LegendOutlineColor.Value;
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
            FigureBackgroundColor = plot.FigureBackground.Color,
            DataBackgroundColor = plot.DataBackground.Color,

            // axes and grids
            AxisColor = plot.Axes.GetAxes().First().FrameLineStyle.Color,
            GridMajorLineColor = plot.Grid.MajorLineColor,

            // legend
            LegendBackgroundColor = plot.Legend.BackgroundColor,
            LegendFontColor = plot.Legend.FontColor,
            LegendOutlineColor = plot.Legend.OutlineColor,
        };
    }

    /// <summary>
    /// Return a plot style with all values nulled except those
    /// that are different than the given style
    /// </summary>
    public PlotStyle WhereDifferentFrom(PlotStyle other)
    {
        return new PlotStyle()
        {
            // TODO: can this be done with reflection?

            // plot
            Palette = Palette != other.Palette ? Palette : null,
            FigureBackgroundColor = FigureBackgroundColor != other.FigureBackgroundColor ? FigureBackgroundColor : null,
            DataBackgroundColor = DataBackgroundColor != other.DataBackgroundColor ? DataBackgroundColor : null,

            // axes and grids
            AxisColor = AxisColor != other.AxisColor ? AxisColor : null,
            GridMajorLineColor = GridMajorLineColor != other.GridMajorLineColor ? GridMajorLineColor : null,

            // legend
            LegendBackgroundColor = LegendBackgroundColor != other.LegendBackgroundColor ? LegendBackgroundColor : null,
            LegendFontColor = LegendFontColor != other.LegendFontColor ? LegendFontColor : null,
            LegendOutlineColor = LegendOutlineColor != other.LegendOutlineColor ? LegendOutlineColor : null,
        };
    }
}
