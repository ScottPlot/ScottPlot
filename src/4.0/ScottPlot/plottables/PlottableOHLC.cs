using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableOHLC : Plottable
    {
        public OHLC[] ohlcs;
        bool candleFormat;
        bool autoWidth;
        bool sequential;
        Pen penUp;
        Pen penDown;
        Brush brushUp;
        Brush brushDown;

        public PlottableOHLC(OHLC[] ohlcs, bool candleFormat, bool autoWidth, Color colorUp, Color colorDown, bool sequential)
        {
            this.ohlcs = ohlcs;
            this.candleFormat = candleFormat;
            this.autoWidth = autoWidth;
            this.sequential = sequential;

            penUp = new Pen(colorUp);
            penDown = new Pen(colorDown);
            brushUp = new SolidBrush(colorUp);
            brushDown = new SolidBrush(colorDown);
        }

        public override string ToString()
        {
            return $"PlottableOHLC with {GetPointCount()} points";
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = ohlcs[0].time;
            limits[1] = ohlcs[0].time;
            limits[2] = ohlcs[0].low;
            limits[3] = ohlcs[0].high;

            for (int i = 1; i < ohlcs.Length; i++)
            {
                if (ohlcs[i].time < limits[0])
                    limits[0] = ohlcs[i].time;
                if (ohlcs[i].time > limits[1])
                    limits[1] = ohlcs[i].time;
                if (ohlcs[i].low < limits[2])
                    limits[2] = ohlcs[i].low;
                if (ohlcs[i].high > limits[3])
                    limits[3] = ohlcs[i].high;
            }

            if (sequential)
            {
                limits[0] = 0;
                limits[1] = ohlcs.Length - 1;
            }

            return new Config.AxisLimits2D(limits);
        }

        public override void Render(Settings settings)
        {
            if (candleFormat)
                RenderCandles(settings);
            else
                RenderOhlc(settings);
        }

        private double GetSmallestSpacing()
        {
            double smallestSpacing = double.PositiveInfinity;
            for (int i = 1; i < ohlcs.Length; i++)
                smallestSpacing = Math.Min(ohlcs[i].time - ohlcs[i - 1].time, smallestSpacing);
            return smallestSpacing;
        }

        public void RenderCandles(Settings settings)
        {
            double fractionalTickWidth = .7;
            float boxWidth = 10;

            if (autoWidth)
            {
                double spacingPx = GetSmallestSpacing() * settings.xAxisScale;
                boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);
            }

            for (int i = 0; i < ohlcs.Length; i++)
            {
                var ohlc = ohlcs[i];
                var ohlcTime = (sequential) ? i : ohlc.time;

                if (autoWidth == false)
                    boxWidth = (float)(ohlc.timeSpan * settings.xAxisScale / 2 * fractionalTickWidth);

                Pen pen = (ohlc.closedHigher) ? penUp : penDown;
                Brush brush = (ohlc.closedHigher) ? brushUp : brushDown;
                pen.Width = (boxWidth >= 2) ? 2 : 1;

                // the wick below the box
                PointF wickLowBot = settings.GetPixel(ohlcTime, ohlc.low);
                PointF wickLowTop = settings.GetPixel(ohlcTime, ohlc.lowestOpenClose);
                settings.gfxData.DrawLine(pen, wickLowBot, wickLowTop);

                // the wick above the box
                PointF wickHighBot = settings.GetPixel(ohlcTime, ohlc.highestOpenClose);
                PointF wickHighTop = settings.GetPixel(ohlcTime, ohlc.high);
                settings.gfxData.DrawLine(pen, wickHighBot, wickHighTop);

                // the candle
                PointF boxLowerLeft = settings.GetPixel(ohlcTime, ohlc.lowestOpenClose);
                PointF boxUpperRight = settings.GetPixel(ohlcTime, ohlc.highestOpenClose);
                settings.gfxData.FillRectangle(brush, boxLowerLeft.X - boxWidth, boxUpperRight.Y, boxWidth * 2, boxLowerLeft.Y - boxUpperRight.Y);
            }
        }

        public void RenderOhlc(Settings settings)
        {
            double fractionalTickWidth = .7;
            float boxWidth = 10;

            if (autoWidth)
            {
                double spacingPx = GetSmallestSpacing() * settings.xAxisScale;
                boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);
            }

            for (int i = 0; i < ohlcs.Length; i++)
            {
                var ohlc = ohlcs[i];
                var ohlcTime = (sequential) ? i : ohlc.time;

                if (autoWidth == false)
                    boxWidth = (float)(ohlc.timeSpan * settings.xAxisScale / 2 * fractionalTickWidth);

                Pen pen = (ohlc.closedHigher) ? penUp : penDown;
                pen.Width = (boxWidth >= 2) ? 2 : 1;

                // the main line
                PointF wickTop = settings.GetPixel(ohlcTime, ohlc.low);
                PointF wickBot = settings.GetPixel(ohlcTime, ohlc.high);
                settings.gfxData.DrawLine(pen, wickBot, wickTop);

                // open and close lines
                float xPx = wickTop.X;
                float yPxOpen = (float)settings.GetPixel(0, ohlc.open).Y;
                float yPxClose = (float)settings.GetPixel(0, ohlc.close).Y;
                settings.gfxData.DrawLine(pen, xPx - boxWidth, yPxOpen, xPx, yPxOpen);
                settings.gfxData.DrawLine(pen, xPx + boxWidth, yPxClose, xPx, yPxClose);
            }
        }

        public override int GetPointCount()
        {
            return ohlcs.Length;
        }

        public override LegendItem[] GetLegendItems()
        {
            return null; // don't show this in the legend
        }
    }
}
