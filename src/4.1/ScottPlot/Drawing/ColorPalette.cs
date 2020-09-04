using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Drawing
{
    public class ColorPalette : IColorPalette
    {
        public static ColorPalette Category10 => new ColorPalette(new ColorPalettes.Category10());

        public readonly string Name;
        private readonly IColorPalette Pal;
        public int Count() => Pal.Count();
        public Color GetColor(int index) => Pal.GetColor(index);

        public ColorPalette(IColorPalette palette)
        {
            Pal = palette ?? new ColorPalettes.Category10();
            Name = palette.GetType().Name;
        }
    }
}
