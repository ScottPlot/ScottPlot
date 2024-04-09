using ScottPlot.Interfaces;

namespace ScottPlot.Plottables;

public class VectorField(IVectorFieldSource source) : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public string? Label { get; set; }
    public ArrowStyle ArrowStyle { get; set; } = new();

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
        new LegendItem
        {
            Label = Label,
            Marker = MarkerStyle.None,
            Line = ArrowStyle.LineStyle,
            HasArrow = true
        });

    IVectorFieldSource Source { get; set; } = source;

    public AxisLimits GetAxisLimits() => Source.GetLimits();

    public void Render(RenderPack rp)
    {
        if (!ArrowStyle.LineStyle.CanBeRendered)
            return;

        float maxLength = 25;

        // TODO: Filter out those that are off-screen? This is subtle, an arrow may be fully off-screen except for its arrowhead, if the blades are long enough.
        var vectors = Source.GetRootedVectors().Select(v => new RootedPixelVector(Axes.GetPixel(v.Point), v.Vector)).ToArray();
        if (vectors.Length == 0)
            return;

        var minMagnitudeSquared = double.PositiveInfinity;
        var maxMagnitudeSquared = double.NegativeInfinity;
        foreach (var v in vectors)
        {
            var magSquared = v.MagnitudeSquared;
            minMagnitudeSquared = Math.Min(minMagnitudeSquared, magSquared);
            maxMagnitudeSquared = Math.Max(maxMagnitudeSquared, magSquared);
        }

        var range = new Range(Math.Sqrt(minMagnitudeSquared), Math.Sqrt(maxMagnitudeSquared));
        if (range.Min == range.Max)
        {
            range = new Range(0, range.Max);
        }

        for (int i = 0; i < vectors.Length; i++)
        {
            var oldMagnitude = vectors[i].Magnitude;
            var newMagnitude = range.Normalize(oldMagnitude) * maxLength;
            var direction = vectors[i].Angle;

            vectors[i].Vector = new((float)(Math.Cos(direction) * newMagnitude), (float)(Math.Sin(direction) * newMagnitude));
        }


        using SKPaint paint = new();
        ArrowStyle.LineStyle.ApplyToPaint(paint);
        paint.Style = SKPaintStyle.StrokeAndFill;

        using SKPath path = PathStrategies.Arrows.GetPath(vectors, ArrowStyle);
        rp.Canvas.DrawPath(path, paint);
    }
}
