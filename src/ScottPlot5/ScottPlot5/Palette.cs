namespace ScottPlot;

public static class Palette
{
    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static ISharedPalette FromColors(string[] hexColors)
    {
        return new Palettes.Custom(hexColors, string.Empty, string.Empty);
    }

    /// <summary>
    /// Create a custom palette from an array of colors
    /// </summary>
    public static ISharedPalette FromColors(Color[] colors)
    {
        return new Palettes.Custom(colors.Convert(), string.Empty, string.Empty);
    }

    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static ISharedPalette[] GetAllPalettes()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(ISharedPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Count() == 0).Any())
            .Select(x => (ISharedPalette)Activator.CreateInstance(x)!)
            .ToArray();
    }
}
