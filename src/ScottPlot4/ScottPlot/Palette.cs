using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ScottPlot;

/// <summary>
/// Fields and methods for accessing color palettes
/// </summary>
public static class Palette
{
    public enum StandardPalettes
    {
        Amber,
        Aurora,
        Category10,
        Category20,
        ColorblindFriendly,
        Dark,
        DarkPastel,
        Frost,
        Microcharts,
        Nero,
        Nord,
        OneHalf,
        OneHalfDark,
        PolarNight,
        Redness,
        SnowStorm,
        Tsitsulin
    };
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

    public static readonly Dictionary<StandardPalettes, IPalette> PaletteDictionary = new()
    {
        { StandardPalettes.Amber, ScottPlot.Palette.Amber },
        { StandardPalettes.Aurora, ScottPlot.Palette.Aurora },
        { StandardPalettes.Category10, ScottPlot.Palette.Category10 },
        { StandardPalettes.Category20, ScottPlot.Palette.Category20 },
        { StandardPalettes.ColorblindFriendly, ScottPlot.Palette.ColorblindFriendly },
        { StandardPalettes.Dark, ScottPlot.Palette.Dark },
        { StandardPalettes.DarkPastel, ScottPlot.Palette.DarkPastel},
        { StandardPalettes.Frost, ScottPlot.Palette.Frost },
        { StandardPalettes.Microcharts, ScottPlot.Palette.Microcharts },
        { StandardPalettes.Nero, ScottPlot.Palette.Nero },
        { StandardPalettes.Nord, ScottPlot.Palette.Nord },
        { StandardPalettes.OneHalf, ScottPlot.Palette.OneHalf },
        { StandardPalettes.OneHalfDark, ScottPlot.Palette.OneHalfDark },
        { StandardPalettes.PolarNight, ScottPlot.Palette.PolarNight },
        { StandardPalettes.Redness, ScottPlot.Palette.Redness },
        { StandardPalettes.SnowStorm, ScottPlot.Palette.SnowStorm },
        { StandardPalettes.Tsitsulin, ScottPlot.Palette.Tsitsulin }
    };

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
