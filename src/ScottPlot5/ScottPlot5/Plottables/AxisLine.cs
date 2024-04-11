namespace ScottPlot.Plottables;

public abstract class AxisLine : IPlottable, IRenderLast
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    [Obsolete("Use properties in this class (e.g., TextColor) or reach into LabelStyle and assign properties there.", true)]
    public Label Label { get; set; } = new();
    public Label LabelStyle { get; set; } = new();
    public float FontSize { get => LabelStyle.FontSize; set => LabelStyle.FontSize = value; }
    public bool FontBold { get => LabelStyle.Bold; set => LabelStyle.Bold = value; }
    public string FontName { get => LabelStyle.FontName; set => LabelStyle.FontName = value; }

    [Obsolete("Use TextColor", true)]
    public Color FontColor => TextColor;
    public Color TextColor { get => LabelStyle.ForeColor; set => LabelStyle.ForeColor = value; }
    public Color TextBackgroundColor { get => LabelStyle.BackColor; set => LabelStyle.BackColor = value; }
    public string Text { get => LabelStyle.Text; set => LabelStyle.Text = value; }
    public float TextRotation { get => LabelStyle.Rotation; set => LabelStyle.Rotation = value; }
    public float TextSize { get => LabelStyle.FontSize; set => LabelStyle.FontSize = value; }
    public Alignment? TextAlignment { get; set; } = null;

    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public bool LabelOppositeAxis { get; set; } = false;

    public bool IsDraggable { get; set; } = false;

    public bool ExcludeFromLegend { get; set; } = false;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            LabelStyle.BackColor = value;
        }
    }

    public double Position { get; set; } = 0;

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            return LegendItem.Single(new LegendItem()
            {
                Label = ExcludeFromLegend ? string.Empty : LabelStyle.Text,
                Line = LineStyle,
                Marker = MarkerStyle.None,
            });
        }
    }

    public abstract bool IsUnderMouse(CoordinateRect rect);

    public abstract AxisLimits GetAxisLimits();

    public abstract void Render(RenderPack rp);

    public abstract void RenderLast(RenderPack rp);
}
