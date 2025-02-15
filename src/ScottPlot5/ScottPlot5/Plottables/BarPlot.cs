namespace ScottPlot.Plottables;

/// <summary>
/// Holds a collection of individually styled bars
/// </summary>
public class BarPlot : IPlottable, IHasLegendText
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public List<Bar> Bars { get; } // TODO: bar plot data source?

    public LabelStyle ValueLabelStyle { get; set; } = new()
    {
        Alignment = Alignment.LowerCenter,
    };

    /// <summary>
    /// Apply a fill color to all bars
    /// </summary>
    public Color Color
    {
        set
        {
            foreach (Bar bar in Bars)
            {
                bar.FillColor = value;
            }
        }
    }

    /// <summary>
    /// Define orientation for all bars
    /// </summary>
    public bool Horizontal
    {
        set
        {
            foreach (Bar bar in Bars)
            {
                bar.Orientation = value
                    ? Orientation.Horizontal
                    : Orientation.Vertical;
            }
        }
    }

    public BarPlot(List<Bar> bars)
    {
        Bars = bars;
    }

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            if (Bars.Count == 0)
            {
                return LegendItem.None;
            }

            LegendItem item = new()
            {
                Plottable = this,
                LabelText = LegendText,
                FillColor = Bars.First().FillColor,
            };

            return LegendItem.Single(item);
        }
    }

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (Bar bar in Bars)
        {
            limits.Expand(bar.AxisLimits);
        }

        return limits.AxisLimits;
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        foreach (Bar bar in Bars)
        {
            ValueLabelStyle.Text = bar.Label;
            bar.Render(rp, Axes, paint, ValueLabelStyle);
        }
    }
}
