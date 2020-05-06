using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.Plot
{
    class Text
    {
        [Test]
        public void Test_TextRotationAlignment_UpperLeft()
        {
            var plt = new ScottPlot.Plot(400, 300);

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                string label = $"Rot{rotations[i]}";
                double x = i / 2;
                double y = i % 2;
                plt.PlotText(label, x, y, Color.Black,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.upperLeft,
                    fontSize: 24,
                    frame: true,
                    frameColor: Color.LightGray);
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

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                string label = $"Rot{rotations[i]}";
                double x = i / 2;
                double y = i % 2;
                plt.PlotText(label, x, y, Color.Black,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.upperCenter,
                    fontSize: 24,
                    frame: true,
                    frameColor: Color.LightGray);
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

            int[] rotations = { 0, 90, -90, 180 };
            for (int i = 0; i < rotations.Length; i++)
            {
                string label = $"Rot{rotations[i]}";
                double x = i / 2;
                double y = i % 2;
                plt.PlotText(label, x, y, Color.Black,
                    rotation: rotations[i],
                    alignment: ScottPlot.TextAlignment.lowerRight,
                    fontSize: 24,
                    frame: true,
                    frameColor: Color.LightGray);
                plt.PlotPoint(x, y, Color.Red, markerSize: 5);
            }
            plt.Title("TextAlignment.lowerRight");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }
    }
}
