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

/*
 
These fonts are on Azure Pipelines on Linux:
    Nimbus Mono L
    Liberation Mono
    Nimbus Sans L
    Century Schoolbook L
    Liberation Serif
    Liberation Sans
    Standard Symbols L
    DejaVu Serif
    URW Chancery L
    URW Gothic L
    URW Palladio L
    Dingbats
    DejaVu Sans Mono
    URW Bookman L
    Liberation Sans Narrow
    Nimbus Roman No9 L
    DejaVu Sans


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
