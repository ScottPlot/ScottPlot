﻿using System;
using System.Drawing;
using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;

namespace ScottPlot.Plottable
{
    public class FinancePlot : IRenderable, IUsesAxes, IValidatable
    {
        /// <summary>
        /// Array of prices (open high low close)
        /// </summary>
        public OHLC[] ohlcs;

        /// <summary>
        /// Display prices as filled candlesticks (otherwise display as OHLC lines)
        /// </summary>
        public bool Candle;

        /// <summary>
        /// Use ohlc timestamps to determine candle width (otherwise they are width ~1)
        /// </summary>
        public bool AutoWidth;

        /// <summary>
        /// If True then OHLC timestamps are ignored and candles are placed at x= 0, 1, 2, 3, etc.
        /// </summary>
        public bool Sqeuential;

        /// <summary>
        /// Color when the OHLC closed above the open price
        /// </summary>
        public Color ColorUp = Color.LightGreen;

        /// <summary>
        /// Color when the OHLC did not close above the open price
        /// </summary>
        public Color ColorDown = Color.LightCoral;

        // TODO: good first issue - create a ColorNeutral for when the candle closed at the same price it opened at

        public bool IsVisible { get; set; } = true;

        public override string ToString() => $"PlottableOHLC with {PointCount} points";

        public int PointCount { get => ohlcs.Length; }

        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public AxisLimits GetAxisLimits()
        {
            // TODO: dont use an array here
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

            if (Sqeuential)
            {
                limits[0] = 0;
                limits[1] = ohlcs.Length - 1;
            }

            return new AxisLimits(limits[0], limits[1], limits[2], limits[3]);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (Candle)
                RenderCandles(dims, bmp, lowQuality);
            else
                RenderOhlc(dims, bmp, lowQuality);
        }

        private double GetSmallestSpacing()
        {
            double smallestSpacing = double.PositiveInfinity;
            for (int i = 1; i < ohlcs.Length; i++)
                smallestSpacing = Math.Min(ohlcs[i].time - ohlcs[i - 1].time, smallestSpacing);
            return smallestSpacing;
        }

        public string ErrorMessage(bool deepValidation = false)
        {
            try
            {
                if (ohlcs is null)
                    throw new ArgumentException("ohlcs cannot be null");
                for (int i = 0; i < ohlcs.Length; i++)
                {
                    if (ohlcs[i] is null)
                        throw new ArgumentException($"ohlcs[{i}] cannot be null");
                    if (!ohlcs[i].IsValid)
                        throw new ArgumentException($"ohlcs[{i}] does not contain valid data");
                }
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }

            return null;
        }

        public void RenderCandles(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double fractionalTickWidth = .7;
            float boxWidth = 10;

            if (AutoWidth)
            {
                double spacingPx = GetSmallestSpacing() * dims.PxPerUnitX;
                boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);
            }

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = new Pen(Color.Magenta))
            using (SolidBrush brush = new SolidBrush(Color.Magenta))
            {
                for (int i = 0; i < ohlcs.Length; i++)
                {
                    var ohlc = ohlcs[i];
                    var ohlcTime = (Sqeuential) ? i : ohlc.time;
                    float pixelX = dims.GetPixelX(ohlcTime);

                    if (AutoWidth == false)
                        boxWidth = (float)(ohlc.timeSpan * dims.PxPerUnitX / 2 * fractionalTickWidth);

                    pen.Color = ohlc.closedHigher ? ColorUp : ColorDown;
                    brush.Color = ohlc.closedHigher ? ColorUp : ColorDown;
                    pen.Width = (boxWidth >= 2) ? 2 : 1;

                    // the wick below the box
                    PointF wickLowBot = new PointF(pixelX, dims.GetPixelY(ohlc.low));
                    PointF wickLowTop = new PointF(pixelX, dims.GetPixelY(ohlc.lowestOpenClose));
                    gfx.DrawLine(pen, wickLowBot, wickLowTop);

                    // the wick above the box
                    PointF wickHighBot = new PointF(pixelX, dims.GetPixelY(ohlc.highestOpenClose));
                    PointF wickHighTop = new PointF(pixelX, dims.GetPixelY(ohlc.high));
                    gfx.DrawLine(pen, wickHighBot, wickHighTop);

                    // the candle
                    PointF boxLowerLeft = new PointF(pixelX, dims.GetPixelY(ohlc.lowestOpenClose));
                    PointF boxUpperRight = new PointF(pixelX, dims.GetPixelY(ohlc.highestOpenClose));
                    gfx.FillRectangle(brush, boxLowerLeft.X - boxWidth, boxUpperRight.Y, boxWidth * 2, boxLowerLeft.Y - boxUpperRight.Y);
                }
            }
        }

        public void RenderOhlc(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double fractionalTickWidth = .7;
            float boxWidth = 10;

            if (AutoWidth)
            {
                double spacingPx = GetSmallestSpacing() * dims.PxPerUnitX;
                boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);
            }

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = new Pen(Color.Magenta))
            {
                for (int i = 0; i < ohlcs.Length; i++)
                {
                    var ohlc = ohlcs[i];
                    var ohlcTime = (Sqeuential) ? i : ohlc.time;
                    float pixelX = dims.GetPixelX(ohlcTime);

                    if (AutoWidth == false)
                        boxWidth = (float)(ohlc.timeSpan * dims.PxPerUnitX / 2 * fractionalTickWidth);

                    pen.Color = ohlc.closedHigher ? ColorUp : ColorDown;
                    pen.Width = (boxWidth >= 2) ? 2 : 1;

                    // the main line
                    PointF wickTop = new PointF(pixelX, dims.GetPixelY(ohlc.low));
                    PointF wickBot = new PointF(pixelX, dims.GetPixelY(ohlc.high));
                    gfx.DrawLine(pen, wickBot, wickTop);

                    // open and close lines
                    float xPx = wickTop.X;
                    float yPxOpen = dims.GetPixelY(ohlc.open);
                    float yPxClose = dims.GetPixelY(ohlc.close);
                    gfx.DrawLine(pen, xPx - boxWidth, yPxOpen, xPx, yPxOpen);
                    gfx.DrawLine(pen, xPx + boxWidth, yPxClose, xPx, yPxClose);
                }
            }
        }
    }
}
