namespace ScottPlot.Plottables;

public abstract class PieBase : IPlottable, IManagesAxisLimits, IHasLine
{
    public IAxes Axes { get; set; } = new Axes();
    public bool IsVisible { get; set; } = true;

    public Angle Rotation { get; set; } = Angle.FromDegrees(-90);

    public IEnumerable<LegendItem> LegendItems => Slices
        .Select((Func<PieSlice, LegendItem>)(slice => new LegendItem
        {
            LabelText = slice.LegendText,
            FillStyle = slice.Fill,
        }));
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public double Padding { get; set; } = 0.2;
    public double SliceLabelDistance { get; set; } = 1.2;

    public IList<PieSlice> Slices { get; set; } = [];

    /// <summary>
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// circles to always appear as circles instead of ellipses.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    public virtual void UpdateAxisLimits(Plot plot)
    {
        if (plot.Axes.Rules
                .OfType<AxisRules.SquareZoomOut>()
                .Any())
        {
            return;
        }

        AxisRules.SquareZoomOut squareRule = new(Axes.XAxis, Axes.YAxis);
        plot.Axes.Rules.Add(squareRule);
    }

    public abstract AxisLimits GetAxisLimits();
    public abstract void Render(RenderPack rp);
}
