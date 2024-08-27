namespace ScottPlot;

public static class Colormap
{
    /// <summary>
    /// Return an array containing every available colormap
    /// </summary>
    public static IColormap[] GetColormaps()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(IColormap)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Length == 0).Any())
            .Select(x => (IColormap)Activator.CreateInstance(x)!)
            .ToArray();
    }
}
