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

            if (DonutSize > 0) {
                rp.Canvas.ClipPath(GetDonutClipPath(rect), SKClipOperation.Difference);
            }

            Slices[i].Fill.ApplyToPaint(paint, new PixelRect(origin, radius));
            paint.Shader = paint.Shader?.WithLocalMatrix(SKMatrix.CreateRotationDegrees(-rotation));
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }
        /*
        if (DonutSize > 0)
        {

            //FillStyle.ApplyToPaint(paint, Axes.GetPixelRect(new CoordinateRect(0, 0, 2, 2)));            
            var circleRadius = .5;
            Debug.WriteLine($"Origin {origin}, Radius {radius}, CircleRadius {circleRadius}");
            
            PixelRect circleRect = new PixelRect(origin, radius / 2);
            SKPath donutPath = new SKPath();
            SKRect innerRect = new(-radius + radius / 2, -radius + radius / 2, radius - radius / 2, radius - radius / 2);
            //donutPath.AddOval(innerRect);
            //paint.StrokeWidth = 10;
            //rp.Canvas.ClipPath(path, SKClipOperation.Intersect);
            //rp.Canvas.ClipPath(donutPath, SKClipOperation.Difference);
            //rp.Canvas.DrawCircle(origin.X, origin.Y, radius/2, paint);

            //path.Reset();
            //donutPath.Reset();

            //rp.Canvas.DrawCircle(origin.X, origin.Y, Convert.ToSingle(circleRadius), paint);            
            Drawing.FillOval(rp.Canvas, paint, new FillStyle() {Color=Colors.White}, circleRect);
            Drawing.DrawOval(rp.Canvas, paint, LineStyle, circleRect);

            //rp.Canvas.DrawOval(rect.MidX, rect.MidY, circleRadius, circleRadius, paint);            
        }
        */

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

    private SKPath GetDonutClipPath(SKRect rect) {

        Pixel origin = Axes.GetPixel(Coordinates.Origin);        
        var circleRadius = .5;
        float minX = Math.Abs(Axes.GetPixelX(1) - origin.X);
        float minY = Math.Abs(Axes.GetPixelY(1) - origin.Y);
        float radius = (Math.Min(minX, minY)/2);        
        SKRect donutRect = new(rect.Left+radius , rect.Top - radius, rect.Right - radius, rect.Bottom - radius);


        Debug.WriteLine($"Origin {origin}, Radius {radius}, CircleRadius {circleRadius}");

        //PixelRect circleRect = new PixelRect(origin, radius / 2);
        //SKRect circleRect =  new SKRect(origin.X, origin.Y, origin.X+radius, origin.Y+radius);
        SKPath donutPath = new SKPath();
        donutPath.AddOval(donutRect);
        //paint.StrokeWidth = 10;
        //rp.Canvas.ClipPath(path, SKClipOperation.Intersect);
        //rp.Canvas.ClipPath(donutPath, SKClipOperation.Difference);
        //rp.Canvas.DrawCircle(origin.X, origin.Y, radius/2, paint);

        //path.Reset();
        //donutPath.Reset();

        //rp.Canvas.DrawCircle(origin.X, origin.Y, Convert.ToSingle(circleRadius), paint);            
        //Drawing.FillOval(rp.Canvas, paint, new FillStyle() { Color = Colors.White }, circleRect);
        //Drawing.DrawOval(rp.Canvas, paint, LineStyle, circleRect);

        //rp.Canvas.DrawOval(rect.MidX, rect.MidY, circleRadius, circleRadius, paint);            
        return donutPath;
    }
}
