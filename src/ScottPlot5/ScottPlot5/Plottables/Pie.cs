namespace ScottPlot.Plottables;

public class Pie : PieBase
{
    public double ExplodeFraction { get; set; } = 0;
    public double DonutFraction { get; set; } = 0;

    public Pie(IList<PieSlice> slices)
    {
        Slices = slices;
    }

    public override AxisLimits GetAxisLimits()
    {
        double radius = Math.Max(SliceLabelDistance, 1 + ExplodeFraction) + Padding;
        return new AxisLimits(-radius, radius, -radius, radius);
    }

    public override void Render(RenderPack rp)
    {
        using SKPath path = new();
        using SKPaint paint = new() { IsAntialias = true };

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);

        // radius of the outer edge of the pie
        float outerRadius = Math.Min(minX, minY);
        SKRect outerRect = new(-outerRadius, -outerRadius, outerRadius, outerRadius);

        // radius of the inner edge of the pie when donut mode is enabled
        float innerRadius = outerRadius * (float)DonutFraction;
        SKRect innerRect = new(-innerRadius, -innerRadius, innerRadius, innerRadius);

        double totalValue = Slices.Sum(s => s.Value);
        Angle totalAngle = Rotation;
        foreach (PieSlice slice in Slices)
        {
            using SKAutoCanvasRestore _ = new(rp.Canvas);

            var percentage = slice.Value / totalValue;
            var sliceAngle = Angle.FromDegrees(percentage * 360);
            Angle centerAngle = totalAngle + sliceAngle / 2;

            Coordinates explosionOffset = new PolarCoordinates(ExplodeFraction * outerRadius, centerAngle).ToCartesian();
            rp.Canvas.Translate(
                (float)(origin.X + explosionOffset.X),
                (float)(origin.Y + explosionOffset.Y));

            if (sliceAngle.Degrees == 360)
            {
                if (DonutFraction > 0)
                {
                    // Clip inner oval
                    // avoid clipping to inner oval line
                    float innerRadius2 = innerRadius - 1;
                    path.AddOval(new SKRect(
                        -innerRadius2, -innerRadius2, innerRadius2, innerRadius2));
                    rp.Canvas.ClipPath(path, SKClipOperation.Difference);
                    path.Reset();

                    // Draw inner oval line
                    path.AddOval(innerRect);
                    LineStyle.ApplyToPaint(paint);
                    rp.Canvas.DrawPath(path, paint);
                }

                path.AddOval(outerRect);
            }
            else
            {
                Coordinates ptInnerHome = new PolarCoordinates(innerRadius, totalAngle).ToCartesian();
                Coordinates ptOuterHome = new PolarCoordinates(outerRadius, totalAngle).ToCartesian();
                Coordinates ptInnerRotated = new PolarCoordinates(innerRadius, totalAngle + sliceAngle).ToCartesian();

                path.MoveTo(new SKPoint((float)ptInnerHome.X, (float)ptInnerHome.Y));
                path.LineTo(new SKPoint((float)ptOuterHome.X, (float)ptOuterHome.Y));
                path.ArcTo(outerRect,
                    (float)totalAngle.Degrees,
                    (float)sliceAngle.Degrees,
                    false);
                path.LineTo(new SKPoint((float)ptInnerRotated.X, (float)ptInnerRotated.Y));
                path.ArcTo(innerRect,
                    (float)(totalAngle + sliceAngle).Degrees,
                    (float)-sliceAngle.Degrees,
                    false);
                path.Close();
            }

            PixelRect rect = new(origin, outerRadius);
            Drawing.DrawPath(rp.Canvas, paint, path, slice.Fill, rect);

            Drawing.DrawPath(rp.Canvas, paint, path, LineStyle);

            Coordinates polar = new PolarCoordinates(1.0 * SliceLabelDistance, centerAngle).ToCartesian();
            polar.Y = -polar.Y;
            Pixel px = Axes.GetPixel(polar) - origin;
            slice.LabelStyle.Render(rp.Canvas, px, paint);

            totalAngle += sliceAngle;
            path.Reset();
        }
    }
}
