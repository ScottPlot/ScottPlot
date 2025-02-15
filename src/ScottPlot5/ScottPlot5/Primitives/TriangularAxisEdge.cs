namespace ScottPlot;

public class TriangularAxisEdge
{
    public CoordinateLine Line { get; }
    public Coordinates Start => Line.Start;
    public Coordinates End => Line.End;
    public List<(Coordinates Point, string Label)> Ticks { get; } = [];
    public LineStyle EdgeLineStyle { get; set; } = new() { IsVisible = true, Width = 1, Color = Colors.Black, };
    public TickMarkStyle TickMarkStyle { get; set; } = new() { Length = 5, Color = Colors.Black };
    public LabelStyle TickLabelStyle { get; set; } = new();
    public LabelStyle LabelStyle { get; set; } = new() { FontSize = 16, Bold = true };
    public PixelOffset TickOffset { get; set; }

    public static readonly double MaxY = Math.Sqrt(3) / 2;

    public static readonly AxisLimits AxisLimits = new(0, 1, 0, MaxY);

    public string LabelText { get; set; } = string.Empty;

    public void Title(string title)
    {
        LabelText = title;
    }

    public void Title(string title, Color color)
    {
        LabelText = title;
        Color(color);
    }

    public void Color(Color color)
    {
        LabelStyle.ForeColor = color;
        TickLabelStyle.ForeColor = color;
        TickMarkStyle.Color = color;
        EdgeLineStyle.Color = color;
    }

    private static Coordinates TopCorner = new(0.5, Math.Sqrt(3) / 2);
    private static Coordinates LeftCorner = new(0, 0);
    private static Coordinates RightCorner = new(1, 0);

    public static TriangularAxisEdge Left(bool clockwise = true)
    {
        CoordinateLine line = new(LeftCorner, TopCorner);
        TriangularAxisEdge ax = new(clockwise ? line : line.Reversed());
        ax.TickOffset = new(-5, 0);
        ax.TickLabelStyle.Alignment = Alignment.MiddleRight;
        ax.LabelStyle.Rotation = -60;
        ax.LabelStyle.Alignment = Alignment.LowerCenter;
        ax.LabelStyle.OffsetX = -10;
        ax.LabelStyle.OffsetY = ax.LabelStyle.OffsetX * (float)Math.Sqrt(3) / 2;
        return ax;
    }

    public static TriangularAxisEdge Right(bool clockwise = true)
    {
        CoordinateLine line = new(TopCorner, RightCorner);
        TriangularAxisEdge ax = new(clockwise ? line : line.Reversed());
        ax.TickOffset = new(5, 0);
        ax.TickLabelStyle.Alignment = Alignment.MiddleLeft;
        ax.LabelStyle.Rotation = 60;
        ax.LabelStyle.Alignment = Alignment.LowerCenter;
        ax.LabelStyle.OffsetX = 10;
        ax.LabelStyle.OffsetY = -ax.LabelStyle.OffsetX * (float)Math.Sqrt(3) / 2;
        return ax;
    }

    public static TriangularAxisEdge Bottom(bool clockwise = true)
    {
        CoordinateLine line = new(RightCorner, LeftCorner);
        TriangularAxisEdge ax = new(clockwise ? line : line.Reversed());
        ax.TickOffset = new(0, 5);
        ax.TickLabelStyle.Alignment = Alignment.UpperCenter;
        ax.LabelStyle.Alignment = Alignment.UpperCenter;
        ax.LabelStyle.OffsetY = 20;
        return ax;
    }

    public TriangularAxisEdge(CoordinateLine line)
    {
        Line = line;
        RegenerateTicks();
    }

    private void RegenerateTicks(int ticksPerEdge = 10)
    {
        Ticks.Clear();
        for (int i = 0; i <= ticksPerEdge; i++)
        {
            double fraction = i / (double)ticksPerEdge;
            double tickX = Start.X + fraction * (End.X - Start.X);
            double tickY = Start.Y + fraction * (End.Y - Start.Y);
            Coordinates tickPoint = new(tickX, tickY);
            string tickLabel = $"{fraction * 10}";
            Ticks.Add((tickPoint, tickLabel));
        }
    }

    /// <summary>
    /// Return the point along this axis a given fraction between 0 and 1
    /// </summary>
    public Coordinates GetCoordinates(double fraction)
    {
        double x = Start.X + (End.X - Start.X) * fraction;
        double y = Start.Y + (End.Y - Start.Y) * fraction;
        return new Coordinates(x, y);
    }
}
