namespace ScottPlot.Plottables;

public class Pie : PieBase
{
    public double Radius { get; set; } = 1.0;
    public double ExplodeFraction { get; set; } = 0;
    public double DonutFraction { get; set; } = 0;

    public Pie(IList<PieSlice> slices)
    {
        Slices = slices;
    }

    public override AxisLimits GetAxisLimits()
    {
        double radius = Math.Max(SliceLabelDistance, Radius + ExplodeFraction) + Padding;
        return new AxisLimits(-radius, radius, -radius, radius);
    }

    protected virtual void RenderDonutSlice(
        RenderPack rp, Paint paint, LineStyle lineStyle, FillStyle fillStyle,
        float radius, float sliceAngle, float startAngle)
    {
        PixelRect outerRect = new(-radius, radius, -radius, radius);

        // radius of the inner edge of the pie when donut mode is enabled
        float innerRadius = radius * (float)DonutFraction;
        PixelRect innerRect = new(-innerRadius, innerRadius, -innerRadius, innerRadius);

        if (Math.Abs(sliceAngle) < 360)
        {
            Drawing.FillAnnularSector(rp.Canvas, paint, fillStyle, outerRect, innerRect, startAngle, sliceAngle);
            Drawing.DrawAnnularSector(rp.Canvas, paint, lineStyle, outerRect, innerRect, startAngle, sliceAngle);
        }
        else
        {
            Drawing.FillEllipticalAnnulus(rp.Canvas, paint, fillStyle, outerRect, innerRect);
            Drawing.DrawEllipticalAnnulus(rp.Canvas, paint, lineStyle, outerRect, innerRect);
        }
    }

    protected virtual void RenderSlice(
        RenderPack rp, Paint paint, LineStyle lineStyle, FillStyle fillStyle,
        float radius, float sliceAngle, float startAngle)
    {
        PixelRect outerRect = new(-radius, radius, -radius, radius);

        if (Math.Abs(sliceAngle) < 360)
        {
            Drawing.FillSector(rp.Canvas, paint, fillStyle, outerRect, startAngle, sliceAngle);
            Drawing.DrawSector(rp.Canvas, paint, lineStyle, outerRect, startAngle, sliceAngle);
        }
        else
        {
            Drawing.FillOval(rp.Canvas, paint, fillStyle, outerRect);
            Drawing.DrawOval(rp.Canvas, paint, lineStyle, outerRect);
        }
    }

    public override void Render(RenderPack rp)
    {
        using Paint paint = new() { IsAntialias = true };

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(Axes.GetPixelX(Radius) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(Radius) - origin.Y);

        // radius of the outer edge of the pie
        float outerRadius = Math.Min(minX, minY);

        double totalValue = Slices.Sum(s => s.Value);
        Angle totalAngle = Rotation;
        foreach (PieSlice slice in Slices)
        {
            using SKAutoCanvasRestore _ = new(rp.Canvas);

            Angle sliceAngle = Angle.FromFraction(slice.Value / totalValue);
            Angle centerAngle = totalAngle + sliceAngle / 2;

            PolarCoordinates slicePolar = new(ExplodeFraction * outerRadius, centerAngle);
            Coordinates explosionOffset = slicePolar.ToCartesian();
            rp.Canvas.Translate(
                (float)(origin.X + explosionOffset.X),
                (float)(origin.Y + explosionOffset.Y));

            if (DonutFraction > 0)
            {
                RenderDonutSlice(rp, paint, LineStyle, slice.Fill, outerRadius, (float)sliceAngle.Degrees, (float)totalAngle.Degrees);
            }
            else
            {
                RenderSlice(rp, paint, LineStyle, slice.Fill, outerRadius, (float)sliceAngle.Degrees, (float)totalAngle.Degrees);
            }

            Coordinates textPolar = slicePolar
                .WithRadius(Radius * SliceLabelDistance)
                .ToCartesian();
            textPolar.Y = -textPolar.Y;
            Pixel px = Axes.GetPixel(textPolar) - origin;
            slice.LabelStyle.Render(rp.Canvas, px, paint);

            totalAngle += sliceAngle;
        }
    }
}
