namespace ScottPlot.Plottables;

public abstract class AxisLine : LabelStyleProperties, IPlottable, IRenderLast, IHasLine, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public override Label LabelStyle { get; set; } = new();
    public string Text { get => LabelText; set => LabelText = value; }
    public Alignment? ManualLabelAlignment { get; set; } = null;
    public string LegendText { get => LabelText; set => LabelText = value; }

    #region obsolete

    [Obsolete("Use properties in this class (e.g., TextColor) or reach into LabelStyle and assign properties there.", true)]
    public Label Label { get; set; } = new();

    [Obsolete("Use LabelFontSize", true)]
    public float FontSize { get; set; }

    [Obsolete("Use LabelBold", true)]
    public bool FontBold { get; set; }

    [Obsolete("Use LabelFontName", true)]
    public string FontName { get; set; } = string.Empty;

    [Obsolete("Use LabelFontColor", true)]
    public Color FontColor { get; set; }

    [Obsolete("Use LabelFontColor", true)]
    public Color TextColor { get; set; }

    [Obsolete("Use LabelFontColor", true)]
    public Color TextBackgroundColor { get; set; }

    [Obsolete("Use LabelRotation", true)]
    public float TextRotation { get; set; }

    [Obsolete("Use LabelFontSize", true)]
    public float TextSize { get; set; }

    [Obsolete("Use ManualLabelAlignment", true)]
    public Alignment? TextAlignment { get; set; }

    #endregion

    public bool LabelOppositeAxis { get; set; } = false;
    public bool IsDraggable { get; set; } = false;
    public bool ExcludeFromLegend { get; set; } = false;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            LabelStyle.BackgroundColor = value;
        }
    }

    public double Position { get; set; } = 0;

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            return LegendItem.Single(new LegendItem()
            {
                LabelText = ExcludeFromLegend ? string.Empty : LabelStyle.Text,
                LineStyle = LineStyle,
                MarkerStyle = MarkerStyle.None,
            });
        }
    }

    public abstract bool IsUnderMouse(CoordinateRect rect);

    public abstract AxisLimits GetAxisLimits();

    public abstract void Render(RenderPack rp);

    public abstract void RenderLast(RenderPack rp);
}
