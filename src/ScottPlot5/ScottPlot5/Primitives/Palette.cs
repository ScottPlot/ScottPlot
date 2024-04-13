namespace ScottPlot;

public static class Palette
{
    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static IPalette FromColors(string[] hexColors)
    {
        return new Palettes.Custom(hexColors, string.Empty, string.Empty);
    }

    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static IPalette FromColors(Color[] colors)
    {
        return new Palettes.Custom(colors, string.Empty, string.Empty);
    }

    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static IPalette[] GetPalettes()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(IPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Count() == 0).Any())
            .Select(x => (IPalette)Activator.CreateInstance(x)!)
            .ToArray();
    }
}
