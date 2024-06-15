
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
    public double WidthFraction { get; set; } = 0.9;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double SymbolWidthFraction { get; set; } = 0.75;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double WhiskerWidthFraction { get; set; } = 0.5;

    /// <summary>
    /// Fraction of the available width to use
    /// </summary>
    public double MarkerWidthFraction { get; set; } = 0.75;
    public double BarBase { get; set; } = 0;

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

    public MarkerStyle DataMarkerStyle { get; set; } = new()
    {
        Size = 7,
        IsVisible = true,
        OutlineColor = Colors.Black,
        Shape = MarkerShape.OpenCircle
    };

    public MarkerStyle MeanMarkerStyle { get; set; } = new()
    {
        Size = 7,
        IsVisible = false,
        FillColor = Colors.Black,
        Shape = MarkerShape.FilledCircle
    };

    public LineStyle WhiskerLineStyle { get; set; } = new()
    {
        IsVisible = true,
        Width = 1,
        Color = Colors.Black,
    };

    public LineStyle BarLineStyle { get; set; } = new()
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

        double pad = (1 - MarkerWidthFraction) * rect.Width / 2;

        (double left, double right) = MarkerAlignment switch
        {
            PopulationMarkerAlignment.MarkersOnLeft => (rect.Left + pad, rect.HorizontalCenter - pad),
            PopulationMarkerAlignment.MarkersOnRight => (rect.HorizontalCenter + pad, rect.Right - pad),
            PopulationMarkerAlignment.MarkersOverlap => (rect.Left + pad, rect.Right - pad),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    private CoordinateRect GetSymbolRect()
    {
        CoordinateRect rect = GetRect();

        double pad = (1 - SymbolWidthFraction) * rect.Width / 2;

        (double left, double right) = MarkerAlignment switch
        {
            PopulationMarkerAlignment.MarkersOnLeft => (rect.HorizontalCenter + pad, rect.Right - pad),
            PopulationMarkerAlignment.MarkersOnRight => (rect.Left + pad, rect.HorizontalCenter - pad),
            PopulationMarkerAlignment.MarkersOverlap => (rect.Left + pad, rect.Right - pad),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderDataMarkers(rp, GetMarkerRect(), paint);
        RenderSymbol(rp, GetSymbolRect(), paint);
    }

    private void RenderDataMarkers(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        double pad = (1 - MarkerWidthFraction) * rect.Width / 2;
        double[] xs = Generate.RandomSample(Population.Count, rect.Left + pad, rect.Right - pad);
        for (int i = 0; i < Population.Count; i++)
        {
            Coordinates location = new(xs[i], Population.Values[i]);
            Pixel px = Axes.GetPixel(location);
            Console.WriteLine(px);
            Drawing.DrawMarker(rp.Canvas, paint, px, DataMarkerStyle);
        }
    }

    private void RenderSymbol(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        DrawBar(rp, rect, paint);
        DrawWhiskers(rp, rect, paint);
        MeanMarkerStyle.Render(rp.Canvas, Axes.GetPixel(new(rect.HorizontalCenter, Population.Mean)), paint);
    }

    private void DrawBar(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        CoordinateRect barRect = new(rect.Left, rect.Right, 0, Population.Mean);
        PixelRect pxRect = Axes.GetPixelRect(barRect);
        FillStyle.Render(rp.Canvas, pxRect, paint);
        BarLineStyle.Render(rp.Canvas, pxRect, paint);
    }

    private void DrawWhiskers(RenderPack rp, CoordinateRect rect, SKPaint paint)
    {
        double left = rect.HorizontalCenter - rect.Width * WhiskerWidthFraction / 2;
        double right = rect.HorizontalCenter + rect.Width * WhiskerWidthFraction / 2;
        double bottom = Population.Mean - Population.StandardError;
        double top = Population.Mean + Population.StandardError;
        CoordinateRect whiskerRect = new(left, right, bottom, top);
        PixelRect whiskerPxRect = Axes.GetPixelRect(whiskerRect);

        PixelLine verticalLine = new(whiskerPxRect.HorizontalCenter, whiskerPxRect.Bottom, whiskerPxRect.HorizontalCenter, whiskerPxRect.Top);
        PixelLine horizontalLineTop = new(whiskerPxRect.Left, whiskerPxRect.Top, whiskerPxRect.Right, whiskerPxRect.Top);
        PixelLine horizontalLineBottom = new(whiskerPxRect.Left, whiskerPxRect.Bottom, whiskerPxRect.Right, whiskerPxRect.Bottom);

        WhiskerLineStyle.Render(rp.Canvas, verticalLine, paint);
        WhiskerLineStyle.Render(rp.Canvas, horizontalLineTop, paint);
        WhiskerLineStyle.Render(rp.Canvas, horizontalLineBottom, paint);
    }
}