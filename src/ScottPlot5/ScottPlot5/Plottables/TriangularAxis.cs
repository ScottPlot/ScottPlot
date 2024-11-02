namespace ScottPlot.Plottables;

/// <summary>
/// This plot type places a triangular axis on the plot 
/// and has methods to convert between triangular and Cartesian coordinates.
/// </summary>
public class TriangularAxis : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Fractional amount of padding between the edge of the triangle and the data area.
    /// Increase this value to make room for large tick and corner labels.
    /// </summary>
    public double PaddingFraction { get; set; } = 0.25;

    public AxisLimits GetAxisLimits() => new AxisLimits(0, 1, 0, Math.Sqrt(3) / 2)
        .WithZoom(1 - PaddingFraction, 1 - PaddingFraction);

    public FillStyle FillStyle { get; set; } = new()
    {
        IsVisible = true,
        Color = Colors.Yellow.WithAlpha(.1),
    };

    public LineStyle GridLineStyle { get; set; } = new()
    {
        IsVisible = true,
        Width = 1,
        Color = Colors.Gray.WithAlpha(.2)
    };

    // TODO: make a class that groups these properties together

    public LineStyle BottomEdgeLineStyle { get; set; } = new() { IsVisible = true, Width = 1, Color = Colors.Black, };
    public LineStyle LeftEdgeLineStyle { get; set; } = new() { IsVisible = true, Width = 1, Color = Colors.Black, };
    public LineStyle RightEdgeLineStyle { get; set; } = new() { IsVisible = true, Width = 1, Color = Colors.Black, };

    public TickMarkStyle BottomTickLineStyle { get; set; } = new() { Length = 5, Color = Colors.Black };
    public TickMarkStyle LeftTickLineStyle { get; set; } = new() { Length = 5, Color = Colors.Black };
    public TickMarkStyle RightTickLineStyle { get; set; } = new() { Length = 5, Color = Colors.Black };

    public LabelStyle BottomTickLabelStyle { get; set; } = new() { Alignment = Alignment.UpperCenter };
    public LabelStyle LeftTickLabelStyle { get; set; } = new() { Alignment = Alignment.MiddleRight, OffsetX = -3 };
    public LabelStyle RightTickLabelStyle { get; set; } = new() { Alignment = Alignment.MiddleLeft, OffsetX = 3 };

    public static Coordinates LeftCorner { get; } = new(0, 0);
    public static Coordinates RightCorner { get; } = new(1, 0);
    public static Coordinates TopCorner { get; } = new(0.5, Math.Sqrt(3) / 2);
    public static IEnumerable<Coordinates> Corners { get; } = [LeftCorner, RightCorner, TopCorner];

    public static CoordinateLine BottomEdgeLine { get; } = new(LeftCorner, RightCorner);
    public static CoordinateLine LeftEdgeLine { get; } = new(TopCorner, LeftCorner);
    public static CoordinateLine RightEdgeLine { get; } = new(RightCorner, TopCorner);
    public static IEnumerable<CoordinateLine> EdgeLines { get; } = [BottomEdgeLine, LeftEdgeLine, RightEdgeLine];

    private List<TriangularTick> BottomTicks { get; }
    private List<TriangularTick> LeftTicks { get; }
    private List<TriangularTick> RightTicks { get; }

    public TriangularAxis()
    {
        BottomTicks = GetEdgeTicks(BottomEdgeLine, 10);
        LeftTicks = GetEdgeTicks(LeftEdgeLine, 10);
        RightTicks = GetEdgeTicks(RightEdgeLine, 10);
    }

    /// <summary>
    /// Get the Cartesian coordinates for a point on the triangular axis
    /// </summary>
    public Coordinates GetCoordinates(double bottomFraction, double leftFraction, double rightFraction)
    {
        double x = bottomFraction;
        double y = ((1 - leftFraction) + rightFraction) / 2 * TopCorner.Y;
        return new Coordinates(x, y);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderBackground(rp, paint);

        BottomEdgeLineStyle.Render(rp.Canvas, Axes.GetPixelLine(BottomEdgeLine), paint);
        LeftEdgeLineStyle.Render(rp.Canvas, Axes.GetPixelLine(LeftEdgeLine), paint);
        RightEdgeLineStyle.Render(rp.Canvas, Axes.GetPixelLine(RightEdgeLine), paint);

        RenderTicks(rp, paint, BottomTicks, BottomTickLineStyle, BottomTickLabelStyle, 0, 5);
        RenderTicks(rp, paint, LeftTicks, LeftTickLineStyle, LeftTickLabelStyle, -5, 0);
        RenderTicks(rp, paint, RightTicks, RightTickLineStyle, RightTickLabelStyle, 5, 0);

        RenderGridLines(rp, paint, BottomTicks, RightTicks, GridLineStyle);
        RenderGridLines(rp, paint, BottomTicks, LeftTicks, GridLineStyle);
        RenderGridLines(rp, paint, LeftTicks, RightTicks, GridLineStyle);
    }

    private void RenderBackground(RenderPack rp, SKPaint paint)
    {
        Pixel[] cornerPixels = Corners.Select(x => Axes.GetPixel(x)).ToArray();
        Drawing.FillPath(rp.Canvas, paint, cornerPixels, FillStyle, rp.DataRect);
    }

    private void RenderTicks(RenderPack rp, SKPaint paint, List<TriangularTick> ticks, TickMarkStyle tickStyle, LabelStyle labelStyle, float dx, float dy)
    {
        foreach (var tick in ticks)
        {
            Pixel px1 = Axes.GetPixel(tick.Point);
            Pixel px2 = px1.WithOffset(dx, dy);
            PixelLine tickLine = new(px1, px2);
            tickStyle.Render(rp.Canvas, paint, tickLine);
            labelStyle.Render(rp.Canvas, px2, paint, tick.Label);
        }
    }

    public record struct TriangularTick(Coordinates Point, string Label);

    private static List<TriangularTick> GetEdgeTicks(CoordinateLine edgeLine, int ticksPerEdge)
    {
        List<TriangularTick> ticks = [];
        for (int i = 0; i <= ticksPerEdge; i++)
        {
            double fraction = i / (double)ticksPerEdge;
            double tickX = edgeLine.Start.X + fraction * (edgeLine.End.X - edgeLine.Start.X);
            double tickY = edgeLine.Start.Y + fraction * (edgeLine.End.Y - edgeLine.Start.Y);
            Coordinates tickPoint = new(tickX, tickY);
            string tickLabel = $"{fraction * 10}";
            ticks.Add(new(tickPoint, tickLabel));
        }

        return ticks;
    }

    private void RenderGridLines(RenderPack rp, SKPaint paint, List<TriangularTick> ticksA, List<TriangularTick> ticksB, LineStyle lineStyle, bool reverse = true)
    {
        for (int i = 0; i < ticksA.Count; i++)
        {
            int i2 = reverse ? ticksA.Count - 1 - i : i;
            CoordinateLine tickLine = new(ticksA[i].Point, ticksB[i2].Point);
            PixelLine pxLine = Axes.GetPixelLine(tickLine);
            Drawing.DrawLine(rp.Canvas, paint, pxLine, lineStyle);
        }
    }
}
