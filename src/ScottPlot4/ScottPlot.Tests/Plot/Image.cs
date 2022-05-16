using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ScottPlotTests.Plot
{
    class Image
    {
        [Test]
        public void Test_ImageRotationAlignment_UpperLeft()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = ScottPlot.DataGen.SampleImage();

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                var img = plt.AddImage(image, x, y);
                img.Rotation = rotations[i];
                img.Alignment = ScottPlot.Alignment.UpperLeft;
                img.BorderColor = Color.LightGray;
                img.BorderSize = 5;
                plt.AddPoint(x, y, Color.Red, size: 5);
            }
            plt.Title("ImageRotationAlignment.upperLeft");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageRotationAlignment_UpperCenter()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = ScottPlot.DataGen.SampleImage();

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                var img = plt.AddImage(image, x, y);
                img.Rotation = rotations[i];
                img.Alignment = ScottPlot.Alignment.UpperCenter;
                img.BorderColor = Color.LightGray;
                img.BorderSize = 5;
                plt.AddPoint(x, y, Color.Red, size: 5);
            }
            plt.Title("ImageRotationAlignment.upperCenter");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageRotationAlignment_LowerRight()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = ScottPlot.DataGen.SampleImage();

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                var img = plt.AddImage(image, x, y);
                img.Rotation = rotations[i];
                img.Alignment = ScottPlot.Alignment.LowerRight;
                img.BorderColor = Color.LightGray;
                img.BorderSize = 5;
                plt.AddPoint(x, y, Color.Red, size: 5);
            }
            plt.Title("ImageRotationAlignment.lowerRight");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_ImageScale()
        {
            var plt = new ScottPlot.Plot(800, 600);
            System.Drawing.Bitmap image = ScottPlot.DataGen.SampleImage();

            float[] scales = { 0.2f, 0.5f, 1f, 1.5f };
            for (int i = 0; i < scales.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                var img = plt.AddImage(image, x, y);
                img.Scale = scales[i];
                img.Rotation = 30;
                img.Alignment = ScottPlot.Alignment.LowerRight;
                img.BorderColor = Color.LightGray;
                img.BorderSize = 5;
                plt.AddPoint(x, y, Color.Red, size: 5);
            }
            plt.Title("ImageScale");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }
    }
}
