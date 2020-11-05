using NUnit.Framework;
using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class AxisStyling
    {
        [Test]
        public void Test_Axis_Left()
        {
            var dims = new PlotDimensions(
                figureSize: new SizeF(200, 500),
                dataSize: new SizeF(25, 400),
                dataOffset: new PointF(150, 50),
                axisLimits: new AxisLimits2D(-1, 1, -100, 100));

            var ticks = new TickCollection(verticalAxis: true);
            ticks.Recalculate(dims);

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Axis Label";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.SetTicks(ticks.tickPositionsMajor, ticks.tickLabels, ticks.tickPositionsMinor);

            using (var bmp = new System.Drawing.Bitmap((int)dims.Width, (int)dims.Height))
            using (var gfx = GDI.Graphics(bmp, lowQuality: true))
            {
                gfx.Clear(Color.White);
                axis.Render(dims, bmp);
                TestTools.SaveFig(bmp);
            }
        }
    }
}
