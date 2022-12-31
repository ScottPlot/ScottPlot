using ScottPlot.Style;

namespace ScottPlot;

public class LegendItem
{
    public string? Label { get; set; }
    public Stroke? Line { get; set; }
    public Marker? Marker { get; set; }
    public Fill? Fill { get; set; }
    public IEnumerable<LegendItem> Children { get; set; } = Array.Empty<LegendItem>();
    public bool HasSymbol => Line.HasValue || Marker.HasValue || Fill.HasValue;
    public bool IsVisible => !string.IsNullOrEmpty(Label);
}
