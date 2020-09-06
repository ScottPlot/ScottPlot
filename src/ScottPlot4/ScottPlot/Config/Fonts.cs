﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace ScottPlot.Config
{
    public static class Fonts
    {
        // TODO: use a new font function and only use it to enforce use of "GraphicsUnit.Pixel" everywhere

        public static string GetDefaultFontName()
        {
            return GetSansFontName();
        }

        public static string GetSerifFontName()
        {
            string[] serifFonts = new string[] { "Times New Roman", "DejaVu Serif", "Times" };
            return GetValidFontName(serifFonts);
        }

        public static string GetSansFontName()
        {
            string[] sansFonts = new string[] { "Segoe UI", "DejaVu Sans", "Helvetica" };
            return GetValidFontName(sansFonts);
        }

        public static string GetMonospaceFontName()
        {
            string[] monospaceFonts = new string[] { "Consolas", "DejaVu Sans Mono", "Courier" };
            return GetValidFontName(monospaceFonts);
        }

        public static string GetValidFontName(string fontName)
        {
            foreach (FontFamily installedFont in FontFamily.Families)
                if (string.Equals(installedFont.Name, fontName, System.StringComparison.OrdinalIgnoreCase))
                    return installedFont.Name;

            return GetDefaultFontName();
        }

        public static string GetValidFontName(string[] fontNames)
        {
            foreach (string preferredFont in fontNames)
                foreach (FontFamily font in FontFamily.Families)
                    if (string.Equals(preferredFont, font.Name, System.StringComparison.OrdinalIgnoreCase))
                        return font.Name;

            return SystemFonts.DefaultFont.Name;
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
