using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public struct Font
    {
        private static readonly SKFontManager fontManager = SKFontManager.Default;

        public Font() { } // Required since we have field initializers

        public string Family { get; set; } = SKTypeface.Default.FamilyName;
        public int Size { get; set; } = 12;
        public SKFontStyleWeight Weight { get; set; } = SKFontStyleWeight.Normal;
        public bool Bold { 
            get => Weight >= SKFontStyleWeight.Bold; 
            set => Weight = value ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        }
        public bool Italic { get; set; } = false;

        private SKFontStyleSlant skFontSlant => Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        private SKFontStyle skFontStyle => new(Weight, SKFontStyleWidth.Normal, skFontSlant);

        // SKFontStyle implements IDisposable, but `FromFamilyName` adopts the font style and prevents disposal, so we don't need a using block here
        public SKTypeface GetTypeface() => SKTypeface.FromFamilyName(Family, skFontStyle);
        public SKFont GetFont() => new(GetTypeface(), Size);
        public static bool FontAvailable(string fontFamily) => fontManager.FontFamilies.Contains(fontFamily);
    }
}
