using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Config
{
    public static class Fonts
    {
        /// <summary>
        /// Returns a font name guaranteed to be installed on the system
        /// </summary>
        public static string GetValidFontName(string fontName)
        {
            foreach (FontFamily installedFont in FontFamily.Families)
            {
                if (string.Equals(fontName, installedFont.Name, StringComparison.OrdinalIgnoreCase))
                    return installedFont.Name;
            }

            string defaultFontName = GetDefaultFontName();
            Debug.WriteLine($"Warning: font {fontName} was not found, defaulting to {defaultFontName}");
            return defaultFontName;
        }

        /// <summary>
        /// Returns the default ScottPlot font name (guaranteed to be installed on the system)
        /// </summary>
        public static string GetDefaultFontName()
        {
            string[] preferredFonts = { "Segoe UI", "DejaVu Sans" };
            string[] installedFonts = FontFamily.Families.Select(font => font.Name.ToUpper()).ToArray();

            foreach (string preferredFont in preferredFonts)
                if (installedFonts.Contains(preferredFont.ToUpper()))
                    return preferredFont;

            return SystemFonts.DefaultFont.Name;
        }
    }
}
