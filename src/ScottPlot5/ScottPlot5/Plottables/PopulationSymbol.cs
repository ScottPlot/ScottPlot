
namespace ScottPlot.Plottables;

public class PopulationSymbol(Population population) : IPlottable
{
    public Population Population { get; } = population;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public double X { get; set; } = 0;

    public double Width { get; set; } = 0.8;

    public double MarkerWidthFraction = 0.8;
    public HorizontalAlignment MarkerAlignment { get; set; } = HorizontalAlignment.Left;

    public double BarWidthFraction = .9;
    public HorizontalAlignment BarAlignment { get; set; } = HorizontalAlignment.Right;
    public HorizontalAlignment BoxAlignment { get => BarAlignment; set => BarAlignment = value; }

    public AxisLimits GetAxisLimits() => new(GetRect());

    public Bar Bar { get; set; } = new() { IsVisible = true };
    public Box Box { get; set; } = new() { IsVisible = false };
    public Marker Marker { get; set; } = new() { Size = 10, Shape = MarkerShape.OpenCircle };

    private Label _EmptyLabel = new() { IsVisible = false };

    public Func<Box, Population, Population> BoxValueConfig { get; set; } = BoxValueConfigurator_MedianQuantileExtrema;

    private CoordinateRect GetRect()
    {
        double left = X - Width / 2;
        double right = X + Width / 2;
        double min = Population.Min;
        double max = Population.Max;

        if (Bar.IsVisible)
        {
            min = Math.Min(Bar.ValueBase, min);
            max = Math.Max(Bar.ValueBase, max);
        }

        return new CoordinateRect(left, right, min, max);
    }

    private CoordinateRect GetMarkerRect()
    {
        CoordinateRect rect = GetRect();

        double pad = (1 - MarkerWidthFraction) / 2;

        (double left, double right) = MarkerAlignment switch
        {
            HorizontalAlignment.Left => (rect.Left + pad, rect.HorizontalCenter - pad),
            HorizontalAlignment.Center => (rect.Left + pad, rect.Right - pad),
            HorizontalAlignment.Right => (rect.HorizontalCenter + pad, rect.Right - pad),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    private CoordinateRect GetBarRect()
    {
        CoordinateRect rect = GetRect();

        double pad = (1 - BarWidthFraction) / 2;

        (double left, double right) = BarAlignment switch
        {
            HorizontalAlignment.Left => (rect.Left + pad, rect.HorizontalCenter - pad),
            HorizontalAlignment.Center => (rect.Left + pad, rect.Right - pad),
            HorizontalAlignment.Right => (rect.HorizontalCenter + pad, rect.Right - pad),
            _ => throw new NotImplementedException(),
        };

        return new CoordinateRect(left, right, rect.Bottom, rect.Top);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderBar(rp, paint);
        RenderBox(rp, paint);
        RenderMarkers(rp, paint);
    }

    private void RenderMarkers(RenderPack rp, SKPaint paint)
    {
        if (!Marker.IsVisible)
            return;

        CoordinateRect rect = GetMarkerRect();
        double[] xs = Generate.RandomSample(Population.Count, rect.Left, rect.Right);
        for (int i = 0; i < Population.Count; i++)
        {
            Coordinates location = new(xs[i], Population.Values[i]);
            Pixel px = Axes.GetPixel(location);
            Drawing.DrawMarker(rp.Canvas, paint, px, Marker.MarkerStyle);
        }
    }

    private void RenderBar(RenderPack rp, SKPaint paint)
    {
        if (!Bar.IsVisible)
            return;

        CoordinateRect rect = GetBarRect();
        Bar.Position = rect.HorizontalCenter;
        Bar.Size = rect.Width;

        Bar.Value = Population.Mean;
        Bar.Error = Population.StandardError;

        Bar.Render(rp, Axes, paint, _EmptyLabel);
    }

    private void RenderBox(RenderPack rp, SKPaint paint)
    {
        if (!Box.IsVisible)
            return;

        CoordinateRect rect = GetBarRect();
        Box.Position = rect.HorizontalCenter;
        Box.Width = rect.Width;

        BoxValueConfig.Invoke(Box, Population);
        Box.Render(rp, paint, Axes);
    }

    public static Population BoxValueConfigurator_MeanStdErrStDev(Box box, Population pop)
    {
        box.BoxMiddle = pop.Mean;
        box.BoxMin = pop.Mean - pop.StandardError;
        box.BoxMax = pop.Mean + pop.StandardError;
        box.WhiskerMin = pop.Mean - pop.StandardDeviation;
        box.WhiskerMax = pop.Mean + pop.StandardDeviation;
        return pop;
    }

    public static Population BoxValueConfigurator_MedianQuantileExtrema(Box box, Population pop)
    {
        box.BoxMiddle = pop.Median;
        box.BoxMin = pop.GetPercentile(25);
        box.BoxMax = pop.GetPercentile(75);
        box.WhiskerMin = pop.Min;
        box.WhiskerMax = pop.Max;
        return pop;
    }
}
