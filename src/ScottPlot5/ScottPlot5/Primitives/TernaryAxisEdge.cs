namespace ScottPlot;

/// <summary>
/// Represents an edge of the ternary plot triangle and includes styling information.
/// </summary>
public class TernaryAxisEdge : IHasLine
{
    public Coordinates Start { get; set; }
    public Coordinates End { get; set; }

    public LineStyle LineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.5),
    };

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public LinePattern LinePattern
    {
        get => LineStyle.Pattern;
        set => LineStyle.Pattern = value;
    }

    public Color LineColor
    {
        get => LineStyle.Color;
        set => LineStyle.Color = value;
    }

    public TernaryAxisEdge(Coordinates start, Coordinates end)
    {
        Start = start;
        End = end;
    }
}
