namespace ScottPlot;

public interface ITickGenerator
{
    /// <summary>
    /// Ticks to display the next time the axis is rendered.
    /// This array and its contents should not be modified directly.
    /// Call Regenerate() to update this array.
    /// </summary>
    Tick[] Ticks { get; }

    /// <summary>
    /// Do not generate more than this number of ticks
    /// </summary>
    int MaxTickCount { get; set; }

    /// <summary>
    /// Generate ticks based on the current settings and store the result in <see cref="Ticks"/>
    /// </summary>
    void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle);
}
