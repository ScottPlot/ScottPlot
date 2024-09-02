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
        double total = Slices.Sum(s => s.Value);
        Angle[] sliceSizeDegrees = Slices
            .Select(x => Angle.FromDegrees(x.Value / total * 360))
            .ToArray();

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);

        // radius of the outer edge of the pie
        float outerRadius = Math.Min(minX, minY);
        SKRect outerRect = new(-outerRadius, -outerRadius, outerRadius, outerRadius);

        // radius of the inner edge of the pie when donut mode is enabled
        float innerRadius = outerRadius * (float)DonutFraction;
        SKRect innerRect = new(-innerRadius, -innerRadius, innerRadius, innerRadius);

        // radius of the outer edge after explosion
        float explosionOuterRadius = (float)ExplodeFraction * outerRadius;

        using SKPath path = new();
        using SKPaint paint = new() { IsAntialias = true };

        // TODO: first slice should be North, not East.
        var sliceOffsetDegrees = new Angle[Slices.Count];
        for (int i = 1; i < Slices.Count; i++)
        {
            sliceOffsetDegrees[i] = sliceOffsetDegrees[i - 1] + sliceSizeDegrees[i - 1];
        }

        var sliceCenterDegrees = new Angle[Slices.Count];
        for (int i = 0; i < Slices.Count; i++)
        {
            sliceCenterDegrees[i] = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
        }

        for (int i = 0; i < Slices.Count; i++)
        {
            using SKAutoCanvasRestore _ = new(rp.Canvas);

            Angle rotation = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
            rp.Canvas.Translate(origin.X, origin.Y);
            rp.Canvas.RotateDegrees((float)rotation.Degrees);
            rp.Canvas.Translate(explosionOuterRadius, 0);

            double degrees1 = -sliceSizeDegrees[i].Degrees / 2;
            double degrees2 = sliceSizeDegrees[i].Degrees / 2;

            SKPoint ptInnerHome = GetRotatedPoint(innerRadius, degrees1);
            SKPoint ptOuterHome = GetRotatedPoint(outerRadius, degrees1);
            SKPoint ptOuterRotated = GetRotatedPoint(outerRadius, degrees2);
            SKPoint ptInnerRotated = GetRotatedPoint(innerRadius, degrees2);

            if (sliceSizeDegrees[i].Degrees != 360)
            {
                path.MoveTo(ptInnerHome);
                path.LineTo(ptOuterHome);
                path.ArcTo(outerRect, (float)-sliceSizeDegrees[i].Degrees / 2, (float)sliceSizeDegrees[i].Degrees, false);
                path.LineTo(ptInnerRotated);
                path.ArcTo(innerRect, (float)sliceSizeDegrees[i].Degrees / 2, (float)-sliceSizeDegrees[i].Degrees, false);
                path.Close();
            }
            else
            {
                path.AddOval(outerRect);
            }

            Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, outerRadius));
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees((float)-rotation.Degrees));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }

        if (ShowSliceLabels)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                double x = Math.Cos(sliceCenterDegrees[i].Radians) * SliceLabelDistance;
                double y = -Math.Sin(sliceCenterDegrees[i].Radians) * SliceLabelDistance;
                Pixel px = Axes.GetPixel(new Coordinates(x, y));
                Slices[i].LabelStyle.Render(rp.Canvas, px, paint);
            }
        }
    }
}
