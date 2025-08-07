namespace ScottPlot
{
    public static class FontExtensions
    {
        public static SKFontStyleWidth ToSKFontStyleWidth(this FontSpacing width)
        {
            return width switch
            {
                FontSpacing.UltraCondensed => SKFontStyleWidth.UltraCondensed,
                FontSpacing.ExtraCondensed => SKFontStyleWidth.ExtraCondensed,
                FontSpacing.Condensed => SKFontStyleWidth.Condensed,
                FontSpacing.SemiCondensed => SKFontStyleWidth.SemiCondensed,
                FontSpacing.Normal => SKFontStyleWidth.Normal,
                FontSpacing.SemiExpanded => SKFontStyleWidth.SemiExpanded,
                FontSpacing.Expanded => SKFontStyleWidth.Expanded,
                FontSpacing.ExtraExpanded => SKFontStyleWidth.ExtraExpanded,
                FontSpacing.UltraExpanded => SKFontStyleWidth.UltraExpanded,
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

        public static SKFontStyleWeight ToSKFontStyleWeight(this FontWeight weight)
        {
            return weight switch
            {
                FontWeight.Invisible => SKFontStyleWeight.Invisible,
                FontWeight.Thin => SKFontStyleWeight.Thin,
                FontWeight.ExtraLight => SKFontStyleWeight.ExtraLight,
                FontWeight.Light => SKFontStyleWeight.Light,
                FontWeight.Normal => SKFontStyleWeight.Normal,
                FontWeight.Medium => SKFontStyleWeight.Medium,
                FontWeight.SemiBold => SKFontStyleWeight.SemiBold,
                FontWeight.Bold => SKFontStyleWeight.Bold,
                FontWeight.ExtraBold => SKFontStyleWeight.ExtraBold,
                FontWeight.Black => SKFontStyleWeight.Black,
                FontWeight.ExtraBlack => SKFontStyleWeight.ExtraBlack,
                _ => throw new ArgumentOutOfRangeException(nameof(weight))
            };
        }
    }
}
