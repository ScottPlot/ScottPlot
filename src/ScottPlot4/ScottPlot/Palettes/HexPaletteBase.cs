using System.Linq;

namespace ScottPlot.Palettes;

// ScottPlot4
public abstract class HexPaletteBase : PaletteBase
{
    internal abstract string[] HexColors { get; }

    public HexPaletteBase()
    {
        Colors = HexColors.Select(x => System.Drawing.ColorTranslator.FromHtml(x)).ToArray();
    }
}
