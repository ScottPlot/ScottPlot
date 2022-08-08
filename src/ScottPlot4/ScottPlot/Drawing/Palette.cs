using System;

namespace ScottPlot.Drawing;

[Obsolete("Use ScottPlot.Palette instead of ScottPlot.Drawing.Palette", true)]
public class Palette
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
    public static IPalette SnowStorm => new Palettes.Snowstorm();
    public static IPalette Tsitsulin => new Palettes.Tsitsulin();
}
