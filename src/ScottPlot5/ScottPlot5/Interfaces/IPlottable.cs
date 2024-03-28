namespace ScottPlot;

/// <summary>
/// Any object that renders shapes on the canvas using scale information from the axes must implement this interface.
/// </summary>
public interface IPlottable
{
    /// <summary>
    /// Toggles whether this plottable is shown and contributes to the automatic axis limit detection.
    /// The calling method will check this variable (it does not need to be checked inside the Render method).
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// This object performs coordinate/pixel translation at render time based on the latest data area.
    /// It stores the axes to use for this plottable and also the data area (in pixels) updated just before each render.
    /// If this object is null it will be constructed using the default X and Y axes at render time.
    /// </summary>
    IAxes Axes { get; set; }

    /// <summary>
    /// Return the 2D area (in coordinate space) occupied by the data contained in this plottable
    /// </summary>
    AxisLimits GetAxisLimits();

    /// <summary>
    /// Draw the data from this plottable into the data area defined in the <see cref="Axes"/>.
    /// By default the surface is already clipped to the data area, but this can be cleared inside the plottable.
    /// </summary>
    void Render(RenderPack rp);

    /// <summary>
    /// Items which will appear in the legend
    /// </summary>
    IEnumerable<LegendItem> LegendItems { get; } // TODO: this should be a method GetLegendItems()
}
