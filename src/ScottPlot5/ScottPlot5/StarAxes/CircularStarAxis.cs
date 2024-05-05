namespace ScottPlot.StarAxes;

public class CircularStarAxis : SpokedStarAxis
{
    public override LineStyle AxisStyle { get; set; } = new LineStyle()
    {
        Color = Colors.DarkGray
    };

    public override void Render(RenderPack rp, IAxes axes, IReadOnlyList<double> values, float rotationDegrees)
    {

        var paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        var ticks = new float[] { 0.25f, 0.5f, 1 };
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        double maxSliceProportion = values.Max() / values.Sum();

        float minX = Math.Abs(axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(axes.GetPixelY(1) - origin.Y);
        var maxRadius = Math.Min(minX, minY) * maxSliceProportion;

        RenderSpokes(rp, axes, values.Count, maxRadius, rotationDegrees);

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees); // Won't matter for the circles, but will if and when we add spokes

        foreach (var tick in ticks)
        {
            rp.Canvas.DrawCircle(0, 0, (float)(tick * maxRadius), paint);
        }
    }
}
