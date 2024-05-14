namespace ScottPlot;

public class LegendItem : LabelStyleProperties, IHasMarker, IHasLine, IHasFill, IHasArrow, IHasLabel
{
    public override Label LabelStyle { get; set; } = new() { Alignment = Alignment.MiddleLeft };

    public LineStyle LineStyle { get; set; } = new() { Width = 0 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public LineStyle OutlineStyle { get; set; } = new() { Width = 0 };
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }


    public MarkerStyle MarkerStyle { get; set; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerFillColor; set { MarkerFillColor = value; MarkerLineColor = value; } }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public ArrowStyle ArrowStyle { get; set; } = new();
    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }
    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }
    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }
    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public float ArrowWidth { get => ArrowStyle.ArrowWidth; set => ArrowStyle.ArrowWidth = value; }
    public float ArrowheadAxisLength { get => ArrowStyle.ArrowheadAxisLength; set => ArrowStyle.ArrowheadAxisLength = value; }
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }
    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    [Obsolete("use LineStyle")]
    public LineStyle Line { get => LineStyle; set => LineStyle = value; }

    [Obsolete("use MarkerStyle")]
    public MarkerStyle Marker { get => MarkerStyle; set => MarkerStyle = value; }

    [Obsolete("use LabelText")]
    public string Label { get => LabelText; set => LabelText = value; }

    #region Static Builders

    public static IEnumerable<LegendItem> None => [];

    public static IEnumerable<LegendItem> Single(LegendItem item) => [item];

    public static IEnumerable<LegendItem> Single(string label, MarkerStyle markerStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = markerStyle,
            LineStyle = LineStyle.None,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, MarkerStyle markerStyle, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = markerStyle,
            LineStyle = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = MarkerStyle.None,
            LineStyle = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, FillStyle fillStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = MarkerStyle.None,
            FillStyle = fillStyle,
            LineStyle = LineStyle.None,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, FillStyle fillStyle, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = MarkerStyle.None,
            FillStyle = fillStyle,
            LineStyle = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, LineStyle lineStyle, MarkerStyle markerStyle)
    {
        LegendItem item = new()
        {
            LabelText = label,
            MarkerStyle = markerStyle,
            LineStyle = lineStyle,
        };

        return Single(item);
    }

    #endregion
}
