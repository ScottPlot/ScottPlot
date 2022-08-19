namespace ScottPlot;

public interface ITickGenerator
{
    /// <summary>
    /// Ticks to display the next time the axis is rendered
    /// </summary>
    Tick[] Ticks { get; set; } // TODO: obsolete

    /// <summary>
    /// Do not automatically generate more ticks than this
    /// </summary>
    int MaxTickCount { get; set; }

    /// <summary>
    /// Logic for generating ticks automatically
    /// </summary>
    void Regenerate(CoordinateRange range, PixelLength size);

    /// <summary>
    /// Return just the ticks within a range (inclusive) up to a maximum number
    /// </summary>
    IEnumerable<Tick> GetVisibleTicks(CoordinateRange range);
}
