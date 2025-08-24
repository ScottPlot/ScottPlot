namespace ScottPlot.StarAxes;

public class PolygonalStarAxis : SpokedStarAxis
{
    public override LineStyle LineStyle { get; set; } = new LineStyle()
    {
        Color = Colors.DarkGray
    };

    public override void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees)
    {
        var ticks = new float[] { 0.25f, 0.5f, 1 };
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(axes.GetPixelY(1) - origin.Y);
        var maxRadius = Math.Min(minX, minY) * maxSpokeLength;

        RenderSpokes(rp, axes, maxRadius, numSpokes, rotationDegrees);

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees);

        var sweepAngle = 2 * Math.PI / numSpokes;
        foreach (var tick in ticks)
        {
            double cumRotation = 0;
            var path = new SKPath();
            for (int i = 0; i < numSpokes; i++)
            {
                var theta = cumRotation + sweepAngle / 2;
                var x = (float)(tick * maxRadius * Math.Cos(theta));
                var y = (float)(tick * maxRadius * Math.Sin(theta));
                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);

                cumRotation += sweepAngle;
            }
            path.Close();

            Drawing.DrawPath(rp.Canvas, rp.Paint, path, LineStyle);
        }

    }
}
