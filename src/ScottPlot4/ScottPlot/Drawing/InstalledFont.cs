using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Drawing
{
    public static class InstalledFont
    {
        private static Dictionary<string, FontFamily> _installedFonts;

        static InstalledFont()
        {
            BuildInstalledFontsCache();
        }

        internal static FontFamily SerifFamily { get; private set; }
        internal static FontFamily SansFamily { get; private set; }
        internal static FontFamily MonospaceFamily { get; private set; }

        public static string Default() => SansFamily.Name;
        public static string Serif() => SerifFamily.Name;
        public static string Sans() => SansFamily.Name;
        public static string Monospace() => MonospaceFamily.Name;

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
        internal static FontFamily ValidFontFamily(string fontName)
        {
            if (fontName != null && _installedFonts.TryGetValue(fontName, out FontFamily installedFont))
                return installedFont;
            return SystemFonts.DefaultFont.FontFamily;
        }

        /// <summary>
        /// Returns a font family guaranteed to be installed on the system
        /// </summary>
        internal static FontFamily ValidFontFamily(string[] fontNames)
        {
            foreach (string preferredFont in fontNames)
                if (preferredFont != null && _installedFonts.TryGetValue(preferredFont, out var installedFont))
                    return installedFont;
            return SystemFonts.DefaultFont.FontFamily;
        }

        internal static void BuildInstalledFontsCache()
        {
            var newFonts = new Dictionary<string, FontFamily>(StringComparer.OrdinalIgnoreCase);
            foreach (var family in FontFamily.Families)
            {
                if (!newFonts.ContainsKey(family.Name))
                    newFonts[family.Name] = family;
            }

            _installedFonts = newFonts;
            SerifFamily = ValidFontFamily(new string[] { "Times New Roman", "DejaVu Serif", "Times" });
            SansFamily = ValidFontFamily(new string[] { "Segoe UI", "DejaVu Sans", "Helvetica" });
            MonospaceFamily = ValidFontFamily(new string[] { "Consolas", "DejaVu Sans Mono", "Courier" });
        }
    }
}

/*
 
These fonts are on Azure Pipelines on Linux:
    Century Schoolbook L
    DejaVu Sans
    DejaVu Sans Mono
    DejaVu Serif
    Dingbats
    Liberation Mono
    Liberation Sans
    Liberation Sans Narrow
    Liberation Serif
    Nimbus Mono L
    Nimbus Roman No9 L
    Nimbus Sans L
    Standard Symbols L
    URW Bookman L
    URW Chancery L
    URW Gothic L
    URW Palladio L



These fonts are on Azure Pipelines on MacOS:
    Apple Braille
    Apple Color Emoji
    Apple SD Gothic Neo
    Apple Symbols
    Arial Hebrew
    Arial Hebrew Scholar
    Avenir
    Avenir Next
    Avenir Next Condensed
    Courier
    Geeza Pro
    Geneva
    Heiti SC
    Heiti TC
    Helvetica
    Helvetica Neue
    Hiragino Kaku Gothic Pro
    Hiragino Kaku Gothic ProN
    Hiragino Kaku Gothic Std
    Hiragino Kaku Gothic StdN
    Hiragino Maru Gothic Pro
    Hiragino Maru Gothic ProN
    Hiragino Mincho Pro
    Hiragino Mincho ProN
    Hiragino Sans
    Hiragino Sans GB
    Kohinoor Bangla
    Kohinoor Devanagari
    Kohinoor Telugu
    Lucida Grande
    Marker Felt
    Menlo
    Monaco
    Noteworthy
    Noto Nastaliq Urdu
    Optima
    Palatino
    PingFang HK
    PingFang SC
    PingFang TC
    Symbol
    System Font
    Thonburi
    Times
    Zapf Dingbats
    System Font
    .Apple Color Emoji UI
    .Apple SD Gothic NeoI
    .Aqua Kana
    .Aqua Kana
    .Arabic UI Display
    .Arabic UI Text
    .Arial Hebrew Desk Interface
    .Geeza Pro Interface
    .Geeza Pro PUA
    .Helvetica LT MM
    .Helvetica Neue DeskInterface
    .Hiragino Kaku Gothic Interface
    .Hiragino Sans GB Interface
    .Keyboard
    .LastResort
    .Lucida Grande UI
    .Noto Nastaliq Urdu UI
    .PingFang HK
    .PingFang SC
    .PingFang TC
    .SF Compact Display
    .SF Compact Rounded
    .SF Compact Text
    .SF NS Display Condensed
    .SF NS Rounded
    .SF NS Symbols
    .SF NS Text Condensed
    .Times LT MM

*/
