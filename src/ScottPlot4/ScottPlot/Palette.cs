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
    public static ISharedPalette Amber => new Palettes.Amber();
    public static ISharedPalette Aurora => new Palettes.Aurora();
    public static ISharedPalette Category10 => new Palettes.Category10();
    public static ISharedPalette Category20 => new Palettes.Category20();
    public static ISharedPalette ColorblindFriendly => new Palettes.ColorblindFriendly();
    public static ISharedPalette Dark => new Palettes.Dark();
    public static ISharedPalette DarkPastel => new Palettes.DarkPastel();
    public static ISharedPalette Frost => new Palettes.Frost();
    public static ISharedPalette Microcharts => new Palettes.Microcharts();
    public static ISharedPalette Nero => new Palettes.Nero();
    public static ISharedPalette Nord => new Palettes.Nord();
    public static ISharedPalette OneHalf => new Palettes.OneHalf();
    public static ISharedPalette OneHalfDark => new Palettes.OneHalfDark();
    public static ISharedPalette PolarNight => new Palettes.PolarNight();
    public static ISharedPalette Redness => new Palettes.Redness();
    public static ISharedPalette SnowStorm => new Palettes.SnowStorm();
    public static ISharedPalette Tsitsulin => new Palettes.Tsitsulin();


    /// <summary>
    /// Create a new color palette from an array of HTML colors
    /// </summary>
    public static ISharedPalette FromHtmlColors(string[] htmlColors, string name = "", string description = "")
    {
        return new Palettes.Custom(htmlColors, name, description);
    }

    /// <summary>
    /// Create a new color palette from an array of colors
    /// </summary>
    public static ISharedPalette FromColors(Color[] colors, string name = "", string description = "")
    {
        return new Palettes.Custom(colors.Convert(), name, description);
    }

    /// <summary>
    /// Return an array containing every available palette
    /// </summary>
    public static ISharedPalette[] GetPalettes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(ScottPlot.ISharedPalette)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Count() == 0).Any())
            .Select(x => (ISharedPalette)Activator.CreateInstance(x))
            .ToArray();
    }
}
