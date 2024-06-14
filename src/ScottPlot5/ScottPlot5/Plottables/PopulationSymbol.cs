
namespace ScottPlot.Plottables;

public class PopulationSymbol(Population population) : IPlottable
{
    public Population Population { get; } = population;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Horizontal center of the symbol
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Total width of the symbol
    /// </summary>
    public double Width { get; set; } = 1;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double WidthFraction { get; set; } = 0.75;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double BarWidthFraction { get; set; } = 0.75;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double WhiskerWidthFraction { get; set; } = 0.5;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double MarkerWidthFraction { get; set; } = 0.75;

    public PopulationMarkerAlignment MarkerAlignment { get; set; } = PopulationMarkerAlignment.MarkersOnLeft;
    public enum PopulationMarkerAlignment
    {
        MarkersOnLeft,
        MarkersOnRight,
        MarkersOverlap,
    }

    public PopulationSymbolStyle SymbolStyle { get; set; } = PopulationSymbolStyle.Bar;
    public enum PopulationSymbolStyle
    {
        None,
        Bar,
        Box,
        HalfBox,
    }

    public MarkerStyle MarkerStyle { get; set; } = new()
    {
        Size = 5,
        IsVisible = true,
        OutlineColor = Colors.Black,
        Shape = MarkerShape.OpenCircle
    };

    public LineStyle LineStyle { get; set; } = new()
    {
        IsVisible = true,
        Width = 1,
        Color = Colors.Black,
    };

    public FillStyle FillStyle { get; set; } = new()
    {
        IsVisible = true,
        Color = Colors.Gray,
    };

    public AxisLimits GetAxisLimits() => new(GetRect());

    private CoordinateRect GetRect()
    {
        double left = X - Width * WidthFraction / 2;
        double right = X + Width * WidthFraction / 2;
        double min = Population.Min;
        double max = Population.Max;

        if (SymbolStyle == PopulationSymbolStyle.Bar)
        {
            min = Math.Min(BarBase, min);
            max = Math.Max(BarBase, max);
        }

        return new CoordinateRect(left, right, min, max);
    }

    private CoordinateRect GetMarkerRect()
    {
        CoordinateRect rect = GetRect();

        (double left, double right) = MarkerAlignment switch
        {
            PopulationMarkerAlignment.MarkersOnLeft => (rect.Left, rect.HorizontalCenter),
            PopulationMarkerAlignment.MarkersOnRight => (rect.HorizontalCenter, rect.Right),
            PopulationMarkerAlignment.MarkersOverlap => (rect.Left, rect.Right),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    private CoordinateRect GetSymbolRect()
    {
        CoordinateRect rect = GetRect();

        (double left, double right) = MarkerAlignment switch
        {
            PopulationMarkerAlignment.MarkersOnLeft => (rect.HorizontalCenter, rect.Right),
            PopulationMarkerAlignment.MarkersOnRight => (rect.Left, rect.HorizontalCenter),
            PopulationMarkerAlignment.MarkersOverlap => (rect.Left, rect.Right),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderMarkers(rp, GetMarkerRect(), paint);
        RenderSymbol(rp, GetSymbolRect(), paint);
    }

    private void RenderMarkers(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        double pad = (1 - MarkerWidthFraction) * rect.Width / 2;
        double[] xs = Generate.RandomSample(Population.Count, rect.Left + pad, rect.Right - pad);
        for (int i = 0; i < Population.Count; i++)
        {
            Coordinates location = new(xs[i], Population.Values[i]);
            Pixel px = Axes.GetPixel(location);
            Console.WriteLine(px);
            Drawing.DrawMarker(rp.Canvas, paint, px, MarkerStyle);
        }
    }

    public double BarBase { get; set; } = 0;
    private void RenderSymbol(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        double pad = (1 - BarWidthFraction) * rect.Width / 2;
        CoordinateRect barRect = new(rect.Left + pad, rect.Right - pad, 0, Population.Mean);

        PixelRect pxRect = Axes.GetPixelRect(barRect);
        FillStyle.Render(rp.Canvas, pxRect, paint);
        LineStyle.Render(rp.Canvas, pxRect, paint);
        DrawWhisker(rp, pxRect.Left, pxRect.Right, Population.Mean, Population.Mean + Population.StandardError, paint);
        DrawWhisker(rp, pxRect.Left, pxRect.Right, Population.Mean, Population.Mean - Population.StandardError, paint);
    }

    private void DrawWhisker(RenderPack rp, float x1, float x2, double yBase, double yTip, SKPaint paint)
    {
        float xMid = (x1 + x2) / 2;
        float originalWidth = xMid - x1;
        x1 = xMid - originalWidth * (float)WhiskerWidthFraction;
        x2 = xMid + originalWidth * (float)WhiskerWidthFraction;
        PixelLine verticalLine = new(xMid, Axes.GetPixelY(yBase), xMid, Axes.GetPixelY(yTip));
        PixelLine horizontalLine = new(x1, verticalLine.Y2, x2, verticalLine.Y2);
        LineStyle.Render(rp.Canvas, verticalLine, paint);
        LineStyle.Render(rp.Canvas, horizontalLine, paint);
    }
}