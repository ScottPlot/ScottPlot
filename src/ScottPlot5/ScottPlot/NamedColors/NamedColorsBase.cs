using System.Linq;

namespace ScottPlot.NamedColors;

public abstract class NamedColorsBase : INamedColors
{
    public Color[] GetAllColors()
    {
        return GetType()
            .GetMethods()
            .Where(x => x.ReturnType == typeof(Color))
            .Select(x => x.Invoke(null, null))
            .Cast<Color>()
            .ToArray();
    }
}
