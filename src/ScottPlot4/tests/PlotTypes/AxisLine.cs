using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class AxisLine
    {
        [Test]
        public void Test_LineLabels_LookGood()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var hline = plt.AddHorizontalLine(.5);
            var vline = plt.AddVerticalLine(25);
            var vline2 = plt.AddVerticalLine(30);

            hline.PositionLabel = true;
            hline.LineWidth = 2;
            hline.PositionLabelBackground = hline.Color;

            vline.PositionLabel = true;
            vline.LineWidth = 2;
            vline.PositionLabelBackground = vline.Color;

            vline2.PositionLabel = true;
            vline2.PositionLabelOppositeAxis = true;

            TestTools.SaveFig(plt);
        }
    }
}
