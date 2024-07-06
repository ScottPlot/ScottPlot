namespace ScottPlot.PolarAxes;

public class RadialAxis : IRadialAxis
{
    public Spoke[] Spokes { get; }

    public RadialAxis(double maxRadius) :
        this(maxRadius, [0, 90, 180, 270])
    {
    }

    public RadialAxis(double maxRadius, IEnumerable<double> angles)
    {
        Spokes = [];
        if (angles is not null &&
            angles.Any())
        {
            var spokes = new List<Spoke>();
            foreach (double angle in angles)
            {
                spokes.Add(new(angle, maxRadius));
            }
            Spokes = [.. spokes];
        }
    }

    protected virtual void RenderSpoke(RenderPack rp, IAxes axes, Spoke spoke)
    {
        var paint = new SKPaint();
        spoke.LineStyle.ApplyToPaint(paint);

        var polar = new PolarCoordinates(spoke.Length, spoke.Angle);
        Pixel pixel = axes.GetPixel(polar) - axes.GetPixel(Coordinates.Origin);

        rp.Canvas.DrawLine(0, 0, pixel.X, pixel.Y, paint);

        var labelStyle = new LabelStyle
        {
            Text = $"{spoke.Angle}"
        };

        var labPolar = new PolarCoordinates(polar.Radial * 1.1, polar.Angular);
        Pixel labelPixel = axes.GetPixel(labPolar) - axes.GetPixel(Coordinates.Origin);
        var labelRect = labelStyle.Measure().Rect(Alignment.MiddleCenter);
        var labelOffset = labelRect.Center - labelRect.TopLeft;
        labelPixel -= labelOffset;

        labelStyle.Render(rp.Canvas, labelPixel, paint);
    }

    public void Render(RenderPack rp, IAxes axes)
    {
        if (Spokes is null ||
            Spokes.Length < 1)
        {
            return;
        }

        using SKAutoCanvasRestore _ = new(rp.Canvas);

        Pixel origin = axes.GetPixel(Coordinates.Origin);
        rp.Canvas.Translate(origin.X, origin.Y);

        foreach (Spoke spoke in Spokes)
        {
            RenderSpoke(rp, axes, spoke);
        }
    }
}
