namespace ScottPlot.Plottables;

public abstract class PieBase : IPlottable, IHasLine
{
    public IAxes Axes { get; set; } = new Axes();
    public bool IsVisible { get; set; } = true;

    public IEnumerable<LegendItem> LegendItems => Slices
        .Select((Func<PieSlice, LegendItem>)(slice => new LegendItem
        {
            LabelText = slice.LegendText,
            FillStyle = slice.Fill,
        }));
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public LineStyle LineStyle { get; set; } = new() { Width = 0 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public double Padding { get; set; } = 0.2;
    public bool ShowSliceLabels { get; set; } = false;
    public double SliceLabelDistance { get; set; } = 1.2;
    public IList<PieSlice> Slices { get; set; } = [];

    protected static SKPoint GetRotatedPoint(double radius, double angleInDegrees)
    {
        double angleInRadians = angleInDegrees * (Math.PI / 180);
        double x = radius * Math.Cos(angleInRadians);
        double y = radius * Math.Sin(angleInRadians);
        return new SKPoint((float)x, (float)y);
    }

    public abstract AxisLimits GetAxisLimits();
    public abstract void Render(RenderPack rp);
}
