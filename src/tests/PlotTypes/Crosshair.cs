using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class Crosshair
    {
        [Test]
        public void Test_CrossHair_Renders()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            plt.Title("Crosshair Demo");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.AddCrosshair(42, 0.45);

            // this is outside the data area so should not be rendered
            plt.AddCrosshair(-5, -1.2);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_CrossHair_DateTime()
        {
            var plt = new ScottPlot.Plot(400, 300);

            plt.Title("Crosshair with DateTime Axis");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            // plot DateTime data on the horizontal axis
            int pointCount = 100;
            Random rand = new(0);
            double[] ys = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            double[] xs = Enumerable.Range(0, pointCount)
                                    .Select(x => new DateTime(2016, 06, 27).AddDays(x))
                                    .Select(x => x.ToOADate()).ToArray();
            plt.AddScatter(xs, ys);
            plt.XAxis.DateTimeFormat(true);

            // add a crosshair
            var ch = plt.AddCrosshair(xs[50], ys[50]);

            // indicaite horizontal axis is DateTime and give a proper DateTime format string
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            ch.IsDateTimeX = true;
            ch.StringFormatX = "d";

            // use a numeric vertical axis but customize the format string
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            ch.IsDateTimeY = false;
            ch.StringFormatY = "F4";

            TestTools.SaveFig(plt);
        }
    }
}
