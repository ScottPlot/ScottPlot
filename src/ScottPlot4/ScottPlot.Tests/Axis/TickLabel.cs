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

        [Test]
        public void Test_Format_CanBeReversed()
        {
            // Demonstrates bug described in https://github.com/ScottPlot/ScottPlot/pull/1813

            var plt = new ScottPlot.Plot(800, 600);
            var sig = plt.AddSignal(ScottPlot.DataGen.Sin(1000));

            // repeat this multiple times to ensure changes can be undone/redone
            for (int i = 0; i < 3; i++)
            {
                // custom NUMERIC format string
                plt.XAxis.TickLabelFormat("F4", dateTimeFormat: false);
                plt.Render();
                var expectedLabel = 0d.ToString("F4", plt.XAxis.AxisTicks.TickCollection.Culture);
                Assert.AreEqual(expectedLabel, plt.XAxis.GetTicks().First().Label);

                // custom DATETIME format string
                plt.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true);
                plt.Render();
                Assert.AreEqual("00:00:00", plt.XAxis.GetTicks().First().Label);

                // custom format FUNCTION
                static string customFormatter(double x) => "custom";
                plt.XAxis.TickLabelFormat(customFormatter);
                plt.Render();
                Assert.AreEqual("custom", plt.XAxis.GetTicks().First().Label);
            }
        }
    }
}
