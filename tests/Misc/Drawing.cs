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
    }
}
