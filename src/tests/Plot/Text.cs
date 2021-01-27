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
                var txt = plt.AddText(label, x, y);
                txt.Rotation = rotations[i];
                txt.Alignment = ScottPlot.Alignment.UpperLeft;
                txt.Font.Size = 24;
                plt.AddPoint(x, y, Color.Red, size: 5);
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
                var txt = plt.AddText(label, x, y);
                txt.Rotation = rotations[i];
                txt.Alignment = ScottPlot.Alignment.UpperCenter;
                txt.Font.Size = 24;
                plt.AddPoint(x, y, Color.Red, size: 5);
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
                var txt = plt.AddText(label, x, y);
                txt.Rotation = rotations[i];
                txt.Alignment = ScottPlot.Alignment.LowerRight;
                txt.Font.Size = 24;
                plt.AddPoint(x, y, Color.Red, size: 5);
            }
            plt.Title("TextAlignment.lowerRight");
            plt.AxisAuto(.5, .5);
            TestTools.SaveFig(plt);
        }
    }
}
