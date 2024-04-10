namespace ScottPlot.Plottables;

public class Crosshair : IPlottable, IRenderLast
{
    public HorizontalLine HorizontalLine { get; } = new();
    public VerticalLine VerticalLine { get; } = new();

    public Coordinates Position { get => new(X, Y); set { X = value.X; Y = value.Y; } }
    public double X { get => VerticalLine.X; set => VerticalLine.X = value; }
    public double Y { get => HorizontalLine.Y; set => HorizontalLine.Y = value; }

    // These properties set styling for both axis lines at once
    public float FontSize { set { HorizontalLine.FontSize = value; VerticalLine.FontSize = value; } }
    public bool FontBold { set { HorizontalLine.FontBold = value; VerticalLine.FontBold = value; } }
    public string FontName { set { HorizontalLine.FontName = value; VerticalLine.FontName = value; } }
    public Color TextColor { set { HorizontalLine.TextColor = value; VerticalLine.TextColor = value; } }
    public Color TextBackgroundColor { set { HorizontalLine.TextBackgroundColor = value; VerticalLine.TextBackgroundColor = value; } }
    public Color LineColor { set { HorizontalLine.LineColor = value; VerticalLine.LineColor = value; } }
    public float LineWidth { set { HorizontalLine.LineWidth = value; VerticalLine.LineWidth = value; } }
    public LinePattern LinePattern { set { HorizontalLine.LinePattern = value; VerticalLine.LinePattern = value; } }

    [Obsolete("Assign values to Color, LineWidth, or LinePattern properties to set style for both lines. " +
        "HorizontalLine.LineStyle and VerticalLine.LineStyle are individually available as well.", true)]
    public LineStyle LineStyle { get; set; } = new();
    [Obsolete("Use TextColor and TextBackgroundColor instead", true)]
    public Color FontColor;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => HorizontalLine.LegendItems.Concat(VerticalLine.LegendItems);
    public AxisLimits GetAxisLimits() => new(X, X, Y, Y);

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        HorizontalLine.Axes = Axes;
        HorizontalLine.Render(rp);

        VerticalLine.Axes = Axes;
        VerticalLine.Render(rp);
    }

    public void RenderLast(RenderPack rp)
    {
        if (!IsVisible)
            return;

        HorizontalLine.Axes = Axes;
        HorizontalLine.RenderLast(rp);

        VerticalLine.Axes = Axes;
        VerticalLine.RenderLast(rp);
    }
}
