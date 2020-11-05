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
                figureSize: new SizeF(100, 500),
                dataSize: new SizeF(20, 400),
                dataOffset: new PointF(75, 50),
                axisLimits: new AxisLimits2D(-1, 1, -100, 100));

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Axis Label";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.TickCollection.Recalculate(dims);
            axis.TickCollection.verticalAxis = true;

            using (var bmp = new System.Drawing.Bitmap((int)dims.Width, (int)dims.Height))
            using (var gfx = GDI.Graphics(bmp, lowQuality: true))
            using (var brush = GDI.Brush(Color.FromArgb(25, Color.Black)))
            {
                gfx.Clear(Color.White);
                gfx.FillRectangle(brush, dims.DataOffsetX, dims.DataOffsetY, dims.DataWidth, dims.DataHeight);
                axis.Render(dims, bmp);
                TestTools.SaveFig(bmp);
            }
        }
    }
}
