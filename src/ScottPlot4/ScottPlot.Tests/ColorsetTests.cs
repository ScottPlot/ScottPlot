using NUnit.Framework;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests
{
    class ColorsetTests
    {
        private Color darkColor = ColorTranslator.FromHtml("#2e3440");
        private ScottPlot.Plot TestColormap(IPalette cset, int lineWidth, bool dark = false)
        {
            var plt = new ScottPlot.Plot(600, 400);

            for (int i = 0; i < cset.Count(); i++)
            {
                double[] ys = DataGen.Sin(51, phase: -i / Math.PI / cset.Count());
                var sig = plt.AddSignal(ys, color: cset.GetColor(i));
                sig.MarkerSize = 0;
                sig.LineWidth = lineWidth;
            }

            if (dark)
            {
                plt.Style(Style.Gray1);
                plt.Style(darkColor, darkColor);
            }

            plt.Title($"Colorset '{cset.Name}' has {cset.Count()} colors");
            plt.AxisAuto(0);

            return plt;
        }

        [Test]
        public void Test_Colorset_Aurora() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.Aurora, 3));

        [Test]
        public void Test_Colorset_Nord() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.Nord, 3));

        [Test]
        public void Test_Colorset_Cat10() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.Category10, 2));

        [Test]
        public void Test_Colorset_Cat20() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.Category20, 1));

        [Test]
        public void Test_Colorset_Half() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.OneHalf, 3));

        [Test]
        public void Test_Colorset_HalfDark() =>
            TestTools.SaveFig(TestColormap(ScottPlot.Palette.OneHalfDark, 3, true));
    }
}
