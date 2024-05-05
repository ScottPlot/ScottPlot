

namespace ScottPlot.Plottables;

public class Radar(IReadOnlyList<RadarSeries> series) : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => Series.Select(s => new LegendItem
    {
        LabelText = s.LegendText,
        FillStyle = s.Fill
    });

    public LineStyle LineStyle { get; set; } = new() { Width = 0 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public IStarAxis StarAxis { get; set; } = new StarAxes.PolygonalStarAxis();
    public IReadOnlyList<RadarSeries> Series { get; set; } = series;
    public double Padding { get; set; } = 0.2;
    public double LabelDistance { get; set; } = 1.2;

    public AxisLimits GetAxisLimits()
    {
        double radius = 1 + Padding;

        return new AxisLimits(-radius, radius, -radius, radius);
    }

    public void Render(RenderPack rp)
    {
        if (!Series.Any())
            return;

        const float startAngle = -90;
        int seriesArity = Series.First().Values.Count();
        StarAxis.Render(rp, Axes, 1, seriesArity, startAngle);

        double maxValue = Series.SelectMany(s => s.Values).Max();
        if (maxValue == 0)
            return;

        Pixel origin = Axes.GetPixel(Coordinates.Origin);

        using SKPaint paint = new();
        using SKPath path = new();
        using SKAutoCanvasRestore _ = new(rp.Canvas);
        rp.Canvas.Translate(origin.X, origin.Y);

        var rotationPerSlice = Math.PI * 2 / seriesArity;

        foreach (var serie in Series)
        {
            for (int i = 0; i < seriesArity; i++)
            {
                double coordinateRadius = serie.Values[i] / maxValue;
                float minX = Math.Abs(Axes.GetPixelX(coordinateRadius) - origin.X);
                float minY = Math.Abs(Axes.GetPixelY(coordinateRadius) - origin.Y);
                var radius = Math.Min(minX, minY);

                var theta = rotationPerSlice * i + rotationPerSlice / 2 + startAngle;
                float x = (float)(radius * Math.Cos(theta));
                float y = (float)(radius * Math.Sin(theta));

                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);
            }

            path.Close();

            serie.Fill.ApplyToPaint(paint, rp.FigureRect);
            rp.Canvas.DrawPath(path, paint);

            LineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            path.Reset();
        }
    }
}
