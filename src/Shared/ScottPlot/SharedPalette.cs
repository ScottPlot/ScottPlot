using System.Reflection;

namespace ScottPlot;

internal static class SharedPalette
{
    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static ISharedPalette[] GetPalettes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(ISharedPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Length == 0).Any())
            .Select(x => (ISharedPalette)Activator.CreateInstance(x)!)
            .ToArray();
    }
}
