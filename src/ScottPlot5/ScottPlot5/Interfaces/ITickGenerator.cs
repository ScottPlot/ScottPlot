namespace ScottPlot;

public interface ITickGenerator
{
    /// <summary>
    /// Ticks to display the next time the axis is rendered
    /// </summary>
    Tick[] Ticks { get; set; }

    /// <summary>
    /// Do not automatically generate more ticks than this
    /// </summary>
    int MaxTickCount { get; set; }

    /// <summary>
    /// Logic for generating ticks automatically.
    /// Generated ticks are stored in <see cref="Ticks"/>.
    /// </summary>
    void Regenerate(CoordinateRange range, Edge edge, PixelLength size);
}
