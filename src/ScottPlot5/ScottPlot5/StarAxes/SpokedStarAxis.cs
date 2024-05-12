namespace ScottPlot.StarAxes;

public abstract class SpokedStarAxis : IStarAxis
{
    public abstract LineStyle AxisStyle { get; set; }
    public abstract void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees = 0);

    public virtual void RenderSpokes(RenderPack rp, IAxes axes, double spokeLength, int numSpokes, float rotationDegrees = 0)
    {
        var paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        var sweepAngle = 2 * Math.PI / numSpokes;
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees);

        for (int i = 0; i < numSpokes; i++)
        {
            var theta = i * sweepAngle + sweepAngle / 2;
            var x = (float)(spokeLength * Math.Cos(theta));
            var y = (float)(spokeLength * Math.Sin(theta));
            rp.Canvas.DrawLine(0, 0, x, y, paint);
        }
    }
}
