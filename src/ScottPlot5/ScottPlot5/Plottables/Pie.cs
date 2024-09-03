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
        double radius = ShowSliceLabels
            ? SliceLabelDistance + Padding
            : 1 + ExplodeFraction + Padding;

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

        // TODO: first slice should be North, not East.
        double totalValue = Slices.Sum(s => s.Value);
        var totalAngle = Angle.FromDegrees(0);
        foreach (PieSlice slice in Slices)
        {
            using SKAutoCanvasRestore _ = new(rp.Canvas);

            var sliceAngle = Angle.FromDegrees(slice.Value / totalValue * 360);
            Angle centerDegrees = totalAngle + sliceAngle / 2;

            var explosionOffset = Coordinates
                .FromPolar((float)ExplodeFraction * outerRadius, centerDegrees);
            rp.Canvas.Translate(
                (float)(origin.X + explosionOffset.X),
                (float)(origin.Y + explosionOffset.Y));

            if (sliceAngle.Degrees == 360)
            {
                path.AddOval(outerRect);
            }
            else
            {
                var ptInnerHome = Coordinates.FromPolar(innerRadius, totalAngle);
                var ptOuterHome = Coordinates.FromPolar(outerRadius, totalAngle);
                var ptInnerRotated = Coordinates.FromPolar(innerRadius, totalAngle + sliceAngle);

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

            slice.Fill.ApplyToPaint(paint, new PixelRect(origin, outerRadius));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            if (ShowSliceLabels)
            {
                var polar = Coordinates
                    .FromPolar(1.0 * SliceLabelDistance, centerDegrees);
                polar.Y = -polar.Y;
                Pixel px = Axes.GetPixel(polar) - origin;
                slice.LabelStyle.Render(rp.Canvas, px, paint);
            }

            totalAngle += sliceAngle;
            path.Reset();
        }
    }
}
