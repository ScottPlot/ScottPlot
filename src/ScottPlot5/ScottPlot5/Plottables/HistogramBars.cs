
using ScottPlot.Statistics;

namespace ScottPlot.Plottables;
public class HistogramBars : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    private readonly Histogram Histogram;
    public Bar[] Bars { get; set; }
    public LabelStyle LabelStyle { get; set; } = new() { IsVisible = false };

    public double BarWidthFraction
    {
        set
        {
            for (int i = 0; i < Histogram.Bins.Length; i++)
            {
                double left = Histogram.Edges[i];
                double right = Histogram.Edges[i + 1];
                Bars[i].Size = (right - left) * value;
            }
        }
    }

    public HistogramBars(Histogram histogram)
    {
        Histogram = histogram;
        Bars = new Bar[histogram.Bins.Length];
        for (int i = 0; i < histogram.Bins.Length; i++)
        {
            double left = histogram.Edges[i];
            double right = histogram.Edges[i + 1];
            double center = (left + right) / 2;
            Bars[i] = new() { Position = center };
        }
        BarWidthFraction = 1.0;
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(
            left: Histogram.Bins.First(),
            right: Histogram.Bins.Last(),
            bottom: 0,
            top: Histogram.Counts.Max());
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        for (int i = 0; i < Histogram.Bins.Length; i++)
        {
            double binLeftEdge = Histogram.Edges[i];
            double binRightEdge = Histogram.Edges[i + 1];
            Bars[i].Value = Histogram.Counts[i];
            Bars[i].Render(rp, Axes, paint, LabelStyle);
        }
    }
}
