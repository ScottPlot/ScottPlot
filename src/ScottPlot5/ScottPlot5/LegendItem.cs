namespace ScottPlot;

public class LegendItem
{
    public string? Label { get; set; }
    public LineStyle Line { get; set; } = new();
    public MarkerStyle Marker { get; set; } = MarkerStyle.Default;
    public FillStyle Fill { get; set; } = new() { Color = Colors.Transparent };
    public IEnumerable<LegendItem> Children { get; set; } = Array.Empty<LegendItem>();
    public bool HasSymbol => Line.Width > 0 || Marker.IsVisible || Fill.HasValue;
    public bool IsVisible => !string.IsNullOrEmpty(Label);
}
