using NUnit.Framework;
using ScottPlot;
using System;

namespace ScottPlotTests
{
    class Multiplot
    {
        private ScottPlot.MultiPlot SampleMultiPlot()
        {
            var multiplot = new ScottPlot.MultiPlot(width: 800, height: 600, rows: 2, cols: 2);

            // plot an increasng spread of data in each subplot
            Random rand = new Random(0);
            int pointCount = 100;
            for (int i = 0; i < multiplot.subplots.Length; i += 1)
            {
                double zoom = Math.Pow(i + 1, 2);
                multiplot.subplots[i].Title($"#{i}");
                multiplot.subplots[i].PlotScatter(
                        xs: ScottPlot.DataGen.Random(rand, pointCount, multiplier: zoom, offset: -.5 * zoom),
                        ys: ScottPlot.DataGen.Random(rand, pointCount, multiplier: zoom, offset: -.5 * zoom)
                    );
            }
            return multiplot;
        }

        private void DisplayAxisInfo(ScottPlot.MultiPlot multiplot)
        {
            for (int i = 0; i < multiplot.subplots.Length; i += 1)
                Console.WriteLine($"Subplot index {i} {multiplot.subplots[i].AxisLimits()}");
        }

        [Test]
        public void Test_MultiPlot_DefaultScales()
        {
            ScottPlot.MultiPlot multiplot = SampleMultiPlot();

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            multiplot.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");

            DisplayAxisInfo(multiplot);
        }

        [Test]
        public void Test_MultiPlot_MatchAxis()
        {
            ScottPlot.MultiPlot multiplot = SampleMultiPlot();

            // update the lower left (index 2) plot to use the scale of the lower right (index 3)
            multiplot.subplots[2].SetAxisLimits(multiplot.subplots[3].GetAxisLimits());
            multiplot.subplots[2].Title("#2 (matched to #3)");

            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string filePath = System.IO.Path.GetFullPath(name + ".png");
            multiplot.SaveFig(filePath);
            Console.WriteLine($"Saved {filePath}");
            DisplayAxisInfo(multiplot);

            var matchedAxisLimits = multiplot.subplots[2].GetAxisLimits();
            Assert.Greater(matchedAxisLimits.XMax, matchedAxisLimits.XMin);
            Assert.Greater(matchedAxisLimits.YMax, matchedAxisLimits.YMin);
        }
    }
}
