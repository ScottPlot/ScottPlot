using ScottPlot.Interfaces;

namespace ScottPlot.StarAxes;

public class PolygonalStarAxis : IStarAxis
{
    public LineStyle AxisStyle { get; set; } = new LineStyle()
    {
        Color = Colors.LightGray
    };

    public void Render(RenderPack rp, IAxes axes, IReadOnlyList<double> values, float rotationDegrees)
    {
        var paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        var ticks = new float[] { 0.25f, 0.5f, 1 };
        Pixel origin = axes.GetPixel(Coordinates.Origin);

        double maxSliceProportion = values.Max() / values.Sum();

        float minX = Math.Abs(axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(axes.GetPixelY(1) - origin.Y);
        var maxRadius = Math.Min(minX, minY) * maxSliceProportion;

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(rotationDegrees);

        var sweepAngle = 2 * Math.PI / values.Count;
        foreach (var tick in ticks)
        {
            double cumRotation = 0;
            var path = new SKPath();
            for (int i = 0; i < values.Count; i++)
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
            rp.Canvas.DrawPath(path, paint);
        }
    }
}
