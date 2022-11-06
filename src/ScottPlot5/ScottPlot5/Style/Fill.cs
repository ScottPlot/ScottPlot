using ScottPlot.Style.Hatches;
using SkiaSharp;

namespace ScottPlot.Style;

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
}
