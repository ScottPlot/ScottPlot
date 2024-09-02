using System.Reflection.Emit;

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
    /// Place spoke labels this fraction of their length from the origin
    /// </summary>
    public double SpokeLabelPositionFraction { get; set; } = 1.2;

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
    public void RegenerateSpokes(int count = 5, double lengthFraction = 1.1)
    {
        string[] labels = new string[count];
        SetSpokes(labels, lengthFraction);
    }

    /// <summary>
    /// Create <paramref name="count"/> ticks (circles) evenly spaced between 0 and <paramref name="maximumRadius"/>
    /// </summary>
    public void SetTicks(double maximumRadius, int count)
    {
        double[] positions = Enumerable.Range(1, count).Select(x => maximumRadius * x / count).ToArray();
        SetTicks(positions);
    }

    /// <summary>
    /// Clear existing circles and add new ones at the defined positions.
    /// </summary>
    public void SetTicks(double[] positions)
    {
        string[] labels = new string[positions.Length];
        SetTicks(positions, labels);
    }

    /// <summary>
    /// Clear existing circles and add new ones at the defined positions with the given labels.
    /// </summary>
    public void SetTicks(double[] positions, string[] labels)
    {
        if (positions.Length != labels.Length)
            throw new ArgumentException($"{nameof(positions)} and {nameof(labels)} must have equal length");

        Circles.Clear();
        for (int i = 0; i < positions.Length; i++)
        {
            PolarAxisCircle circle = new(positions[i]) { LabelText = labels[i] };
            Circles.Add(circle);
        }
    }

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
        double maxCircleRadius = Circles.Count > 0 ? Circles.Select(x => x.Radius).Max() : 0;
        double maxSpokeRadius = Spokes.Count > 0 ? Spokes.Select(x => x.Length).Max() : 0;
        double maxRadius = Math.Max(maxCircleRadius, maxSpokeRadius);
        if (maxRadius == 0)
            return AxisLimits.NoLimits;

        double radius = maxRadius * SpokeLabelPositionFraction;
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
        using SKPaint paint = new();
        RenderSpokes(rp, paint);

        if (StraightLines)
        {
            RenderStraightLines(rp, paint);
        }
        else
        {
            RenderCircles(rp, paint);
        }

        RenderCircleLabels(rp, paint);
    }

    private void RenderSpokes(RenderPack rp, SKPaint paint)
    {
        using SKAutoCanvasRestore _ = new(rp.Canvas);
        Pixel origin = Axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees((float)RotationDegrees);

        foreach (var spoke in Spokes)
        {
            PolarCoordinates tipPoint = new(spoke.Length, spoke.Angle);
            Pixel tipPixel = Axes.GetPixel(tipPoint.CartesianCoordinates) - Axes.GetPixel(Coordinates.Origin);
            Drawing.DrawLine(rp.Canvas, paint, new Pixel(0, 0), tipPixel, spoke.LineStyle);

            spoke.LabelStyle.Text = spoke.LabelText ?? string.Empty;
            spoke.LabelStyle.Rotation = -(float)RotationDegrees;
            spoke.LabelStyle.Alignment = Alignment.MiddleCenter;

            PolarCoordinates labelPoint = new(tipPoint.Radius * SpokeLabelPositionFraction, tipPoint.Angle);
            Pixel labelPixel = Axes.GetPixel(labelPoint.CartesianCoordinates) - Axes.GetPixel(Coordinates.Origin);
            spoke.LabelStyle.Render(rp.Canvas, labelPixel.WithOffset(0, 0), paint);
        }
    }

    private void RenderCircleLabels(RenderPack rp, SKPaint paint)
    {
        foreach (var circle in Circles)
        {
            Coordinates c = GetCoordinates(radius: circle.Radius, angle: circle.LabelAngle);
            Pixel px = Axes.GetPixel(c);
            circle.LabelStyle.Render(rp.Canvas, px, paint, circle.LabelText);
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
