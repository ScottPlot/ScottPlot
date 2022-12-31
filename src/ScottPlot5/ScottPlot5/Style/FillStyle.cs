using ScottPlot.Style.Hatches;

namespace ScottPlot.Style;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class FillStyle
{
    public Color Color { get; set; } = Colors.Black;
    public Color HatchColor { get; set; } = Colors.Gray;
    public IHatch? Hatch { get; set; } = null;
    public bool HasValue => Color != Colors.Transparent || (Hatch is not null && HatchColor != Colors.Transparent);
}
