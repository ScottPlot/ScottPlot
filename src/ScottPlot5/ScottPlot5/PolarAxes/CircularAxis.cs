namespace ScottPlot.PolarAxes;

public class CircularAxis : ICircularAxis
{
    public CircularAxisLine[] AxisLines { get; }

    /// <summary>
    /// Generate circular axes form list
    /// </summary>
    /// <param name="radii">axis radius list</param>
    public CircularAxis(IEnumerable<double> radii) :
        this(radii?.Select(i => new CircularAxisLine(i))!)
    {
    }

    /// <summary>
    /// Generate circular axes form list
    /// </summary>
    /// <param name="radii">axis line list</param>
    public CircularAxis(IEnumerable<CircularAxisLine> radii)
    {
        AxisLines = radii?.ToArray() ?? [];
    }

    public virtual AxisLimits GetAxisLimits()
    {
        double radius = AxisLines.Max(i => i.Value);
        return new AxisLimits(-radius, radius, radius, -radius);
    }

    public void Render(RenderPack rp, IAxes axes)
    {
        if (AxisLines.Length < 1)
        {
            return;
        }

        var paint = new SKPaint();
        using SKAutoCanvasRestore _ = new(rp.Canvas);

        Pixel origin = axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (var axisLine in AxisLines)
        {
            float pixelX = axes.GetPixelX(axisLine.Value) - origin.X;
            float pixelY = axes.GetPixelY(axisLine.Value) - origin.Y;
            Drawing.DrawOval(
                rp.Canvas,
                paint,
                axisLine.LineStyle,
                new PixelRect(-pixelX, pixelX, pixelY, -pixelY));
        }
    }
}
