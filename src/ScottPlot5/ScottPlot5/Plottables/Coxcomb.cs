
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
        double radius = ShowSliceLabels
            ? SliceLabelDistance + Padding
            : 1 + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);
    }

    private double SliceTotal => Slices.Sum(s => s.Value);
    private float[] NormalizedSlices => Slices.Select(x => (float)(x.Value / SliceTotal)).ToArray();

    public override void Render(RenderPack rp)
    {
        StarAxis.Render(rp, Axes, Slices.Select(s => s.Value).ToArray(), -90);

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
}
