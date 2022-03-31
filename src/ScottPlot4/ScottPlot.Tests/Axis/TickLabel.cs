using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    class TickLabel
    {
        [Test]
        public void Test_TickLabel_CustomFunction()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            static string customTickFormatter(double tickPosition)
            {
                if (tickPosition >= 0)
                    return "+" + tickPosition.ToString("F2");
                else
                    return $"({Math.Abs(tickPosition).ToString("F2")})";
            };

            plt.XAxis.TickLabelFormat(customTickFormatter);
            plt.YAxis.TickLabelFormat(customTickFormatter);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_TickLabel_CustomFunction2()
        {
            var plt = new ScottPlot.Plot(400, 300);

            var prices = ScottPlot.DataGen.RandomStockPrices(new Random(0), 50, new TimeSpan(0, 15, 0));
            plt.AddOHLCs(prices);

            static string customTickFormatter(double tickPosition)
            {
                return DateTime.FromOADate(tickPosition).ToString("H:m");
            }

            plt.XAxis.TickLabelFormat(customTickFormatter);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Label_Rotation()
        {
            var plt = new ScottPlot.Plot(400, 300);

            plt.XAxis.Label("Test Label");
            plt.XAxis.TickLabelStyle(rotation: 90);
            plt.XAxis.LabelStyle(rotation: 180);

            plt.YAxis.Label("Test Label");
            plt.YAxis.TickLabelStyle(rotation: 90);
            plt.YAxis.LabelStyle(rotation: 90);

            plt.XAxis2.Label("Test Label");
            plt.XAxis2.Ticks(true);
            plt.XAxis2.TickLabelStyle(rotation: 90);
            plt.XAxis2.LabelStyle(rotation: 180);

            plt.YAxis2.Label("Test Label");
            plt.YAxis2.Ticks(true);
            plt.YAxis2.TickLabelStyle(rotation: 90);
            plt.YAxis2.LabelStyle(rotation: 90);

            TestTools.SaveFig(plt);
        }
    }
}
