using System;
using NUnit.Framework;

namespace ScottPlotTests
{
    internal class PaletteTests
    {
        [Test]
        public void Test_GetPalette_ReturnsPalettes()
        {
            var palettes = ScottPlot.Palette.GetPalettes();
            Assert.IsNotNull(palettes);
            Assert.IsNotEmpty(palettes);
            foreach (var palette in palettes)
                Console.WriteLine(palette);
        }
    }
}
