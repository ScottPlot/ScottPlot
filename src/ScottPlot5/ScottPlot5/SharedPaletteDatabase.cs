using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot;

// NOTE: projects should implement their own version of this
internal static class SharedPalette
{
    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static IPalette[] GetPalettes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(IPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Length == 0).Any())
            .Select(x => (IPalette)Activator.CreateInstance(x)!)
            .ToArray();
    }
}
