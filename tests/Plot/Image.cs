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
        public void Test_TextRotationAlignment_UpperLeft()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap("Images/niceBackground.bmp");

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                plt.PlotImage(image: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.upperLeft,
                    frame: true,
                    frameColor: Color.LightGray,
                    frameSize: 5);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.upperLeft");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_TextRotationAlignment_UpperCenter()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap("Images/niceBackground.bmp");

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                plt.PlotImage(image: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.upperCenter,
                    frame: true,
                    frameColor: Color.LightGray,
                    frameSize: 5);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.upperCenter");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_TextRotationAlignment_LowerRight()
        {
            var plt = new ScottPlot.Plot(400, 300);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap("Images/niceBackground.bmp");

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                double x = i / 2;
                double y = i % 2;
                plt.PlotImage(image: image,
                    x, y,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.lowerRight,
                    frame: true,
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
