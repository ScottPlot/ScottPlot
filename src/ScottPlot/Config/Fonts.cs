using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

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
                if (fontName.ToUpper() == installedFont.Name.ToUpper())
                    return installedFont.Name;

            string defaultFontName = GetDefaultFontName();
            Debug.WriteLine($"Warning: font {fontName} was not found, defaulting to {defaultFontName}");
            return defaultFontName;
        }

        /// <summary>
        /// Returns the default ScottPlot font name (guaranteed to be installed on the system)
        /// </summary>
        public static string GetDefaultFontName()
        {
            return GetDefaultFontName(FontFamily.Families.Select(font => font.Name));
        }

        /// <summary>
        /// Returns the default ScottPlot font name (guaranteed to be installed on the system)
        /// This method for ability to inject test set of fonts, main api method is GetDefaultFontName()
        /// </summary>
        /// <param name="installedFonts">strings containing installed fonts</param>
        public static string GetDefaultFontName(IEnumerable<string> installedFonts)
        {
            string[] preferredFonts = { "Segoe UI", "DejaVu", "Sans" };
            preferredFonts = preferredFonts.Select(f => f.ToUpper()).ToArray();

            foreach (string prefferredFont in preferredFonts)
                foreach (string installedFont in installedFonts)
                    if (installedFont.ToUpper().Contains(prefferredFont))
                        return installedFont;

            return SystemFonts.DefaultFont.Name;
        }
    }
}
