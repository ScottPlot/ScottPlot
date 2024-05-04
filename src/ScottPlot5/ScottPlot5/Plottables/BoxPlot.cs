namespace ScottPlot.Plottables;

/// <summary>
/// Displays 1 or more boxes all styled the same
/// </summary>
public class BoxPlot : IPlottable, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, Boxes.First().FillStyle);

    public readonly List<Box> Boxes = [];

    // helper methods to quickly style all boxes with common traits
    public Color FillColor { set => Boxes.ForEach(x => x.FillColor = value); }
    public Color LineColor { set => Boxes.ForEach(x => x.LineColor = value); }
    public float LineWidth { set => Boxes.ForEach(x => x.LineWidth = value); }

    #region obsolete

    [Obsolete("use LineColor", true)]
    public Color StrokeColor { set => Boxes.ForEach(x => x.LineColor = value); }

    [Obsolete("use LineWidth", true)]
    public float StrokeWidth { set => Boxes.ForEach(x => x.LineWidth = value); }

    #endregion

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (Box box in Boxes)
        {
            limits.Expand(box.GetAxisLimits());
        }

        return limits.AxisLimits;
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        foreach (Box box in Boxes)
        {
            box.Render(rp, paint, Axes);
        }
    }
}
