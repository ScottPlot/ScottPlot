using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ScottPlot;

/// <summary>
/// Fields and methods for accessing color palettes
/// </summary>
public static class Palette
{
    public static IPalette Amber => new Palettes.Amber();
    public static IPalette Aurora => new Palettes.Aurora();
    public static IPalette Category10 => new Palettes.Category10();
    public static IPalette Category20 => new Palettes.Category20();
    public static IPalette ColorblindFriendly => new Palettes.ColorblindFriendly();
    public static IPalette Dark => new Palettes.Dark();
    public static IPalette DarkPastel => new Palettes.DarkPastel();
    public static IPalette Frost => new Palettes.Frost();
    public static IPalette Microcharts => new Palettes.Microcharts();
    public static IPalette Nero => new Palettes.Nero();
    public static IPalette Nord => new Palettes.Nord();
    public static IPalette OneHalf => new Palettes.OneHalf();
    public static IPalette OneHalfDark => new Palettes.OneHalfDark();
    public static IPalette PolarNight => new Palettes.PolarNight();
    public static IPalette Redness => new Palettes.Redness();
    public static IPalette SnowStorm => new Palettes.SnowStorm();
    public static IPalette Tsitsulin => new Palettes.Tsitsulin();


    /// <summary>
    /// Create a new color palette from an array of HTML colors
    /// </summary>
    public static IPalette FromHtmlColors(string[] htmlColors, string name = "", string description = "")
    {
        return new Palettes.Custom(htmlColors, name, description);
    }

    /// <summary>
    /// Create a new color palette from an array of colors
    /// </summary>
    public static IPalette FromColors(Color[] colors, string name = "", string description = "")
    {
        return new Palettes.Custom(colors.Convert(), name, description);
    }

    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static IPalette[] GetPalettes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(ScottPlot.IPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Count() == 0).Any())
            .Select(x => (IPalette)Activator.CreateInstance(x))
            .ToArray();
    }
}
