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
    }
}
