namespace ScottPlot;

public class LegendItem : LabelStyleProperties, IHasMarker, IHasLine, IHasFill, IHasArrow, IHasLabel
{
    public override Label LabelStyle { get; } = new() { Alignment = Alignment.MiddleLeft }; // TODO: remove setter

    public LineStyle LineStyle { get; set; } = new() { Width = 0 };// TODO: remove setter
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();// TODO: remove setter
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public LineStyle OutlineStyle { get; set; } = new() { Width = 0 };// TODO: remove setter
    public float OutlineWidth { get => OutlineStyle.Width; set => OutlineStyle.Width = value; }
    public LinePattern OutlinePattern { get => OutlineStyle.Pattern; set => OutlineStyle.Pattern = value; }
    public Color OutlineColor { get => OutlineStyle.Color; set => OutlineStyle.Color = value; }


    public MarkerStyle MarkerStyle { get; set; } = new(); // TODO: remove setter
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.Fill.Color; set => MarkerStyle.Fill.Color = value; }
    public Color MarkerLineColor { get => MarkerStyle.Outline.Color; set => MarkerStyle.Outline.Color = value; }
    public float MarkerLineWidth { get => MarkerStyle.Outline.Width; set => MarkerStyle.Outline.Width = value; }

    public ArrowStyle ArrowStyle { get; } = new();
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public Color ArrowColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }

    public PixelSize MeasureLabel()
    {
        return LabelStyle.MeasureLine();
    }

    #region obsolete these
    public FontStyle? CustomFontStyle { get; set; } = null;
    public LineStyle Line { get => LineStyle; set => LineStyle = value; }
    public MarkerStyle Marker { get => MarkerStyle; set => MarkerStyle = value; }
    public FillStyle Fill { get => FillStyle; set => FillStyle = value; }
    public string Label { get => LabelText; set => LabelText = value; }
    public bool HasArrow { get => ArrowStyle.LineStyle.IsVisible; set => ArrowStyle.LineStyle.IsVisible = value; }
    public bool IsVisible => !string.IsNullOrEmpty(LabelText);
    public bool HasSymbol => HasArrow || Marker.IsVisible;
    #endregion

    #region Static Builders

    public static IEnumerable<LegendItem> None => [];

    public static IEnumerable<LegendItem> Single(LegendItem item) => [item];

    public static IEnumerable<LegendItem> Single(string label, MarkerStyle markerStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = markerStyle,
            Line = LineStyle.None,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, MarkerStyle markerStyle, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = markerStyle,
            Line = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = MarkerStyle.None,
            Line = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, FillStyle fillStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = MarkerStyle.None,
            Fill = fillStyle,
            Line = LineStyle.None,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, FillStyle fillStyle, LineStyle lineStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = MarkerStyle.None,
            Fill = fillStyle,
            Line = lineStyle,
        };

        return Single(item);
    }

    public static IEnumerable<LegendItem> Single(string label, LineStyle lineStyle, MarkerStyle markerStyle)
    {
        LegendItem item = new()
        {
            Label = label,
            Marker = markerStyle,
            Line = lineStyle,
        };

        return Single(item);
    }

    #endregion
}
