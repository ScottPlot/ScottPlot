using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Drawing
{
    /// <summary>
    /// This class is used to retrieve OS-agnostic fonts using those known to be installed on the system.
    /// </summary>
    public static class InstalledFont
    {
        private readonly static Dictionary<string, FontFamily> InstalledFonts;

        static InstalledFont()
        {
            // non-Windows systems may have multiple fonts with the same names
            InstalledFonts = FontFamily.Families
                .GroupBy(x => x.Name, StringComparer.InvariantCultureIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.InvariantCultureIgnoreCase);

            SerifFamily = ValidFontFamily(new string[] { "Times New Roman", "DejaVu Serif", "Times" });
            SansFamily = ValidFontFamily(new string[] { "Segoe UI", "DejaVu Sans", "Helvetica" });
            MonospaceFamily = ValidFontFamily(new string[] { "Consolas", "DejaVu Sans Mono", "Courier" });
        }

        internal static FontFamily SerifFamily { get; private set; }
        internal static FontFamily SansFamily { get; private set; }
        internal static FontFamily MonospaceFamily { get; private set; }

        public static string Default() => SansFamily.Name;
        public static string Serif() => SerifFamily.Name;
        public static string Sans() => SansFamily.Name;
        public static string Monospace() => MonospaceFamily.Name;
        public static string[] Names() => InstalledFonts.Values.Select(x => x.Name).ToArray();

        /// <summary>
        /// Returns a font name guaranteed to be installed on the system
        /// </summary>
        public static string ValidFontName(string fontName)
        {
            return ValidFontFamily(fontName).Name;
        }

        /// <summary>
        /// Returns a font name guaranteed to be installed on the system
        /// </summary>
        public static string ValidFontName(string[] fontNames)
        {
            return ValidFontFamily(fontNames).Name;
        }

        /// <summary>
        /// Returns a font family guaranteed to be installed on the system
        /// </summary>
        public static FontFamily ValidFontFamily(string fontName)
        {
            if (fontName is not null && InstalledFonts.ContainsKey(fontName))
                return InstalledFonts[fontName];

            return SystemFonts.DefaultFont.FontFamily;
        }

        /// <summary>
        /// Returns a font family guaranteed to be installed on the system
        /// </summary>
        public static FontFamily ValidFontFamily(string[] fontNames)
        {
            foreach (string fontName in fontNames)
            {
                if (fontName is not null && InstalledFonts.ContainsKey(fontName))
                    return InstalledFonts[fontName];
            }

            return SystemFonts.DefaultFont.FontFamily;
        }
    }
}
