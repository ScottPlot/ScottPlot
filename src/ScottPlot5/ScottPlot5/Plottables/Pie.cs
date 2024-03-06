namespace ScottPlot.Plottables;

public class Pie : IPlottable
{
    public IList<PieSlice> Slices { get; set; }
    public LineStyle LineStyle { get; set; } = new() { Width = 0 };
    public bool IsVisible { get; set; } = true;
    public double ExplodeFraction { get; set; } = 0;
    public double SliceLabelDistance { get; set; } = 1.2;
    public bool ShowSliceLabels { get; set; } = false;
    public double Padding { get; set; } = 0.2;
    public double DonutSize { get; set; } = 0;

    public IAxes Axes { get; set; } = new Axes();

    public Pie(IList<PieSlice> slices)
    {
        Slices = slices;
    }

    public AxisLimits GetAxisLimits()
    {
        double radius = ShowSliceLabels
            ? SliceLabelDistance + Padding
            : 1 + ExplodeFraction + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);

    }
    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
        new LegendItem
        {
            Children = Slices.Select(slice => new LegendItem
            {
                Label = slice.Label,
                Fill = slice.Fill
            })
        });

    public void Render(RenderPack rp)
    {
        double total = Slices.Sum(s => s.Value);
        float[] sliceSizeDegrees = Slices.Select(x => (float)(x.Value / total) * 360).ToArray();

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);
        float radius = Math.Min(minX, minY);
        float explosionRadius = (float)ExplodeFraction * radius;
        SKRect rect = new(-radius, -radius, radius, radius);

        using SKPath path = new();
        using SKPaint paint = new() { IsAntialias = true };

        SKPath donutClipPath = null;
        if (DonutSize > 0)
        {
            donutClipPath = GetDonutClipPath(rect, radius * (float)DonutSize);
        }

        // TODO: first slice should be North, not East.        
        float[] sliceOffsetDegrees = new float[Slices.Count];
        for (int i = 1; i < Slices.Count(); i++)
        {
            sliceOffsetDegrees[i] = sliceOffsetDegrees[i - 1] + sliceSizeDegrees[i - 1];
        }

        float[] sliceCenterDegrees = new float[Slices.Count];
        for (int i = 0; i < Slices.Count(); i++)
        {
            sliceCenterDegrees[i] = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
        }

        for (int i = 0; i < Slices.Count(); i++)
        {
            using var _ = new SKAutoCanvasRestore(rp.Canvas);

            float rotation = sliceOffsetDegrees[i] + sliceSizeDegrees[i] / 2;
            rp.Canvas.Translate(origin.X, origin.Y);
            rp.Canvas.RotateDegrees(rotation);
            rp.Canvas.Translate(explosionRadius, 0);

            if (sliceSizeDegrees[i] != 360)
            {
                path.MoveTo(0, 0);
                path.ArcTo(rect, -sliceSizeDegrees[i] / 2, sliceSizeDegrees[i], false);
                path.Close();
            }
            else
            {
                path.AddOval(rect);
            }

            if (donutClipPath != null)
            {
                rp.Canvas.ClipPath(donutClipPath, SKClipOperation.Difference);
            }

            Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, radius));
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotation));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }

        if (ShowSliceLabels)
        {
            for (int i = 0; i < Slices.Count(); i++)
            {
                double x = Math.Cos(sliceCenterDegrees[i] * Math.PI / 180) * SliceLabelDistance;
                double y = -Math.Sin(sliceCenterDegrees[i] * Math.PI / 180) * SliceLabelDistance;
                Pixel px = Axes.GetPixel(new Coordinates(x, y));
                Slices[i].LabelStyle.Render(rp.Canvas, px);
            }
        }
    }

    private SKPath GetDonutClipPath(SKRect rect, float donutRadius)
    {

        SKPath donutPath = new SKPath();
        donutPath.AddCircle(rect.MidX, rect.MidY, donutRadius);

        return donutPath;
    }
}
