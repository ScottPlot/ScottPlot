namespace ScottPlot.PolarAxes;

public class RadialAxis : IRadialAxis
{
    public Spoke[] Spokes { get; }

    public double LabelDistance { get; set; } = 1.2;

    /// <summary>
    /// Generate horizontal (0 and 180 degrees) and vertical (90 and 270 degrees) radial axes
    /// </summary>
    /// <param name="length">axis line length</param>
    public RadialAxis(double length) :
        this(length, [0, 90, 180, 270])
    {
    }

    /// <summary>
    /// Generate radial axes form angle(degrees) list
    /// </summary>
    /// <param name="length">axis line length</param>
    /// <param name="angles">axis line angle list</param>
    public RadialAxis(double length, IEnumerable<double> angles) :
        this(angles?.Select(i => new Spoke(i, length))!)
    {
    }

    /// <summary>
    /// Generate radial axes form list
    /// </summary>
    /// <param name="spokes">axis line list</param>
    public RadialAxis(IEnumerable<Spoke> spokes)
    {
        Spokes = spokes?.ToArray() ?? [];
    }

    public virtual AxisLimits GetAxisLimits()
    {
        double radius = Spokes.Max(i => i.Length) * LabelDistance;
        return new AxisLimits(-radius, radius, radius, -radius);
    }

    protected virtual void RenderSpoke(RenderPack rp, IAxes axes, Spoke spoke)
    {
        var paint = new SKPaint();
        spoke.LineStyle.ApplyToPaint(paint);

        PolarCoordinates polar = spoke.GetPolarCoordinates();
        Pixel pixel = axes.GetPixel(polar) - axes.GetPixel(Coordinates.Origin);
        Drawing.DrawLine(rp.Canvas, paint, new(0, 0), pixel, spoke.LineStyle);

        var labelStyle = new LabelStyle
        {
            Text = $"{spoke.Angle}"
        };
        var labPolar = new PolarCoordinates(polar.Radial * LabelDistance, polar.Angular);
        Pixel labelPixel = axes.GetPixel(labPolar) - axes.GetPixel(Coordinates.Origin);
        PixelRect labelRect = labelStyle.Measure().Rect(Alignment.MiddleCenter);
        Pixel labelOffset = labelRect.Center - labelRect.TopLeft;
        labelPixel -= labelOffset;

        labelStyle.Render(rp.Canvas, labelPixel, paint);
    }

    public virtual void Render(RenderPack rp, IAxes axes)
    {
        if (Spokes.Length < 1)
        {
            return;
        }

        using SKAutoCanvasRestore _ = new(rp.Canvas);

        Pixel origin = axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (Spoke spoke in Spokes)
        {
            RenderSpoke(rp, axes, spoke);
        }
    }
}
