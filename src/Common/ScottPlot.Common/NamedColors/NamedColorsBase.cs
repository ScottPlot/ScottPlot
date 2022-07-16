namespace ScottPlot.NamedColors;

public abstract class NamedColorsBase : INamedColors
{
    public SPColor[] GetAllColors()
    {
        return GetType()
            .GetMethods()
            .Where(x => x.ReturnType == typeof(SPColor))
            .Select(x => x.Invoke(null, null))
            .Cast<SPColor>()
            .ToArray();
    }
}
