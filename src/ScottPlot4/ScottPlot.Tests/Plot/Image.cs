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
                plt.PlotBitmap(bitmap: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.ImageAlignment.upperLeft,
                    frameColor: Color.LightGray,
                    frameSize: 5);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.upperLeft");
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
                plt.PlotBitmap(bitmap: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.ImageAlignment.upperCenter,
                    frameColor: Color.LightGray,
                    frameSize: 5);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.upperCenter");
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
                plt.PlotBitmap(bitmap: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.ImageAlignment.lowerRight,
                    frameColor: Color.LightGray,
                    frameSize: 5);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.lowerRight");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }
    }
}
