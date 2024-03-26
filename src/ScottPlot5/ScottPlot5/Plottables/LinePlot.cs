namespace ScottPlot.Plottables;

public class LinePlot : IPlottable
{
    public Coordinates Start { get; set; }
    public Coordinates End { get; set; }
    public LineStyle LineStyle { get; set; } = new();
    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0 };
    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public object Tag { get; set; } = new();
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, LineStyle, MarkerStyle);

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.Fill.Color = value;
        }
    }

    public Color LineColor
    {
        get => LineStyle.Color;
        set => LineStyle.Color = value;
    }

    public Color MarkerColor
    {
        get => MarkerStyle.Fill.Color;
        set
        {
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public MarkerShape MarkerShape
    {
        get => MarkerStyle.Shape;
        set => MarkerStyle.Shape = value;
    }

    public float LineWidth
    {
        get => LineStyle.Width;
        set => LineStyle.Width = value;
    }

    public float MarkerSize
    {
        get => MarkerStyle.Size;
        set => MarkerStyle.Size = value;
    }

    public LinePattern LinePattern
    {
        get => LineStyle.Pattern;
        set => LineStyle.Pattern = value;
    }

    public AxisLimits GetAxisLimits()
    {
        CoordinateRect boundingRect = new(Start, End);
        return new AxisLimits(boundingRect);
    }

    public void Render(RenderPack rp)
    {
        CoordinateLine line = new(Start, End);
        PixelLine pxLine = Axes.GetPixelLine(line);

        using SKPaint paint = new();
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(Start), MarkerStyle);
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(End), MarkerStyle);
        Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
    }
}
