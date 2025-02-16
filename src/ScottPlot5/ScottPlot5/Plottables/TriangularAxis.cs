namespace ScottPlot.Plottables;

/// <summary>
/// This plot type places a triangular axis on the plot 
/// and has methods to convert between triangular and Cartesian coordinates.
/// </summary>
public class TriangularAxis(bool clockwise) : IPlottable
{
    public bool Clockwise { get; } = clockwise;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public double Padding { get; set; } = 0.25;
    public AxisLimits GetAxisLimits() => TriangularAxisEdge.AxisLimits.WithZoom(1 - Padding, 1 - Padding);

    public FillStyle FillStyle { get; set; } = new() { IsVisible = true, Color = Colors.Yellow.WithAlpha(.1), };
    public LineStyle GridLineStyle { get; set; } = new() { IsVisible = true, Width = 1, Color = Colors.Gray.WithAlpha(.2) };

    public TriangularAxisEdge Left { get; } = TriangularAxisEdge.Left(clockwise);
    public TriangularAxisEdge Right { get; } = TriangularAxisEdge.Right(clockwise);
    public TriangularAxisEdge Bottom { get; } = TriangularAxisEdge.Bottom(clockwise);

    /// <summary>
    /// Return coordinates for a point on the triangle for a fractional distance 
    /// (0 through 1, inclusive) along the bottom and left axes.
    /// </summary>
    public Coordinates GetCoordinates(double bottomFraction, double leftFraction)
    {
        double x = Bottom.GetCoordinates(bottomFraction).X;
        double y = Left.GetCoordinates(leftFraction).Y;
        return new Coordinates(x, y);
    }

    /// <summary>
    /// Converts ternary coordinates (bottom, left, right) to Cartesian coordinates (X, Y)
    /// </summary>
    public Coordinates GetCoordinates(double bottomFraction, double leftFraction, double rightFraction)
    {
        if (Math.Abs(bottomFraction + leftFraction + rightFraction - 1) > 1e-6)
        {
            throw new ArgumentException("The sum of bottom, left, and right fractions must equal 1.");
        }

        double x, y;

        if (!Clockwise)
        {
            // Counterclockwise transformation
            x = 0.5 * (2 * bottomFraction + rightFraction);
            y = (Math.Sqrt(3) / 2) * rightFraction;
        }
        else
        {
            // Clockwise transformation
            x = 0.5 * (2 * rightFraction + leftFraction);
            y = (Math.Sqrt(3) / 2) * leftFraction;
        }

        return new Coordinates(x, y);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        RenderBackground(rp, paint);

        RenderEdge(rp, paint, Left);
        RenderEdge(rp, paint, Right);
        RenderEdge(rp, paint, Bottom);

        RenderGridLines(rp, paint, Bottom, Right);
        RenderGridLines(rp, paint, Bottom, Left);
        RenderGridLines(rp, paint, Left, Right);
    }

    private void RenderBackground(RenderPack rp, SKPaint paint)
    {
        Coordinates[] corners = [Left.Start, Right.Start, Bottom.Start];
        var cornerPixels = corners.Select(Axes.GetPixel);
        Drawing.FillPath(rp.Canvas, paint, cornerPixels, FillStyle, rp.DataRect);
    }

    private void RenderEdge(RenderPack rp, SKPaint paint, TriangularAxisEdge edge)
    {
        // edge line
        Drawing.DrawLine(rp.Canvas, paint, Axes.GetPixelLine(edge.Line), edge.EdgeLineStyle);

        // ticks
        foreach (var tick in edge.Ticks)
        {
            Pixel px1 = Axes.GetPixel(tick.Point);
            Pixel px2 = px1.WithOffset(edge.TickOffset);
            PixelLine tickLine = new(px1, px2);
            edge.TickMarkStyle.Render(rp.Canvas, paint, tickLine);
            edge.TickLabelStyle.Render(rp.Canvas, px2, paint, tick.Label);
        }

        // label
        edge.LabelStyle.Render(rp.Canvas, Axes.GetPixel(edge.Line.Center), paint, edge.LabelText);
    }

    private void RenderGridLines(RenderPack rp, SKPaint paint, TriangularAxisEdge edge1, TriangularAxisEdge edge2, bool reverse = true)
    {
        for (int i = 0; i < edge1.Ticks.Count; i++)
        {
            int i2 = reverse ? edge1.Ticks.Count - 1 - i : i;
            CoordinateLine tickLine = new(edge1.Ticks[i].Point, edge2.Ticks[i2].Point);
            PixelLine pxLine = Axes.GetPixelLine(tickLine);
            Drawing.DrawLine(rp.Canvas, paint, pxLine, GridLineStyle);
        }
    }
}
