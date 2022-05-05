using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Finance plots display open/high/low/close (OHLC) data
    /// </summary>
    public class FinancePlot : IPlottable
    {
        public readonly List<OHLC> OHLCs = new();

        /// <summary>
        /// Returns the last element of OHLCs so users can modify FinancePlots in real time.
        /// </summary>
        public OHLC Last() => OHLCs.Last();

        /// <summary>
        /// Display prices as filled candlesticks (otherwise display as OHLC lines)
        /// </summary>
        public bool Candle { get; set; }

        /// <summary>
        /// If True, OHLC timestamps are ignored and candles are placed at consecutive integers and all given a width of 1
        /// </summary>
        public bool Sequential { get; set; }

        /// <summary>
        /// Color of the candle if it closes at or above its open value
        /// </summary>
        public Color ColorUp { get; set; } = Color.LightGreen;

        /// <summary>
        /// Color of the candle if it closes below its open value
        /// </summary>
        public Color ColorDown { get; set; } = Color.LightCoral;

        /// <summary>
        /// This field controls the color of the wick and rectangular candle border.
        /// If null, the wick is the same color as the candle and no border is applied.
        /// </summary>
        public Color? WickColor { get; set; } = null;

        public bool IsVisible { get; set; } = true;
        public override string ToString() => $"FinancePlot with {OHLCs.Count} OHLC indicators";
        public LegendItem[] GetLegendItems() => Array.Empty<LegendItem>();
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Create an empty finance plot. 
        /// Call Add() and AddRange() to add data.
        /// </summary>
        public FinancePlot() { }

        /// <summary>
        /// Create a finance plot from existing OHLC data.
        /// </summary>
        /// <param name="ohlcs"></param>
        public FinancePlot(OHLC[] ohlcs) => AddRange(ohlcs);

        /// <summary>
        /// Add a single candle representing a defined time span
        /// </summary>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeSpan"></param>
        public void Add(double open, double high, double low, double close, DateTime timeStart, TimeSpan timeSpan) =>
            Add(new OHLC(open, high, low, close, timeStart, timeSpan));

        /// <summary>
        /// Add a single candle to the end of the list assuming each candle is spaced 1 horizontal unit apart
        /// </summary>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        public void Add(double open, double high, double low, double close) =>
            Add(new OHLC(open, high, low, close, OHLCs.Count));

        /// <summary>
        /// Add a single OHLC to the plot
        /// </summary>
        /// <param name="ohlc"></param>
        public void Add(OHLC ohlc)
        {
            if (ohlc is null)
                throw new ArgumentNullException();
            OHLCs.Add(ohlc);
        }

        /// <summary>
        /// Add multiple OHLCs to the plot
        /// </summary>
        /// <param name="ohlcs"></param>
        public void AddRange(OHLC[] ohlcs)
        {
            if (ohlcs is null)
                throw new ArgumentNullException();

            foreach (var ohlc in ohlcs)
                if (ohlc is null)
                    throw new ArgumentNullException("no OHLCs may be null");

            OHLCs.AddRange(ohlcs);
        }

        /// <summary>
        /// Clear all OHLCs
        /// </summary>
        public void Clear() => OHLCs.Clear();

        public AxisLimits GetAxisLimits()
        {
            if (OHLCs.Count() == 0)
            {
                return new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            // TODO: dont use an array here
            double[] limits = new double[4];
            limits[0] = OHLCs[0].DateTime.ToOADate();
            limits[1] = OHLCs[0].DateTime.ToOADate();
            limits[2] = OHLCs[0].Low;
            limits[3] = OHLCs[0].High;

            for (int i = 1; i < OHLCs.Count; i++)
            {
                if (OHLCs[i].DateTime.ToOADate() < limits[0])
                    limits[0] = OHLCs[i].DateTime.ToOADate();
                if (OHLCs[i].DateTime.ToOADate() > limits[1])
                    limits[1] = OHLCs[i].DateTime.ToOADate();
                if (OHLCs[i].Low < limits[2])
                    limits[2] = OHLCs[i].Low;
                if (OHLCs[i].High > limits[3])
                    limits[3] = OHLCs[i].High;
            }

            if (Sequential)
            {
                limits[0] = 0;
                limits[1] = OHLCs.Count - 1;
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

        public void ValidateData(bool deepValidation = false)
        {
            if (OHLCs is null)
                throw new InvalidOperationException("ohlcs cannot be null");

            for (int i = 0; i < OHLCs.Count; i++)
            {
                if (OHLCs[i] is null)
                    throw new InvalidOperationException($"ohlcs[{i}] cannot be null");
                if (!OHLCs[i].IsValid)
                    throw new InvalidOperationException($"ohlcs[{i}] does not contain valid data");
            }
        }

        private void RenderCandles(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double fractionalTickWidth = .7;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Pen pen = new Pen(Color.Magenta);
            using SolidBrush brush = new SolidBrush(Color.Magenta);
            for (int i = 0; i < OHLCs.Count; i++)
            {
                var ohlc = OHLCs[i];
                bool closedHigher = ohlc.Close >= ohlc.Open;
                double highestOpenClose = Math.Max(ohlc.Open, ohlc.Close);
                double lowestOpenClose = Math.Min(ohlc.Open, ohlc.Close);

                var ohlcTime = Sequential ? i : ohlc.DateTime.ToOADate();
                var ohlcSpan = Sequential ? 1 : ohlc.TimeSpan.TotalDays;
                float pixelX = dims.GetPixelX(ohlcTime);

                float boxWidth = (float)(ohlcSpan * dims.PxPerUnitX / 2 * fractionalTickWidth);

                Color priceChangeColor = closedHigher ? ColorUp : ColorDown;
                pen.Color = WickColor ?? priceChangeColor;
                pen.Width = (boxWidth >= 2) ? 2 : 1;

                // draw the wick below the box
                PointF wickLowBot = new PointF(pixelX, dims.GetPixelY(ohlc.Low));
                PointF wickLowTop = new PointF(pixelX, dims.GetPixelY(lowestOpenClose));
                gfx.DrawLine(pen, wickLowBot, wickLowTop);

                // draw the wick above the box
                PointF wickHighBot = new PointF(pixelX, dims.GetPixelY(highestOpenClose));
                PointF wickHighTop = new PointF(pixelX, dims.GetPixelY(ohlc.High));
                gfx.DrawLine(pen, wickHighBot, wickHighTop);

                // draw the candle body
                PointF boxLowerLeft = new PointF(pixelX, dims.GetPixelY(lowestOpenClose));
                PointF boxUpperRight = new PointF(pixelX, dims.GetPixelY(highestOpenClose));
                if (ohlc.Open == ohlc.Close)
                {
                    // draw OHLC (non-filled) candle
                    gfx.DrawLine(pen, boxLowerLeft.X - boxWidth, boxLowerLeft.Y, boxLowerLeft.X + boxWidth, boxLowerLeft.Y);
                }
                else
                {
                    // draw a filled candle
                    brush.Color = priceChangeColor;
                    gfx.FillRectangle(
                        brush: brush,
                        x: boxLowerLeft.X - boxWidth,
                        y: boxUpperRight.Y,
                        width: boxWidth * 2,
                        height: boxLowerLeft.Y - boxUpperRight.Y);

                    if (WickColor != null)
                        gfx.DrawRectangle(
                            pen: pen,
                            x: boxLowerLeft.X - boxWidth,
                            y: boxUpperRight.Y,
                            width: boxWidth * 2,
                            height: boxLowerLeft.Y - boxUpperRight.Y);
                }
            }
        }

        private void RenderOhlc(PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double fractionalTickWidth = .7;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Pen pen = new Pen(Color.Magenta);
            for (int i = 0; i < OHLCs.Count; i++)
            {
                var ohlc = OHLCs[i];
                bool closedHigher = ohlc.Close >= ohlc.Open;

                var ohlcTime = (Sequential) ? i : ohlc.DateTime.ToOADate();
                var ohlcSpan = Sequential ? 1 : ohlc.TimeSpan.TotalDays;
                float pixelX = dims.GetPixelX(ohlcTime);

                float boxWidth = (float)(ohlcSpan * dims.PxPerUnitX / 2 * fractionalTickWidth);

                pen.Color = closedHigher ? ColorUp : ColorDown;
                pen.Width = (boxWidth >= 2) ? 2 : 1;

                // the main line
                PointF wickTop = new PointF(pixelX, dims.GetPixelY(ohlc.Low));
                PointF wickBot = new PointF(pixelX, dims.GetPixelY(ohlc.High));
                gfx.DrawLine(pen, wickBot, wickTop);

                // open and close lines
                float xPx = wickTop.X;
                float yPxOpen = dims.GetPixelY(ohlc.Open);
                float yPxClose = dims.GetPixelY(ohlc.Close);
                gfx.DrawLine(pen, xPx - boxWidth, yPxOpen, xPx, yPxOpen);
                gfx.DrawLine(pen, xPx + boxWidth, yPxClose, xPx, yPxClose);
            }
        }

        /// <summary>
        /// Return the simple moving average (SMA) of the OHLC closing prices.
        /// The returned ys are SMA where each point is the average of N points.
        /// The returned xs are times in OATime units.
        /// The returned xs and ys arrays will be the length of the OHLC data minus N.
        /// </summary>
        /// <param name="N">each returned value represents the average of N points</param>
        /// <returns>times and averages of the OHLC closing prices</returns>
        public (double[] xs, double[] ys) GetSMA(int N)
        {
            if (N >= OHLCs.Count)
                throw new ArgumentException("can not analyze more points than are available in the OHLCs");

            double[] xs = OHLCs.Skip(N).Select(x => x.DateTime.ToOADate()).ToArray();
            double[] ys = Statistics.Finance.SMA(OHLCs.ToArray(), N);
            return (xs, ys);
        }

        /// <summary>
        /// Return Bollinger bands (mean +/- 2*SD) for the OHLC closing prices.
        /// The returned xs are times in OATime units.
        /// The returned xs and ys arrays will be the length of the OHLC data minus N (points).
        /// </summary>
        /// <param name="N">each returned value represents the average of N points</param>
        /// <returns>times, averages, and both Bollinger bands for the OHLC closing prices</returns>
        public (double[] xs, double[] sma, double[] lower, double[] upper) GetBollingerBands(int N)
        {
            if (N >= OHLCs.Count)
                throw new ArgumentException("can not analyze more points than are available in the OHLCs");

            double[] xs = OHLCs.Skip(N).Select(x => x.DateTime.ToOADate()).ToArray();
            (var sma, var lower, var upper) = Statistics.Finance.Bollinger(OHLCs.ToArray(), N);
            return (xs, sma, lower, upper);
        }
    }
}
