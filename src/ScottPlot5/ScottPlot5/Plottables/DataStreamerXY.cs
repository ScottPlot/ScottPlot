
using ScottPlot.Collections;
using System.IO;

namespace ScottPlot.Plottables;

public class DataStreamerXY(int capacity) : IPlottable, IManagesAxisLimits, IHasLine, IHasLegendText, IHasMarker
{
    CircularBuffer<Coordinates> Buffer { get; } = new(capacity);

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, LineStyle);
    public string LegendText { get; set; } = string.Empty;

    public bool ManageAxisLimits { get; set; } = true;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 0);
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineColor = value;
            MarkerFillColor = value;
        }
    }

    public AxisLimits GetAxisLimits() => new(Buffer);

    public void UpdateAxisLimits(Plot plot)
    {
        if (!ManageAxisLimits)
            return;

        AxisLimits dataLimits = GetAxisLimits();
        Axes.XAxis.Range.Set(dataLimits.XRange);
        Axes.YAxis.Range.Set(dataLimits.YRange);
    }

    public void Add(double x, double y)
    {
        Buffer.Add(new(x, y));
    }

    public void Add(DateTime x, double y)
    {
        Buffer.Add(new(x.ToOADate(), y));
    }

    public void Add(Coordinates point)
    {
        Buffer.Add(point);
    }

    public virtual void Render(RenderPack rp)
    {
        // TODO: move this logic inside the buffer and make it more effecient
        var pixels = Buffer._buffer.Select(Axes.GetPixel).OrderBy(pt => pt.X);

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
    }
}
