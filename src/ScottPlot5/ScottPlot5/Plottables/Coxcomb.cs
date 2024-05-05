using ScottPlot.Primitives;

namespace ScottPlot.Plottables;

public class Coxcomb : PieBase
{
    public StarAxis AxisType { get; set; } = StarAxis.Circle;
    public LineStyle AxisStyle { get; set; } = new LineStyle()
    {
        Color = Colors.DimGrey
    };

    public Coxcomb(IList<PieSlice> slices)
    {
        Slices = slices;
    }

    public override AxisLimits GetAxisLimits()
    {
        double radius = ShowSliceLabels
            ? SliceLabelDistance + Padding
            : 1 + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);
    }

    private double SliceTotal => Slices.Sum(s => s.Value);
    private float[] NormalizedSlices => Slices.Select(x => (float)(x.Value / SliceTotal)).ToArray();

    public override void Render(RenderPack rp)
    {
        RenderStarAxis(rp);

        var sliceSizes = NormalizedSlices;

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        using SKPath path = new();
        using SKPaint paint = new() { IsAntialias = true };

        var rotationPerSlice = 360f / Slices.Count;
        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(-90);


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
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotationPerSlice));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }

        if (ShowSliceLabels)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                double x = Math.Cos(rotationPerSlice * Math.PI / 180) * SliceLabelDistance;
                double y = -Math.Sin(rotationPerSlice * Math.PI / 180) * SliceLabelDistance;
                Pixel px = Axes.GetPixel(new Coordinates(x, y));
                Slices[i].LabelStyle.Render(rp.Canvas, px, paint);
            }
        }
    }

    private void RenderStarAxis(RenderPack rp)
    {
        var paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        var ticks = new float[] { 0.25f, 0.5f, 1 };
        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);
        rp.Canvas.RotateDegrees(-90);

        float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);
        var maxRadius = Math.Min(minX, minY) * NormalizedSlices.Max();

        switch (AxisType)
        {
            case StarAxis.Circle:
                foreach (var tick in ticks)
                {
                    rp.Canvas.DrawCircle(0, 0, tick * maxRadius, paint);
                }
                break;
            case StarAxis.Polygon:
                var sweepAngle = 2 * Math.PI / Slices.Count;
                foreach (var tick in ticks)
                {
                    double cumRotation = 0;
                    var path = new SKPath();
                    for (int i = 0; i < Slices.Count; i++)
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
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(AxisType));
        }
    }
}
