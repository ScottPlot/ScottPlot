using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests
{
    class LayoutTests
    {
        private Bitmap ShowLayout(Plot plt)
        {
            Bitmap bmp = plt.GetBitmap();
            using (var gfx = Graphics.FromImage(bmp))
            using (var labelBrush = new SolidBrush(Color.FromArgb(100, Color.Red)))
            using (var ticksBrush = new SolidBrush(Color.FromArgb(100, Color.Blue)))
            using (var dataBrush = new SolidBrush(Color.FromArgb(100, Color.Green)))
            {
                // the data rectangle is chief
                RectangleF dataRect = new RectangleF(plt.Info.DataOffsetX, plt.Info.DataOffsetY, plt.Info.DataWidth, plt.Info.DataHeight);
                gfx.FillRectangle(dataBrush, dataRect);

                RectangleF titleRect = new RectangleF(plt.Info.DataOffsetX, 0, plt.Info.DataWidth, plt.Title.Size.Height);
                gfx.FillRectangle(labelBrush, titleRect);

                RectangleF yLabelRect = new RectangleF(0, plt.Info.DataOffsetY, plt.YLabel.Size.Width, plt.Info.DataHeight);
                RectangleF yTicksRect = new RectangleF(yLabelRect.Width, yLabelRect.Y, plt.YTicks.Size.Width, yLabelRect.Height);
                gfx.FillRectangle(labelBrush, yLabelRect);
                gfx.FillRectangle(ticksBrush, yTicksRect);

                RectangleF y2LabelRect = new RectangleF(plt.Width - plt.Y2Label.Size.Width, plt.Info.DataOffsetY, plt.Y2Label.Size.Width, plt.Info.DataHeight);
                RectangleF y2TicksRect = new RectangleF(y2LabelRect.X - plt.Y2Ticks.Size.Width, plt.Info.DataOffsetY, plt.Y2Ticks.Size.Width, plt.Info.DataHeight);
                gfx.FillRectangle(labelBrush, y2LabelRect);
                gfx.FillRectangle(ticksBrush, y2TicksRect);

                RectangleF xLabelRect = new RectangleF(plt.Info.DataOffsetX, plt.Height - plt.XLabel.Size.Height, plt.Info.DataWidth, plt.XLabel.Size.Height);
                RectangleF xTicksRect = new RectangleF(plt.Info.DataOffsetX, xLabelRect.Y - plt.XTicks.Size.Height, plt.Info.DataWidth, plt.XTicks.Size.Height);
                gfx.FillRectangle(labelBrush, xLabelRect);
                gfx.FillRectangle(ticksBrush, xTicksRect);
            }
            return bmp;
        }

        [Test]
        public void Test_Layout_ResizesForBigLabels()
        {
            var plt = new Plot();
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            plt.PlotScatter(xs, ys);

            plt.Title.Text = "Title";
            //plt.Title.FontSize = 36;
            plt.XLabel.Text = "Horizontal Axis";
            plt.YLabel.Text = "Primary Vertical Axis";
            plt.Y2Label.Text = "Secondary Vertical Axis";

            plt.AutoLayout();
            plt.Axis(0, 50, -1, 1);

            TestTools.SaveFig(plt);
            TestTools.SaveFig(ShowLayout(plt));
        }
    }
}
