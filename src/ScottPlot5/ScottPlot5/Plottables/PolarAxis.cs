namespace ScottPlot.Plottables;

/// <summary>
/// A polar axes uses spoke lines and circles to describe a polar coordinate system
/// where points are represented by a radius and angle. 
/// This class draws a polar axes and has options to customize spokes and circles.
/// </summary>
public class PolarAxis : IPlottable, IManagesAxisLimits, IHasFill
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Spokes are straight lines that extend outward from the origin
    /// </summary>
    public List<PolarAxisSpoke> Spokes { get; } = [];

    /// <summary>
    /// Radial positions describing concentric circles centered on the origin
    /// </summary>
    public List<PolarAxisCircle> Circles { get; } = [];

    /// <summary>
    /// This value will be added to all angles, effectively rotating the polar axis.
    /// The default origin angle (0 degrees) is right in 2D Axis space.
    /// The direction of rotation is defined by <see cref="Clockwise"/>.
    /// </summary>
    public Angle Rotation { get; set; } = Angle.FromDegrees(0);

    /// <summary>
    /// Determines whether angles ascend clockwise or counter-clockwise relative to the origin.
    /// The origin can be effectively changed by setting <see cref="Rotation"/>.
    /// </summary>
    public bool Clockwise { get; set; } = false;

    /// <summary>
    /// If enabled, radial ticks will be drawn using straight lines connecting intersections circles and spokes
    /// </summary>
    public bool StraightLines { get; set; } = false;

    /// <summary>
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// circles to always appear as circles instead of ellipses.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    /// <summary>
    /// Create <paramref name="count"/> ticks (circles) evenly spaced between 0 and <paramref name="maximumRadius"/>
    /// </summary>
    public void SetCircles(double maximumRadius, int count)
    {
        double[] positions = Enumerable.Range(1, count).Select(x => maximumRadius * x / count).ToArray();
        SetCircles(positions);
    }

    /// <summary>
    /// Clear existing circles and add new ones at the defined positions.
    /// </summary>
    public void SetCircles(double[] positions)
    {
        string[] labels = new string[positions.Length];
        SetCircles(positions, labels);
    }

    /// <summary>
    /// Clear existing circles and add new ones at the defined positions with the given labels.
    /// </summary>
    public void SetCircles(double[] positions, string[] labels)
    {
        if (positions.Length != labels.Length)
            throw new ArgumentException($"{nameof(positions)} and {nameof(labels)} must have equal length");

        Circles.Clear();
        for (int i = 0; i < positions.Length; i++)
        {
            PolarAxisCircle circle = new(positions[i])
            {
                Origin = Coordinates.Origin,
                LabelText = labels[i]
            };
            Circles.Add(circle);
        }
    }

    /// <summary>
    /// Replace existing spokes with a new set evenly spaced around the circle.
    /// </summary>
    public void SetSpokes(int count, double length, bool degreeLabels = true)
    {
        Angle[] angles = new Angle[count];
        double delta = 360.0 / count;

        for (int i = 0; i < count; i++)
        {
            angles[i] = Angle.FromDegrees(delta * i);
        }

        string[] labels = degreeLabels
            ? angles.Select(x => x.Degrees.ToString()).ToArray()
            : new string[count];

        SetSpokes(angles, length, labels);
    }

    /// <summary>
    /// Replace existing spokes with new ones placed at the specified angles
    /// </summary>
    public void SetSpokes(Angle[] angles, double length, string[] labels)
    {
        if (angles.Length != labels.Length)
            throw new ArgumentException($"{nameof(angles)} and {nameof(labels)} must have equal length");

        Spokes.Clear();
        for (int i = 0; i < angles.Length; i++)
        {
            PolarAxisSpoke spoke = new(angles[i], length) { LabelText = labels[i] };
            Spokes.Add(spoke);
        }
    }


    [Obsolete("Set the Clockwise property, then call the other SetSpokes() overload", true)]
    public void SetSpokes(string[] labels, double length, bool clockwise = true) { }

    /// <summary>
    /// Replace existing spokes with new ones that have the given labels evenly spaced around the circle
    /// </summary>
    public void SetSpokes(string[] labels, double length)
    {
        Spokes.Clear();

        Angle[] angles = new Angle[labels.Length];
        double delta = 360.0 / labels.Length;
        for (int i = 0; i < labels.Length; i++)
        {
            angles[i] = Angle.FromDegrees(delta * i);
        }

        SetSpokes(angles, length, labels);
    }

    /// <summary>
    /// Return the X/Y position of a point defined in polar space
    /// </summary>
    public Coordinates GetCoordinates(double radius, double degrees)
    {
        return GetCoordinates(radius, Angle.FromDegrees(degrees));
    }

    /// <summary>
    /// Return the X/Y position of a point defined in polar space
    /// </summary>
    public Coordinates GetCoordinates(double radius, Angle angle)
    {
        return GetCoordinates(new PolarCoordinates(radius, angle));
    }

    /// <summary>
    /// Return the X/Y position of a point defined in polar space
    /// </summary>
    public Coordinates GetCoordinates(PolarCoordinates point)
    {
        Angle angleWithRotation = point.Angle + Rotation;

        if (Clockwise)
            angleWithRotation = angleWithRotation.Inverted;

        return point.WithAngle(angleWithRotation).ToCartesian();
    }

    [Obsolete("Set the Clockwise property then call the GetCoordinates() overload that does not pass it in")]
    public Coordinates[] GetCoordinates(IReadOnlyList<double> values, bool clockwise = false)
    {
        Clockwise = clockwise;
        return GetCoordinates(values);
    }

    /// <summary>
    /// Return coordinates for the given radius values assuming one value per spoke.
    /// </summary>
    public Coordinates[] GetCoordinates(IReadOnlyList<double> radiusValues)
    {
        if (radiusValues.Count != Spokes.Count)
            throw new ArgumentException($"{nameof(radiusValues)} must have a length equal to the number of spokes");

        return Enumerable
            .Range(0, Spokes.Count).Select(x => GetCoordinates(radiusValues[x], Spokes[x].Angle))
            .ToArray();
    }

    public AxisLimits GetAxisLimits()
    {
        double maxCircleRadius = Circles.Count > 0 ? Circles.Select(x => x.Radius).Max() : 0;
        double maxSpokeLabelRadius = Spokes.Count > 0 ? Spokes.Select(x => x.LabelLength).Max() : 0;
        double radius = Math.Max(maxCircleRadius, maxSpokeLabelRadius);
        return radius == 0
            ? AxisLimits.NoLimits
            : new AxisLimits(-radius, radius, -radius, radius);
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
        RenderBackgroundColor(rp, rp.Paint);
        RenderSpokes(rp, rp.Paint);

        if (StraightLines)
        {
            RenderStraightLines(rp, rp.Paint);
        }
        else
        {
            RenderCircles(rp, rp.Paint);
        }

        RenderCircleLabels(rp, rp.Paint);
    }

    protected virtual void RenderBackgroundColor(RenderPack rp, Paint paint)
    {
        double maxCircleRadius = Circles.Count > 0 ? Circles.Max(x => x.Radius) : 0;
        double maxSpokeRadius = Spokes.Count > 0 ? Spokes.Max(x => x.Length) : 0;
        double radius = Math.Max(maxCircleRadius, maxSpokeRadius);
        double pxPerUnit = rp.DataRect.Width / Axes.XAxis.Width;
        float radiusPx = (float)(pxPerUnit * radius);
        Pixel originPx = Axes.GetPixel(Coordinates.Origin);
        PixelRect rect = new(
            originPx.X - radiusPx,
            originPx.X + radiusPx,
            originPx.Y + radiusPx,
            originPx.Y - radiusPx);
        Drawing.FillOval(rp.Canvas, paint, FillStyle, rect);
    }

    private void RenderSpokes(RenderPack rp, Paint paint)
    {
        using SKAutoCanvasRestore _ = new(rp.Canvas);
        Pixel origin = Axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (var spoke in Spokes)
        {
            Angle angle = spoke.Angle + Rotation;
            if (Clockwise)
                angle = angle.Inverted;

            PolarCoordinates tipPoint = new(spoke.Length, angle);
            Pixel tipPixel = Axes.GetPixel(tipPoint.ToCartesian()) - Axes.GetPixel(Coordinates.Origin);
            Drawing.DrawLine(rp.Canvas, paint, new Pixel(0, 0), tipPixel, spoke.LineStyle);

            spoke.LabelStyle.Text = spoke.LabelText ?? string.Empty;
            //spoke.LabelStyle.Rotation = -canvasRotationDegrees;
            spoke.LabelStyle.Alignment = Alignment.MiddleCenter;

            PolarCoordinates labelPoint = new(spoke.LabelLength, tipPoint.Angle);
            Pixel labelPixel = Axes.GetPixel(labelPoint.ToCartesian()) - Axes.GetPixel(Coordinates.Origin);
            spoke.LabelStyle.Render(rp.Canvas, labelPixel.WithOffset(0, 0), paint);
        }
    }

    private void RenderCircleLabels(RenderPack rp, Paint paint)
    {
        foreach (var circle in Circles)
        {
            Coordinates c = GetCoordinates(radius: circle.Radius, angle: circle.LabelAngle);
            Pixel px = Axes.GetPixel(c);
            circle.LabelStyle.Render(rp.Canvas, px, paint, circle.LabelText);
        }
    }

    private void RenderCircles(RenderPack rp, Paint paint)
    {
        double pxPerUnit = rp.DataRect.Width / Axes.XAxis.Width;

        for (int i = 0; i < Circles.Count; i++)
        {
            float radiusPx = (float)(pxPerUnit * Circles[i].Radius);
            Pixel originPx = Axes.GetPixel(Circles[i].Origin);
            PixelRect rect = new(
                originPx.X - radiusPx,
                originPx.X + radiusPx,
                originPx.Y + radiusPx,
                originPx.Y - radiusPx);
            Drawing.DrawArc(rp.Canvas, paint, Circles[i].LineStyle, rect,
                (float)Circles[i].StartAngle.Degrees,
                (float)Circles[i].SweepAngle.Degrees);
        }
    }

    private void RenderStraightLines(RenderPack rp, Paint paint)
    {
        for (int i = 0; i < Circles.Count; i++)
        {
            Coordinates[] cs = Spokes.Select(x => GetCoordinates(Circles[i].Radius, x.Angle)).ToArray();
            Pixel[] px = cs.Select(Axes.GetPixel).ToArray();
            Drawing.DrawPath(rp.Canvas, paint, px, Circles[i].LineStyle, true);
        }
    }

    [Obsolete("use SetCircles()", true)]
    public void RegenerateCircles(int count = 3) { }

    [Obsolete("use SetSpokes()", true)]
    public void RegenerateSpokes(int count = 5) { }

    [Obsolete("use SetSpokes()", true)]
    public void RegenerateSpokes(string[] labels) { }
}
