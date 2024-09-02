namespace ScottPlot.Plottables;

/// <summary>
/// A polar axes uses spoke lines and circles to describe a polar coordinate system
/// where points are represented by a radius and angle. 
/// This class draws a polar axes and has options to customize spokes and circles.
/// </summary>
public class PolarAxis : IPlottable, IManagesAxisLimits
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Spokes are straight lines that extend outward from the origin
    /// </summary>
    public List<PolarAxisSpoke> Spokes { get; } = [];

    /// <summary>
    /// Concentric circles centered on the origin
    /// </summary>
    public List<PolarAxisCircle> Circles { get; } = [];

    /// <summary>
    /// Size of the largest circle or longest spoke
    /// </summary>
    public double MaximumRadius { get; set; } = 1;

    /// <summary>
    /// Additional padding given to accommodate labels
    /// </summary>
    public double PaddingFraction { get; set; } = 1.1;

    /// <summary>
    /// Rotates the axis from its default position (where 0 points right)
    /// </summary>
    public double RotationDegrees { get; set; } = 0;

    /// <summary>
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// circles to always appear as circles instead of ellipses.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    /// <summary>
    /// If enabled, radial ticks will be drawn using straight lines connecting intersections circles and spokes
    /// </summary>
    public bool StraightLines { get; set; } = false;

    /// <summary>
    /// Sets line width for all <see cref="Circles"/> and <see cref="Spokes"/>
    /// </summary>
    public float LineWidth
    {
        set
        {
            Circles.ForEach(x => x.LineWidth = value);
            Spokes.ForEach(x => x.LineWidth = value);
        }
    }

    /// <summary>
    /// Sets line pattern for all <see cref="Circles"/> and <see cref="Spokes"/>
    /// </summary>
    public LinePattern LinePattern
    {
        set
        {
            Circles.ForEach(x => x.LinePattern = value);
            Spokes.ForEach(x => x.LinePattern = value);
        }
    }

    /// <summary>
    /// Sets line color for all <see cref="Circles"/> and <see cref="Spokes"/>
    /// </summary>
    public Color LineColor
    {
        set
        {
            Circles.ForEach(x => x.LineColor = value);
            Spokes.ForEach(x => x.LineColor = value);
        }
    }

    /// <summary>
    /// Replace spokes with a new collection evenly-spaced around the circle
    /// </summary>
    public void RegenerateSpokes(int count = 5)
    {
        Spokes.Clear();
        if (count < 1)
        {
            return;
        }

        double delta = 360.0 / count;
        for (int i = 0; i < count; i++)
        {
            Angle angle = Angle.FromDegrees(delta * i);
            PolarAxisSpoke spoke = new(angle, MaximumRadius);
            Spokes.Add(spoke);
        }
    }

    /// <summary>
    /// Replace spokes with a new collection evenly-spaced around the circle labeled with the given strings
    /// </summary>
    public void RegenerateSpokes(string[] labels)
    {
        Spokes.Clear();
        if (labels.Length < 1)
        {
            return;
        }

        double delta = 360.0 / labels.Length;
        for (int i = 0; i < labels.Length; i++)
        {
            Angle angle = Angle.FromDegrees(delta * i);
            PolarAxisSpoke spoke = new(angle, MaximumRadius) { LabelText = labels[i] };
            Spokes.Add(spoke);
        }
    }

    /// <summary>
    /// Replace circles with a new collection evenly-spaced along the maximum radius
    /// </summary>
    public void RegenerateCircles(int count = 3)
    {
        Circles.Clear();
        if (count < 1)
        {
            return;
        }

        if (count == 1)
        {
            PolarAxisCircle circle = new(MaximumRadius);
            Circles.Add(circle);
            return;
        }

        double delta = MaximumRadius / count;
        for (int i = 0; i < count; i++)
        {
            PolarAxisCircle circle = new(delta * (i + 1));
            Circles.Add(circle);
        }
    }

    /// <summary>
    /// Return the X/Y position of a polar point
    /// </summary>
    public Coordinates GetCoordinates(double radius, double degrees)
    {
        degrees -= RotationDegrees;
        double x = radius * Math.Cos(degrees * Math.PI / 180);
        double y = radius * Math.Sin(degrees * Math.PI / 180);
        return new Coordinates(x, y);
    }

    /// <summary>
    /// Return the X/Y position of a polar point
    /// </summary>
    public Coordinates GetCoordinates(double radius, Angle angle)
    {
        return GetCoordinates(radius, angle.Degrees);
    }

    /// <summary>
    /// Return the X/Y position of a polar point
    /// </summary>
    public Coordinates GetCoordinates(PolarCoordinates point)
    {
        return GetCoordinates(point.Radius, point.Angle.Degrees);
    }

    /// <summary>
    /// Return coordinates for the given radius values assuming one value per spoke.
    /// </summary>
    public Coordinates[] GetCoordinates(IReadOnlyList<double> values)
    {
        if (values.Count != Spokes.Count)
            throw new ArgumentException($"{nameof(values)} must have a length equal to the number of spokes");

        return Enumerable.Range(0, Spokes.Count)
            .Select(x => GetCoordinates(values[x], Spokes[x].Angle))
            .ToArray();
    }

    public AxisLimits GetAxisLimits()
    {
        double radius = MaximumRadius * PaddingFraction;
        return new AxisLimits(-radius, radius, -radius, radius);
    }

    public virtual void UpdateAxisLimits(Plot plot)
    {
        foreach (IAxisRule rule in plot.Axes.Rules)
        {
            if (rule is AxisRules.SquareZoomOut)
                return;
        }

        AxisRules.SquareZoomOut squareRule = new(Axes.XAxis, Axes.YAxis);
        plot.Axes.Rules.Add(squareRule);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKAutoCanvasRestore _ = new(rp.Canvas);
        Pixel origin = Axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees((float)RotationDegrees);

        using SKPaint paint = new();
        Spokes.ForEach(x => x.Render(rp, Axes, paint, PaddingFraction, RotationDegrees));

        _.Dispose();

        if (StraightLines)
        {
            RenderStraightLines(rp, paint);
        }
        else
        {
            RenderCircles(rp, paint);
        }
    }

    private void RenderCircles(RenderPack rp, SKPaint paint)
    {
        Pixel originPx = Axes.GetPixel(Coordinates.Origin);
        double pxPerUnit = rp.DataRect.Width / Axes.XAxis.Width;

        for (int i = 0; i < Circles.Count; i++)
        {
            float radiusPx = (float)(pxPerUnit * Circles[i].Radius);
            Drawing.DrawCircle(rp.Canvas, originPx, radiusPx, Circles[i].LineStyle, paint);
        }
    }

    private void RenderStraightLines(RenderPack rp, SKPaint paint)
    {
        for (int i = 0; i < Circles.Count; i++)
        {
            Coordinates[] cs = Spokes.Select(x => GetCoordinates(Circles[i].Radius, x.Angle)).ToArray();
            Pixel[] px = cs.Select(Axes.GetPixel).ToArray();
            Drawing.DrawPath(rp.Canvas, paint, px, Circles[i].LineStyle, true);
        }
    }
}
