using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot.Plottables
{
    public class Finance : IPlottable
    {
        // interface stuff
        public bool visible { get; set; } = true;
        public int pointCount { get { return ohlcs.Length; } }
        public string label { get; set; }
        public Color color { get; set; }
        public MarkerShape markerShape { get; set; }
        public LineStyle lineStyle { get; set; }

        // properties
        public OHLC[] ohlcs;
        bool displayCandles;
        Pen penUp;
        Pen penDown;
        Brush brushUp;
        Brush brushDown;

        public Finance(OHLC[] ohlcs, bool displayCandles = true)
        {
            this.ohlcs = ohlcs;
            this.displayCandles = displayCandles;

            Color colorUp = Color.DarkGreen;
            Color colorDown = Color.Red;

            penUp = new Pen(colorUp);
            penDown = new Pen(colorDown);
            brushUp = new SolidBrush(colorUp);
            brushDown = new SolidBrush(colorDown);
        }

        public override string ToString()
        {
            return $"PlottableOHLC with {pointCount} points";
        }

        public Config.AxisLimits2D GetLimits()
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

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        public void Render(DataArea dataArea)
        {
            if (displayCandles) // TODO: make enum
                RenderCandles(dataArea);
            else
                RenderOhlc(dataArea);
        }

        private double GetSmallestSpacing()
        {
            double smallestSpacing = double.PositiveInfinity;
            for (int i = 1; i < ohlcs.Length; i++)
                smallestSpacing = Math.Min(ohlcs[i].time - ohlcs[i - 1].time, smallestSpacing);
            return smallestSpacing;
        }

        public void RenderCandles(DataArea dataArea)
        {
            double fractionalTickWidth = .7;
            double spacingPx = GetSmallestSpacing() * dataArea.pxPerUnit.x;
            float boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);

            foreach (OHLC ohlc in ohlcs)
            {
                Pen pen = (ohlc.closedHigher) ? penUp : penDown;
                Brush brush = (ohlc.closedHigher) ? brushUp : brushDown;
                pen.Width = 2;

                // the wick below the box
                PointF wickLowBot = dataArea.GetPixel(ohlc.time, ohlc.low);
                PointF wickLowTop = dataArea.GetPixel(ohlc.time, ohlc.lowestOpenClose);
                dataArea.gfxData.DrawLine(pen, wickLowBot, wickLowTop);

                // the wick above the box
                PointF wickHighBot = dataArea.GetPixel(ohlc.time, ohlc.highestOpenClose);
                PointF wickHighTop = dataArea.GetPixel(ohlc.time, ohlc.high);
                dataArea.gfxData.DrawLine(pen, wickHighBot, wickHighTop);

                // the candle
                PointF boxLowerLeft = dataArea.GetPixel(ohlc.time, ohlc.lowestOpenClose);
                PointF boxUpperRight = dataArea.GetPixel(ohlc.time, ohlc.highestOpenClose);
                dataArea.gfxData.FillRectangle(brush, boxLowerLeft.X - boxWidth, boxUpperRight.Y, boxWidth * 2, boxLowerLeft.Y - boxUpperRight.Y);
            }
        }

        public void RenderOhlc(DataArea dataArea)
        {
            double fractionalTickWidth = 1;
            double spacingPx = GetSmallestSpacing() * dataArea.pxPerUnit.x;
            float boxWidth = (float)(spacingPx / 2 * fractionalTickWidth);

            foreach (OHLC ohlc in ohlcs)
            {
                Pen pen = (ohlc.closedHigher) ? penUp : penDown;
                pen.Width = 2;

                // the main line
                PointF wickTop = dataArea.GetPixel(ohlc.time, ohlc.low);
                PointF wickBot = dataArea.GetPixel(ohlc.time, ohlc.high);
                dataArea.gfxData.DrawLine(pen, wickBot, wickTop);

                // open and close lines
                float xPx = wickTop.X;
                float yPxOpen = (float)dataArea.GetPixel(0, ohlc.open).Y;
                float yPxClose = (float)dataArea.GetPixel(0, ohlc.close).Y;
                dataArea.gfxData.DrawLine(pen, xPx - boxWidth, yPxOpen, xPx, yPxOpen);
                dataArea.gfxData.DrawLine(pen, xPx + boxWidth, yPxClose, xPx, yPxClose);
            }
        }
    }
}
