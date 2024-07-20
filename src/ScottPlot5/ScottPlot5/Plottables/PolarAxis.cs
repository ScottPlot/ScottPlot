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
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// circles to always appear as circles instead of ellipses.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

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
            PolarAxisSpoke spoke = new(delta * i, MaximumRadius);
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
        double x = radius * Math.Cos(degrees * Math.PI / 180);
        double y = radius * Math.Sin(degrees * Math.PI / 180);
        return new Coordinates(x, y);
    }

    public AxisLimits GetAxisLimits()
    {
        double spokeAbs = 0;
        if (Spokes.Count > 0)
        {
            double minSpokeLength = Spokes.Select(x => x.Length).Min();
            double maxSpokeLength = Spokes.Select(x => x.Length).Max();
            spokeAbs = Math.Max(Math.Abs(minSpokeLength), Math.Abs(maxSpokeLength));
        }

        double circleAbs = 0;
        if (Circles.Count > 0)
        {
            double minCircleRadius = Circles.Select(x => x.Radius).Min();
            double maxCircleRadius = Circles.Select(x => x.Radius).Max();
            circleAbs = Math.Max(Math.Abs(minCircleRadius), Math.Abs(maxCircleRadius));
        }

        double maxAbs = Math.Max(spokeAbs, circleAbs);

        return maxAbs > 0
            ? new AxisLimits(-spokeAbs, spokeAbs, -spokeAbs, spokeAbs)
            : AxisLimits.NoLimits;
    }

    public virtual void UpdateAxisLimits(Plot plot)
    {
        // Square grid need to render data, so implement IManagesAxisLimits
        plot.Axes.Rules.Add(new AxisRules.SquareZoomOut(Axes.XAxis, Axes.YAxis));
    }

    public virtual void Render(RenderPack rp)
    {
        // TODO: make sure this doesn't creep up
        Debug.WriteLine($"NUMBER OF RULES: {rp.Plot.Axes.Rules.Count}");

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        Pixel origin = Axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        using SKPaint paint = new();
        Spokes.ForEach(x => x.Render(rp, Axes, paint, PaddingFraction));
        Circles.ForEach(x => x.Render(rp, Axes, paint));
    }
}
