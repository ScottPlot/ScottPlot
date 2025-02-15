namespace ScottPlot.Plottables;

public class LinePlot : IPlottable, IHasLine, IHasMarker, IHasLegendText
{
    public Coordinates Start { get; set; }
    public Coordinates End { get; set; }
    public CoordinateLine Line
    {
        get
        {
            return new CoordinateLine(Start, End);
        }
        set
        {
            Start = value.Start;
            End = value.End;
        }
    }

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0 };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, LineStyle, MarkerStyle);

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public bool LineOnTop { get; set; } = true;
    public bool MarkersOnTop { get => !LineOnTop; set => LineOnTop = !value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
        }
    }

    public AxisLimits GetAxisLimits()
    {
        CoordinateRect boundingRect = new(Start, End);
        return new AxisLimits(boundingRect);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        if (LineOnTop)
        {
            DrawMarkers(rp, paint);
            DrawLine(rp, paint);
        }
        else
        {
            DrawLine(rp, paint);
            DrawMarkers(rp, paint);
        }
    }

    private void DrawMarkers(RenderPack rp, SKPaint paint)
    {
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(Start), MarkerStyle);
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(End), MarkerStyle);
    }

    private void DrawLine(RenderPack rp, SKPaint paint)
    {
        CoordinateLine line = new(Start, End);
        PixelLine pxLine = Axes.GetPixelLine(line);
        Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
    }
}
