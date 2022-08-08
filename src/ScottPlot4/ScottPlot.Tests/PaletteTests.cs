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

        [Test]
        public void Test_Custom_Palette()
        {
            string[] customColors = { "#019d9f", "#7d3091", "#57e075", "#e5b5fa", "#009118" };
            var pal = ScottPlot.Palette.FromHtmlColors(customColors);
            Assert.That(pal.Colors.Length, Is.EqualTo(customColors.Length));
        }
    }
}
