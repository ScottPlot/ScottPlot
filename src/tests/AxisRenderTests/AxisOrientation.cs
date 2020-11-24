using NUnit.Framework;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.AxisRenderTests
{
    class AxisOrientation
    {
        public PlotDimensions DimsLeft =>
            new PlotDimensions(
                figureSize: new SizeF(100, 500),
                dataSize: new SizeF(20, 400),
                dataOffset: new PointF(75, 50),
                axisLimits: (-1, 1, -100, 100));

        public PlotDimensions DimsRight =>
            new PlotDimensions(
                figureSize: new SizeF(100, 500),
                dataSize: new SizeF(20, 400),
                dataOffset: new PointF(5, 50),
                axisLimits: (-1, 1, -100, 100));

        public PlotDimensions DimsTop =>
            new PlotDimensions(
                figureSize: new SizeF(500, 100),
                dataSize: new SizeF(450, 20),
                dataOffset: new PointF(25, 75),
                axisLimits: (-1, 1, -100, 100));

        public PlotDimensions DimsBottom =>
            new PlotDimensions(
                figureSize: new SizeF(500, 100),
                dataSize: new SizeF(450, 20),
                dataOffset: new PointF(25, 5),
                axisLimits: (-1, 1, -100, 100));

        [Test]
        public void Test_Axis_Left()
        {
            var dims = DimsLeft;

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Left Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Left;
            axis.Ticks.TickCollection.verticalAxis = true;

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

        [Test]
        public void Test_Axis_Right()
        {
            var dims = DimsRight;

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Right Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Right;
            axis.Ticks.TickCollection.verticalAxis = true;

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

        [Test]
        public void Test_Axis_Top()
        {
            var dims = DimsTop;

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Top Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Top;
            axis.Ticks.TickCollection.verticalAxis = false;

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

        [Test]
        public void Test_Axis_Bottom()
        {
            var dims = DimsBottom;

            var axis = new ScottPlot.Renderable.Axis();
            axis.Title.Label = "Sample Bottom Axis";
            axis.Edge = ScottPlot.Renderable.Edge.Bottom;
            axis.Ticks.TickCollection.verticalAxis = false;

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
