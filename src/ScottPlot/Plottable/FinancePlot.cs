using System;
using System.Drawing;
using ScottPlot.Drawing;
using System.Data;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Finance plots display open/high/low/close (OHLC) data
    /// </summary>
    public class FinancePlot : IPlottable
    {
        /// <summary>
        /// Array of prices (open high low close)
        /// </summary>
        public OHLC[] OHLCs;

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

        // customizations
        public bool IsVisible { get; set; } = true;
        public override string ToString() => $"PlottableOHLC with {PointCount} points";
        public LegendItem[] GetLegendItems() => null;
        public int PointCount { get => OHLCs.Length; }
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public Color ColorUp = Color.LightGreen;
        public Color ColorDown = Color.LightCoral;

        public AxisLimits GetAxisLimits()
        {
            // TODO: dont use an array here
            double[] limits = new double[4];
            limits[0] = OHLCs[0].time;
            limits[1] = OHLCs[0].time;
            limits[2] = OHLCs[0].low;
            limits[3] = OHLCs[0].high;

            for (int i = 1; i < OHLCs.Length; i++)
            {
                if (OHLCs[i].time < limits[0])
                    limits[0] = OHLCs[i].time;
                if (OHLCs[i].time > limits[1])
                    limits[1] = OHLCs[i].time;
                if (OHLCs[i].low < limits[2])
                    limits[2] = OHLCs[i].low;
                if (OHLCs[i].high > limits[3])
                    limits[3] = OHLCs[i].high;
            }

            if (Sqeuential)
            {
                limits[0] = 0;
                limits[1] = OHLCs.Length - 1;
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
            for (int i = 1; i < OHLCs.Length; i++)
                smallestSpacing = Math.Min(OHLCs[i].time - OHLCs[i - 1].time, smallestSpacing);
            return smallestSpacing;
        }

        public void ValidateData(bool deepValidation = false)
        {
            if (OHLCs is null)
                throw new InvalidOperationException("ohlcs cannot be null");

            for (int i = 0; i < OHLCs.Length; i++)
            {
                if (OHLCs[i] is null)
                    throw new InvalidOperationException($"ohlcs[{i}] cannot be null");
                if (!OHLCs[i].IsValid)
                    throw new InvalidOperationException($"ohlcs[{i}] does not contain valid data");
            }
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
                for (int i = 0; i < OHLCs.Length; i++)
                {
                    var ohlc = OHLCs[i];
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
                for (int i = 0; i < OHLCs.Length; i++)
                {
                    var ohlc = OHLCs[i];
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
