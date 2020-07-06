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
        private ScottPlot.Plot TestColormap(Colorset cset, int lineWidth, bool dark = false)
        {
            var plt = new ScottPlot.Plot(600, 400);

            for (int i = 0; i < cset.Count(); i++)
            {
                double[] ys = DataGen.Sin(51,
                    //mult: 1 - .5 * i / cset.Count(),
                    phase: -i / Math.PI / cset.Count());
                plt.PlotSignal(ys, color: cset.GetColor(i), markerSize: 0, lineWidth: lineWidth);
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
            TestTools.SaveFig(TestColormap(Colorset.Aurora, 3));

        [Test]
        public void Test_Colorset_Nord() =>
            TestTools.SaveFig(TestColormap(Colorset.Nord, 3));

        [Test]
        public void Test_Colorset_Cat10() =>
            TestTools.SaveFig(TestColormap(Colorset.Category10, 2));

        [Test]
        public void Test_Colorset_Cat20() =>
            TestTools.SaveFig(TestColormap(Colorset.Category20, 1));

        [Test]
        public void Test_Colorset_Half() =>
            TestTools.SaveFig(TestColormap(Colorset.OneHalf, 3));

        [Test]
        public void Test_Colorset_HalfDark() =>
            TestTools.SaveFig(TestColormap(Colorset.OneHalfDark, 3, true));
    }
}
