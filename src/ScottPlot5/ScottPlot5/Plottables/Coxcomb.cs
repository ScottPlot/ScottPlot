
namespace ScottPlot.Plottables;

public class Coxcomb : PieBase
{
    public IStarAxis StarAxis { get; set; } = new StarAxes.CircularStarAxis();

    public Coxcomb(IList<PieSlice> slices)
    {
        Slices = slices;
    }

    public override AxisLimits GetAxisLimits()
    {
        double maxRadius = NormalizedSlices.Max();

        double radius = ShowSliceLabels
            ? maxRadius * SliceLabelDistance + Padding
            : maxRadius + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);
    }

    private double SliceTotal => Slices.Sum(s => s.Value);
    private float[] NormalizedSlices => Slices.Select(x => (float)(x.Value / SliceTotal)).ToArray();

    public override void Render(RenderPack rp)
    {
        const float startAngle = -90;

        var sliceSizes = NormalizedSlices;
        double maxRadius = NormalizedSlices.Max();
        StarAxis.Render(rp, Axes, maxRadius, Slices.Count, startAngle);

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        using SKPath path = new();
        using SKPaint paint = new() { IsAntialias = true };

        var rotationPerSlice = 360f / Slices.Count;

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(startAngle);

        for (int i = 0; i < Slices.Count; i++)
        {
            rp.Canvas.RotateDegrees(rotationPerSlice);

            float degrees1 = 0f;

            SKPoint ptInner = GetRotatedPoint(sliceSizes[i], degrees1); // Unlike piecharts this is unique (there's no donut coxcomb charts)
            SKPoint ptOuterHome = GetRotatedPoint(sliceSizes[i], degrees1);

            float minX = Math.Abs(Axes.GetPixelX(sliceSizes[i]) - origin.X);
            float minY = Math.Abs(Axes.GetPixelY(sliceSizes[i]) - origin.Y);
            var radius = Math.Min(minX, minY);
            var rect = new SKRect(-radius, -radius, radius, radius);

            if (rotationPerSlice != 360)
            {
                path.MoveTo(ptInner);
                path.LineTo(ptOuterHome);
                path.ArcTo(rect, 0, rotationPerSlice, false);
                path.Close();
            }
            else
            {
                path.AddOval(rect);
            }

            Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, radius));
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotationPerSlice * (i + 1) - startAngle));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();

            if (ShowSliceLabels)
            {
                double cumulativeRotation = (i + 1) * rotationPerSlice;
                double x = SliceLabelDistance * maxRadius * Math.Cos(-(cumulativeRotation + startAngle + rotationPerSlice / 2) * Math.PI / 180);
                double y = SliceLabelDistance * maxRadius * Math.Sin(-(cumulativeRotation + startAngle + rotationPerSlice / 2) * Math.PI / 180);
                Pixel px = Axes.GetPixel(new Coordinates(x, y));

                using var textTransform = new SKAutoCanvasRestore(rp.Canvas);
                rp.Canvas.RotateDegrees((float)-cumulativeRotation - startAngle);
                rp.Canvas.Translate(-origin.X, -origin.Y);

                Slices[i].LabelStyle.Render(rp.Canvas, px, paint);

            }
        }
    }
}
