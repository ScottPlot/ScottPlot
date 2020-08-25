using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Space
{
    class Coordinates
    {
        [Test]
        public void Test_Space_Coordinates()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            // test a point 10% (~38 pixels) from left edge of data area
            Assert.AreEqual(150 + 38, fig.GetPixelX(-.8), 1);

            // test a point 10% (~31 pixels) from top edge of data area
            Assert.AreEqual(50 + 31, fig.GetPixelY(8), 1);

            // test the same two points in reverse
            Assert.AreEqual(-.8, fig.GetPositionX(150 + 38), .01);
            Assert.AreEqual(8, fig.GetPositionY(50 + 31), .1);
        }

        [Test]
        public void Test_Space_SecondY()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);

            // set limits for primary X and primary Y
            fig.SetLimits(-123, 123, -123, 123);

            // set limits for primary X and secondary Y
            fig.SetLimits(-1, 1, -10, 10, planeIndex: 1);

            // test expected values of the primary X
            Assert.AreEqual(150 + 38, fig.GetPixelX(-.8), 1);

            // test expected values of the secondary Y
            Assert.AreEqual(150 + 38, fig.GetPixelX(-.8, planeIndex: 1), 1);
            Assert.AreEqual(50 + 31, fig.GetPixelY(8, planeIndex: 1), 1);
        }

        [Test]
        public void Test_Space_PixelAlignment()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            // create a test image where the corners of the plot are are marked with black pixels
            var bmp = new System.Drawing.Bitmap((int)fig.Width, (int)fig.Height);
            using (var gfx = Graphics.FromImage(bmp))
            using (var fillBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
            {
                gfx.Clear(Color.White);
                gfx.FillRectangle(fillBrush, fig.DataOffsetX, fig.DataOffsetY, fig.DataWidth, fig.DataHeight);

                // set pixels at the corners
                bmp.SetPixel((int)fig.GetPixelX(-1), (int)fig.GetPixelY(-10), Color.Black);
                bmp.SetPixel((int)fig.GetPixelX(1), (int)fig.GetPixelY(-10), Color.Black);
                bmp.SetPixel((int)fig.GetPixelX(-1), (int)fig.GetPixelY(10), Color.Black);
                bmp.SetPixel((int)fig.GetPixelX(1), (int)fig.GetPixelY(10), Color.Black);

                // set the pixel used in previous tests
                bmp.SetPixel((int)fig.GetPixelX(-.8), (int)fig.GetPixelY(8), Color.Black);

                //TestTools.SaveBitmap(bmp);
            }
        }

        public System.Drawing.Bitmap CrudePlot(ScottPlot.Space.FigureInfo fig)
        {
            var bmp = new System.Drawing.Bitmap((int)fig.Width, (int)fig.Height);
            using (var gfx = Graphics.FromImage(bmp))
            using (var fillBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
            using (var linePen = new Pen(Color.Blue, 3))
            {
                gfx.Clear(Color.White);
                gfx.FillRectangle(fillBrush, fig.DataOffsetX, fig.DataOffsetY, fig.DataWidth, fig.DataHeight);

                // draw a triangle across the whole figure area
                List<PointF> points = new List<PointF>();
                points.Add(new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8)));
                points.Add(new PointF(fig.GetPixelX(-.8), fig.GetPixelY(-8)));
                points.Add(new PointF(fig.GetPixelX(.8), fig.GetPixelY(8)));
                points.Add(new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8)));
                gfx.DrawLines(linePen, points.ToArray());
            }
            return bmp;
        }

        [Test]
        public void Test_Space_MousePan()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            //TestTools.SaveBitmap(CrudePlot(fig), "1");
            PointF cornerOriginal = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));

            fig.MousePan(10, 20);
            PointF cornerAfterPan = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "2");

            Assert.Greater(cornerAfterPan.X, cornerOriginal.X);
            Assert.Greater(cornerAfterPan.Y, cornerOriginal.Y);
        }

        [Test]
        public void Test_Space_MouseZoom()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            PointF cornerOriginal = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "1");

            fig.MouseZoom(5, 10);
            PointF cornerAfterZoom = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "2");

            Assert.Less(cornerAfterZoom.X, cornerOriginal.X);
            Assert.Greater(cornerAfterZoom.Y, cornerOriginal.Y);
        }

        [Test]
        public void Test_Space_MousePanMultipleRemember()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            PointF corner1 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "1");

            fig.MousePan(10, 20);
            PointF corner2 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "2");

            fig.MousePan(10, 20);
            PointF corner3 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "3");

            // each mouse action is applied and remembered (do this OnMouseUp)
            Assert.Greater(corner3.X, corner2.X);
            Assert.Greater(corner2.X, corner1.X);
        }

        [Test]
        public void Test_Space_MousePanMultipleForget()
        {
            var fig = new ScottPlot.Space.FigureInfo();
            fig.Resize(600, 400, 380, 310, 150, 50);
            fig.SetLimits(-1, 1, -10, 10);

            PointF corner1 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "1");

            fig.MousePan(10, 20, remember: false);
            PointF corner2 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "2");

            fig.MousePan(10, 20, remember: false);
            PointF corner3 = new PointF(fig.GetPixelX(-.8), fig.GetPixelY(8));
            //TestTools.SaveBitmap(CrudePlot(fig), "3");

            // each mouse action is applied but NOT remembered (do this OnMouseMove while panning or zooming)
            Assert.AreEqual(corner3.X, corner2.X);
            Assert.Greater(corner3.X, corner1.X);
            Assert.Greater(corner2.X, corner1.X);
        }
    }
}
