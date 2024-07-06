namespace ScottPlot.PolarAxes;

public class CircularAxis : ICircularAxis
{
    public double[] Radii { get; }

    public LineStyle AxisStyle { get; set; } = new LineStyle()
    {
        Width = 1,
    };

    public CircularAxis(IEnumerable<double> radii)
    {
        Radii = [];
        if (radii is not null &&
            radii.Any())
        {
            Radii = [.. radii];
        }
    }

    public void Render(RenderPack rp, IAxes axes)
    {
        if (Radii.Length < 1)
        {
            return;
        }

        var paint = new SKPaint();
        AxisStyle.ApplyToPaint(paint);

        using SKAutoCanvasRestore _ = new(rp.Canvas);

        Pixel origin = axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (double radius in Radii)
        {
            float pixelX = axes.GetPixelX(radius) - origin.X;
            float pixelY = axes.GetPixelY(radius) - origin.Y;
            Drawing.DrawOval(
                rp.Canvas,
                paint,
                AxisStyle,
                new PixelRect(-pixelX, pixelX, pixelY, -pixelY));
        }
    }
}
