using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;

namespace ScottPlotTests
{
    public class Bitmap
    {
        [Test]
        public void Test_Plot_GetBitmap()
        {
            {
                var plt = new ScottPlot.Plot(321, 123);
                System.Drawing.Bitmap bmp = plt.Render();
                Assert.AreEqual(bmp.Width, 321);
                Assert.AreEqual(bmp.Height, 123);
            }

            {
                var plt = new ScottPlot.Plot(321, 123);
                System.Drawing.Bitmap bmp = plt.Render();
                Assert.AreEqual(bmp.Width, 321);
                Assert.AreEqual(bmp.Height, 123);
            }
        }

        [Test]
        public void Test_Plot_SaveFig()
        {
            {
                var plt = new ScottPlot.Plot(321, 123);
                string file = Path.GetTempPath() + "test.bmp";
                plt.SaveFig(file);
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 321);
                    Assert.AreEqual(bmp.Height, 123);
                }
            }

            {
                var plt = new ScottPlot.Plot(321, 123);
                string file = Path.GetTempPath() + "test.png";
                plt.SaveFig(file);
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file))
                {
                    Assert.AreEqual(bmp.Width, 321);
                    Assert.AreEqual(bmp.Height, 123);
                }
            }
        }

        [Test]
        public void Test_GetBitmapBytes_IsValidPng()
        {
            string outputFilePath = Path.GetFullPath("imageBytes.png");

            var plt = new ScottPlot.Plot(321, 123);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));
            byte[] bytes = plt.GetImageBytes();
            File.WriteAllBytes(outputFilePath, bytes);

            var bmp = new System.Drawing.Bitmap(outputFilePath);
            Assert.AreEqual(plt.Width, bmp.Width);
            Assert.AreEqual(plt.Height, bmp.Height);
        }

        [Test]
        public void Test_Plot_WidthAndHeight()
        {
            var plt = new ScottPlot.Plot(111, 222);
            Assert.AreEqual(111, plt.Width);
            Assert.AreEqual(222, plt.Height);

            plt.Resize(333, 444);
            Assert.AreEqual(333, plt.Width);
            Assert.AreEqual(444, plt.Height);

            plt.Width = 123;
            plt.Height = 321;
            var bmp = plt.GetBitmap();
            Assert.AreEqual(123, bmp.Width);
            Assert.AreEqual(321, bmp.Height);
        }
    }
}
