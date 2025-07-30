using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Primitives
{
    public static class FontExtensions
    {
        public static SKFontStyleWidth ToSKFontStyleWidth(this FontWidth width)
        {
            return width switch
            {
                FontWidth.UltraCondensed => SKFontStyleWidth.UltraCondensed,
                FontWidth.ExtraCondensed => SKFontStyleWidth.ExtraCondensed,
                FontWidth.Condensed => SKFontStyleWidth.Condensed,
                FontWidth.SemiCondensed => SKFontStyleWidth.SemiCondensed,
                FontWidth.Normal => SKFontStyleWidth.Normal,
                FontWidth.SemiExpanded => SKFontStyleWidth.SemiExpanded,
                FontWidth.Expanded => SKFontStyleWidth.Expanded,
                FontWidth.ExtraExpanded => SKFontStyleWidth.ExtraExpanded,
                FontWidth.UltraExpanded => SKFontStyleWidth.UltraExpanded,
                _ => throw new ArgumentOutOfRangeException(nameof(width))
            };
        }

        public static SKFontStyleSlant ToSKFontStyleSlant(this FontSlant slant)
        {
            return slant switch
            {
                FontSlant.Upright => SKFontStyleSlant.Upright,
                FontSlant.Italic => SKFontStyleSlant.Italic,
                FontSlant.Oblique => SKFontStyleSlant.Oblique,
                _ => throw new ArgumentOutOfRangeException(nameof(slant))
            };
        }
    }
}
