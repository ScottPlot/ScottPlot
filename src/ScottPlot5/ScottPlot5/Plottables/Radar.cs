using System.Reflection.Emit;

namespace ScottPlot.Plottables;

public class Radar() : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => Series.Select(s => new LegendItem
    {
        LabelText = s.LegendText,
        FillStyle = s.FillStyle,
    });

    public PolarAxis PolarAxis { get; set; } = new() { RotationDegrees = -90 };

    public List<RadarSeries> Series { get; } = [];
    public double Padding { get; set; } = 0.2;
    public double LabelDistance { get; set; } = 1.2;

    public double ValuePaddingFraction = 0.2;

    public AxisLimits GetAxisLimits() => PolarAxis.GetAxisLimits();

    public void SetLabels(string[] labels)
    {
        PolarAxis.RegenerateSpokes(labels);
    }

    public void SetTicks(double[] positions)
    {
        PolarAxis.Circles.Clear();
        foreach (double radius in positions)
        {
            PolarAxisCircle circle = new(radius);
            PolarAxis.Circles.Add(circle);
        }
    }

    /// <summary>
    /// Scale the polar axis to fit the data in each series
    /// </summary>
    public void AutoScale()
    {
        if (Series.Count == 0)
            return;

        double maxValue = Series.Select(x => x.Values.Max()).Max();

        PolarAxis.MaximumRadius = maxValue * (1 + ValuePaddingFraction);

        Console.WriteLine($"max radius: {PolarAxis.MaximumRadius}");
    }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        if (Series.Count == 0)
            return;

        int valueCount = Series.First().Values.Count;
        if (PolarAxis.Spokes.Count != valueCount)
        {
            PolarAxis.RegenerateSpokes(valueCount);
        }

        using SKPaint paint = new();

        PolarAxis.Axes = Axes;
        PolarAxis.Render(rp);

        for (int i = 0; i < Series.Count; i++)
        {
            Coordinates[] cs1 = PolarAxis.GetCoordinates(Series[i].Values);
            Pixel[] pixels = cs1.Select(Axes.GetPixel).ToArray();
            Drawing.DrawPath(rp.Canvas, paint, pixels, Series[i].FillStyle);
            Drawing.DrawPath(rp.Canvas, paint, pixels, Series[i].LineStyle);
        }
    }
}
