namespace ScottPlot.Plottable;

/// <summary>
/// Describes a Plottable that can report whether or not it is beneath the mouse cursor
/// </summary>
public interface IHittable
{
    /// <summary>
    /// Cursor to display when the Plottable is under the mouse
    /// </summary>
    Cursor HitCursor { get; set; }

    /// <summary>
    /// Returns true if the Plottable is at the given coordinate
    /// </summary>
    bool HitTest(Coordinate coord);
}
