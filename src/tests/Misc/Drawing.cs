﻿using NUnit.Framework;
using ScottPlot;
using ScottPlot.Drawing;
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
            plt.AddPolygon(xs, ys, fillColor: Color.LightGreen);
            plt.AddLine(xs[0], ys[0], xs[1], ys[1], Color.Blue);
            plt.Grid(false);
            plt.Frame(false);
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);
            plt.Title("Line/Scatter");

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PolygonVsSignal_Alignment()
        {
            double[] xs = { 75, 250, 280, 100 };
            double[] ys = { -100, -75, -200, -220 };

            var plt = new ScottPlot.Plot(320, 240);
            plt.AddPolygon(xs, ys, fillColor: Color.LightGreen);
            var sig = plt.AddSignal(
                ys: new double[] { ys[0], ys[1] },
                sampleRate: 1.0 / (xs[1] - xs[0]),
                color: Color.Blue);
            sig.MarkerSize = 0;
            sig.OffsetX = xs[0];

            plt.Grid(false);
            plt.Frame(false);
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);
            plt.Title("Signal");

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PolygonVsSignal_AlignmentWithLargeValues()
        {
            double[] xs = { 1e6 + 75, 1e6 + 250, 1e6 + 280, 1e6 + 100 };
            double[] ys = { 1e6 - 100, 1e6 - 75, 1e6 - 200, 1e6 - 220 };

            var plt = new ScottPlot.Plot(320, 240);
            plt.AddPolygon(xs, ys, fillColor: Color.LightGreen);
            var sig = plt.AddSignal(
                ys: new double[] { ys[0], ys[1] },
                sampleRate: 1.0 / (xs[1] - xs[0]),
                color: Color.Blue);
            sig.MarkerSize = 0;
            sig.OffsetX = xs[0];
            plt.Grid(false);
            plt.Frame(false);
            plt.XAxis.Ticks(false);
            plt.YAxis.Ticks(false);
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
            plt.AddScatter(xs, ys, markerSize: 0);
            plt.AddScatter(ys, xs, markerSize: 0);
            plt.AxisAuto(0, 0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_TickMinor_OnePixelOffFromMajor()
        {
            // this makes the horizontal axis tick near 2.0 look bad
            var plt = new ScottPlot.Plot(400, 300);
            plt.SetAxisLimits(0, 3, 0, 3);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MeasureString_ShowSize()
        {
            string testString = "ScottPlot";

            var fontName = InstalledFont.Default();
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
        public void Test_LettersDontRenderAsRectangles_SerifFont()
        {
            // this test ensures letters don't render as rectangles
            // https://github.com/ScottPlot/ScottPlot/issues/1079

            System.Drawing.Bitmap bmp = new(200, 100);
            using var gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            string[] fontNames = { InstalledFont.Serif(), InstalledFont.Sans(), InstalledFont.Monospace() };

            foreach (string fontName in fontNames)
            {
                gfx.Clear(Color.Navy);
                System.Drawing.Font fnt = new(fontName, 18);

                gfx.DrawString("tttt", fnt, Brushes.Yellow, 10, 10);
                string hash1 = ScottPlot.Tools.BitmapHash(bmp);

                gfx.DrawString("eeee", fnt, Brushes.Yellow, 10, 10);
                string hash2 = ScottPlot.Tools.BitmapHash(bmp);

                Assert.AreNotEqual(hash1, hash2);
            }
        }
    }
}
