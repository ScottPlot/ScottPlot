using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace ScottPlotTests.UnitTests
{
    internal class Palette
    {
        [Test]
        public void Test_Palette_IndexedColorsRollOver()
        {
            var pal = ScottPlot.Palettes.Default;
            Assert.AreEqual(pal.GetColor(0), pal.GetColor(pal.Count));

            for (int i = 0; i < pal.Count * 3; i++)
                Console.WriteLine($"{i + 1} of {pal.Count}: {pal.GetColor(i).ToHex()}");
        }

        [Test]
        public void Test_Palette_MayNotBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => new ScottPlot.Palette(null!));
            Assert.Throws<ArgumentException>(() => new ScottPlot.Palette(Array.Empty<Color>()));
        }
    }
}
