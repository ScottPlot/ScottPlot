namespace ScottPlot.Plottables;

public abstract class AxisLine : LabelStyleProperties, IPlottable, IRenderLast, IHasLine, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public override Label LabelStyle { get; set; } = new();
    public string Text { get => LabelText; set => LabelText = value; }
    public string LegendText { get; set; } = string.Empty;

    public Alignment? ManualLabelAlignment { get; set; } = null;

    #region obsolete

    [Obsolete("Use ManualLabelAlignment")]
    public Alignment? TextAlignment { get => ManualLabelAlignment; set => ManualLabelAlignment = value; }

    [Obsolete("Set LabelFontSize, LabelBold, LabelFontColor, or properties of the LabelStyle object.")]
    public Label Label { get => LabelStyle; set => LabelStyle = value; }

    [Obsolete("Use LabelFontSize")]
    public float FontSize { get => LabelFontSize; set => LabelFontSize = value; }

    [Obsolete("Use LabelBold")]
    public bool FontBold { get => LabelBold; set => LabelBold = value; }

    [Obsolete("Use LabelFontName")]
    public string FontName { get => LabelFontName; set => LabelFontName = value; }

    [Obsolete("Use LabelFontColor")]
    public Color FontColor { get => LabelFontColor; set => LabelFontColor = value; }

    [Obsolete("Use LabelFontColor")]
    public Color TextColor { get => LabelFontColor; set => LabelFontColor = value; }

    [Obsolete("Use LabelFontColor")]
    public Color TextBackgroundColor { get => LabelFontColor; set => LabelFontColor = value; }

    [Obsolete("Use LabelRotation")]
    public float TextRotation { get => LabelRotation; set => LabelRotation = value; }

    [Obsolete("Use LabelFontSize")]
    public float TextSize { get => LabelFontSize; set => LabelFontSize = value; }

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
                LabelText = ExcludeFromLegend ? string.Empty : LegendText,
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
