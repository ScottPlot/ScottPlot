namespace ScottPlot;

public class LegendItem
{
    public string? Label { get; set; }
    public LineStyle Line { get; set; } = new();
    public Color LineColor { get => Line.Color; set => Line.Color = value; }
    public float LineWidth { get => Line.Width; set => Line.Width = value; }
    public MarkerStyle Marker { get; set; } = MarkerStyle.Default;
    public Color MarkerColor
    {
        get => Marker.Fill.Color;
        set { Marker.Fill.Color = value; Marker.Outline.Color = value; }
    }
    public FillStyle Fill { get; set; } = new() { Color = Colors.Transparent };
    public Color FillColor { get => Fill.Color; set => Fill.Color = value; }
    public IEnumerable<LegendItem> Children { get; set; } = Array.Empty<LegendItem>();
    public bool HasSymbol => Line.Width > 0 || Marker.IsVisible || Fill.HasValue;
    public bool IsVisible => !string.IsNullOrEmpty(Label);
    internal FontStyle? CustomFontStyle { get; set; } = null;

    public static IEnumerable<LegendItem> None => Array.Empty<LegendItem>();

    public static IEnumerable<LegendItem> Single(LegendItem item)
    {
        return new LegendItem[] { item };
    }

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
}
