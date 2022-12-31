using ScottPlot.Style.Hatches;

namespace ScottPlot.Style;

[Obsolete()]
public struct Fill
{
    public Color Color { get; set; } = Colors.CornflowerBlue;
    public Color HatchColor { get; set; } = Colors.Gray;
    public IHatch? Hatch { get; set; } = null;

    public Fill()
    {
    }

    public Fill(Color color)
    {
        Color = color;
    }

    public SKShader? GetShader() => Hatch?.GetShader(Color, HatchColor);

    public Fill WithColor(Color color) => new Fill() { Color = color, Hatch = Hatch, HatchColor = HatchColor };
}
