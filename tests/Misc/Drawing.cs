using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class Drawing
    {
        [Test]
        public void Test_Drawing_LinesOnPolygons()
        {
            var bmp = new System.Drawing.Bitmap(320, 240);

            using (var pen = new Pen(Color.Blue, 2))
            using (var brush = new SolidBrush(Color.LightGreen))
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(Color.White);

                Point[] points = {
                    new Point(75, 100),
                    new Point(250, 75),
                    new Point(280, 200),
                    new Point(100, 220)
                };

                gfx.FillPolygon(brush, points);
                gfx.DrawLine(pen, points[0], points[1]);
            }

            TestTools.SaveFig(bmp);
        }

        [Test]
        public void Test_PolygonVsScatter_Alignment()
        {
            double[] xs = { 75, 250, 280, 100 };
            double[] ys = { -100, -75, -200, -220 };

            var plt = new ScottPlot.Plot(320, 240);
            plt.PlotPolygon(xs, ys, fillColor: Color.LightGreen);
            plt.PlotLine(xs[0], ys[0], xs[1], ys[1], Color.Blue);
            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);
            plt.Title("Line/Scatter");

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PolygonVsSignal_Alignment()
        {
            double[] xs = { 75, 250, 280, 100 };
            double[] ys = { -100, -75, -200, -220 };

            var plt = new ScottPlot.Plot(320, 240);
            plt.PlotPolygon(xs, ys, fillColor: Color.LightGreen);
            plt.PlotSignal(
                ys: new double[] { ys[0], ys[1] },
                sampleRate: 1.0 / (xs[1] - xs[0]),
                xOffset: xs[0],
                color: Color.Blue,
                markerSize: 0
                );
            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);
            plt.Title("Signal");

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PolygonVsSignal_AlignmentWithLargeValues()
        {
            double[] xs = { 1e6 + 75, 1e6 + 250, 1e6 + 280, 1e6 + 100 };
            double[] ys = { 1e6 - 100, 1e6 - 75, 1e6 - 200, 1e6 - 220 };

            var plt = new ScottPlot.Plot(320, 240);
            plt.PlotPolygon(xs, ys, fillColor: Color.LightGreen);
            plt.PlotSignal(
                ys: new double[] { ys[0], ys[1] },
                sampleRate: 1.0 / (xs[1] - xs[0]),
                xOffset: xs[0],
                color: Color.Blue,
                markerSize: 0
                );
            plt.Grid(false);
            plt.Frame(false);
            plt.Ticks(false, false);
            plt.Title("Large Value Signal");

            TestTools.SaveFig(plt);
        }
    }
}
