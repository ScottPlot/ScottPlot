using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        [Test]
        public void Test_TickAlignment_SnapEdgePixel()
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Range(0, 10, .1, true);
            double[] ys = DataGen.RandomWalk(rand, xs.Length, .5);

            var plt = new ScottPlot.Plot(320, 240);
            plt.PlotScatter(xs, ys, markerSize: 0);
            plt.PlotScatter(ys, xs, markerSize: 0);
            plt.AxisAuto(0, 0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_TickMinor_OnePixelOffFromMajor()
        {
            // this makes the horizontal axis tick near 2.0 look bad
            var plt = new ScottPlot.Plot(400, 300);
            plt.Axis(0, 3, 0, 3);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MeasureString_ShowSize()
        {
            string testString = "ScottPlot";

            var fontName = ScottPlot.Config.Fonts.GetDefaultFontName();
            float fontSize = 14;

            // an active Graphics object is required to measure a string...
            using (var bmp = new System.Drawing.Bitmap(1, 1))
            using (var gfx = System.Drawing.Graphics.FromImage(bmp))
            using (var font = new System.Drawing.Font(fontName, fontSize))
            {
                var stringSize = ScottPlot.Drawing.GDI.MeasureString(gfx, testString, font);

                var sb = new StringBuilder();
                sb.AppendLine(System.Environment.OSVersion.ToString());
                sb.AppendLine($"The string '{testString}' with font '{fontName}' (size {fontSize}) " +
                    $"measures: {stringSize.Width}px x {stringSize.Height}px");

                Console.WriteLine(sb);
            }
        }

        [Test]
        public void Test_RenderingArtifacts_Demonstrate()
        {
            // Due to a bug in System.Drawing the drawing of perfectly straight lines is
            // prone to rendering artifacts (diagonal lines) when anti-aliasing is off.
            // https://github.com/swharden/ScottPlot/issues/327
            // https://github.com/swharden/ScottPlot/issues/401

            var plt = new ScottPlot.Plot(400, 300);
            plt.Grid(xSpacing: 2, ySpacing: 2, color: Color.Red);
            plt.Axis(-13, 13, -10, 10);

            // create conditions to reveal rendering artifact
            plt.AntiAlias(false, false, false);
            plt.Grid(enableVertical: false);

            // save the figure (bmpData + bmpFigure)
            TestTools.SaveFig(plt);

            // save the data bitmap too
            string gfxFilePath = System.IO.Path.GetFullPath("diag.png");
            plt.GetSettings(false).bmpData.Save(gfxFilePath, ImageFormat.Png);
            Console.WriteLine($"SAVED: {gfxFilePath}");
        }
    }
}
