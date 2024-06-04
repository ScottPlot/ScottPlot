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
        float[] sliceSizeDegrees = Slices.Select(x => (float)(x.Value / total) * 360).ToArray();

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
        float[] sliceOffsetDegrees = new float[Slices.Count];
        for (int i = 1; i < Slices.Count; i++)
        {
            sliceOffsetDegrees[i] = sliceOffsetDegrees[i - 1] + sliceSizeDegrees[i - 1];
        }

        float[] sliceCenterDegrees = new float[Slices.Count];
        for (int i = 0; i < Slices.Count; i++)
        {
            sliceCenterDegrees[i] = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
        }

        for (int i = 0; i < Slices.Count; i++)
        {
            using SKAutoCanvasRestore _ = new(rp.Canvas);

            float rotation = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
            rp.Canvas.Translate(origin.X, origin.Y);
            rp.Canvas.RotateDegrees(rotation);
            rp.Canvas.Translate(explosionOuterRadius, 0);

            float degrees1 = -sliceSizeDegrees[i] / 2;
            float degrees2 = sliceSizeDegrees[i] / 2;

            SKPoint ptInnerHome = GetRotatedPoint(innerRadius, degrees1);
            SKPoint ptOuterHome = GetRotatedPoint(outerRadius, degrees1);
            SKPoint ptOuterRotated = GetRotatedPoint(outerRadius, degrees2);
            SKPoint ptInnerRotated = GetRotatedPoint(innerRadius, degrees2);

            if (sliceSizeDegrees[i] != 360)
            {
                path.MoveTo(ptInnerHome);
                path.LineTo(ptOuterHome);
                path.ArcTo(outerRect, -sliceSizeDegrees[i] / 2, sliceSizeDegrees[i], false);
                path.LineTo(ptInnerRotated);
                path.ArcTo(innerRect, sliceSizeDegrees[i] / 2, -sliceSizeDegrees[i], false);
                path.Close();
            }
            else
            {
                path.AddOval(outerRect);
            }

            Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, outerRadius));
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotation));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }

        if (ShowSliceLabels)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                double x = Math.Cos(sliceCenterDegrees[i] * Math.PI / 180) * SliceLabelDistance;
                double y = -Math.Sin(sliceCenterDegrees[i] * Math.PI / 180) * SliceLabelDistance;
                Pixel px = Axes.GetPixel(new Coordinates(x, y));
                Slices[i].LabelStyle.Render(rp.Canvas, px, paint);
            }
        }
    }
}
