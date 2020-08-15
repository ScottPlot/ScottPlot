using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.ImageAnalysis
{
    class RGB
    {
        private void Grayscale(System.Drawing.Bitmap bmp)
        {
            System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
            for (int i = 0; i < 256; i++)
                pal.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
            bmp.Palette = pal;
        }

        private System.Drawing.Bitmap GetB(System.Drawing.Bitmap bmp) => GetChannel(bmp, 0);
        private System.Drawing.Bitmap GetG(System.Drawing.Bitmap bmp) => GetChannel(bmp, 1);
        private System.Drawing.Bitmap GetR(System.Drawing.Bitmap bmp) => GetChannel(bmp, 2);
        private System.Drawing.Bitmap GetChannel(System.Drawing.Bitmap bmp, int byteOffset)
        {
            byte[] bytes = ScottPlot.Tools.BitmapToBytes(bmp);
            int bytesPerPixel = 4;

            byte[] bytes2 = new byte[bytes.Length / bytesPerPixel];
            for (int i = 0; i < bytes2.Length; i++)
                bytes2[i] = bytes[i * bytesPerPixel + byteOffset];

            var bmp2 = ScottPlot.Tools.BitmapFromBytes(bytes2, bmp.Size);
            Grayscale(bmp2);
            return bmp2;
        }

        private (double A, double R, double G, double B) MeanPixel(System.Drawing.Bitmap bmp)
        {
            byte[] bytes = ScottPlot.Tools.BitmapToBytes(bmp);
            int bytesPerPixel = 4;
            int pixelCount = bytes.Length / bytesPerPixel;

            double R = 0;
            double G = 0;
            double B = 0;
            double A = 0;

            for (int i = 0; i < pixelCount; i++)
            {
                B += bytes[i * bytesPerPixel + 0];
                G += bytes[i * bytesPerPixel + 1];
                R += bytes[i * bytesPerPixel + 2];
                A += bytes[i * bytesPerPixel + 3];
            }

            return (A / pixelCount, R / pixelCount, G / pixelCount, B / pixelCount);
        }

        [NUnit.Framework.Test]
        public void Test_RGB_analysis()
        {
            Random rand = new Random(0);
            double[] ys = DataGen.RandomWalk(rand, 100);

            var plt = new ScottPlot.Plot(400, 300);
            plt.Style(figBg: System.Drawing.Color.Gray, dataBg: System.Drawing.Color.Gray);

            plt.PlotSignal(ys, yOffset: 0, color: System.Drawing.Color.FromArgb(255, 0, 0), label: "red", lineWidth: 2);
            plt.PlotSignal(ys, yOffset: 1, color: System.Drawing.Color.FromArgb(0, 255, 0), label: "green", lineWidth: 3);
            plt.PlotSignal(ys, yOffset: 2, color: System.Drawing.Color.FromArgb(0, 0, 255), label: "blue", lineWidth: 4);
            plt.PlotSignal(ys, yOffset: 3, color: System.Drawing.Color.FromArgb(0, 0, 0), label: "black");
            plt.PlotSignal(ys, yOffset: 4, color: System.Drawing.Color.FromArgb(255, 255, 255), label: "white");
            plt.Legend();

            System.Drawing.Bitmap bmp = plt.GetBitmap();

            var means = MeanPixel(bmp);
            Console.WriteLine($"mean bitmap intensity (ARGB): {means.A}, {means.R}, {means.G}, {means.B}");
            // mean bitmap intensity (ARGB): 255, 123.4157, 124.8481, 126.327066666667

            Assert.AreEqual(means.A, 255); // image is not transparent
            Assert.Greater(means.B, means.G); // blue line is thicker than green line
            Assert.Greater(means.G, means.R); // green line is thicker than red line
        }
    }
}
