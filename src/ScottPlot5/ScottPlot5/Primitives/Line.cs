using System.IO;

namespace ScottPlot;

/// <summary>
/// Represents an line on screen defined by the endpoints, pattern, and style.
/// </summary>
public class Line
{
    public Coordinates Start { get; set; }
    public Coordinates End { get; set; }
    public static Line Default => new(0, 0, 1, 1);
    public LineStyle Style { get; set; } = new LineStyle();

    /// <summary>
    /// Create a default line using x and y values
    /// </summary>
    public Line(double x1, double y1, double x2, double y2)
    {
        Start = new Coordinates(x1, y1);
        End = new Coordinates(x2, y2);
    }

    /// <summary>
    /// Create a default line using coordinates
    /// </summary>
    public Line(Coordinates start, Coordinates end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Create a styled line using x and y values, LinePattern, and LineStyle information
    /// </summary>
    public Line(double x1, double y1, double x2, double y2, LineStyle style)
    {
        Start = new Coordinates(x1, y1);
        End = new Coordinates(x2, y2);
        Style = style;
    }

    /// <summary>
    /// Create a styled line using coordinates, LinePattern, and LineStyle information
    /// </summary>
    public Line(Coordinates start, Coordinates end, LineStyle style)
    {
        Start = start;
        End = end;
        Style = style;
    }

    public override string ToString()
    {
        return $"Line {{ X1 = {Start.X}, Y1 = {Start.Y}, X2 = {End.X}, Y2 = {End.Y} }}";
    }
}
