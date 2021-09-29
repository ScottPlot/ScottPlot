using System;
using NUnit.Framework;
using ScottPlot;

namespace ScottPlotTests
{
    public class Theme
    {
        [Test]
        public void Test_theme_works()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            var theme = ScottPlot.Theme.Gray1;
            plt.Theme(theme);
            plt.Title(theme.ToString());
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            TestTools.SaveFig(plt);
        }
    }
}
