using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableOHLC : Plottable
    {
        OHLC[] ohlcs;
        bool displayCandles;
        Pen penUp;
        Pen penDown;
        Brush brushUp;
        Brush brushDown;

        public PlottableOHLC(OHLC[] ohlcs, bool displayCandles = true)
        {
            this.ohlcs = ohlcs;
            pointCount = ohlcs.Length;
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

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = ohlcs[0].epochSeconds;
            limits[1] = ohlcs[0].epochSeconds;
            limits[2] = ohlcs[0].low;
            limits[3] = ohlcs[0].high;

            for (int i = 1; i < ohlcs.Length; i++)
            {
                if (ohlcs[i].epochSeconds < limits[0])
                    limits[0] = ohlcs[i].epochSeconds;
                if (ohlcs[i].epochSeconds > limits[1])
                    limits[1] = ohlcs[i].epochSeconds;
                if (ohlcs[i].low < limits[2])
                    limits[2] = ohlcs[i].low;
                if (ohlcs[i].high > limits[3])
                    limits[3] = ohlcs[i].high;
            }

            return limits;
        }

        public override void Render(Settings settings)
        {
            RenderCandles(settings);
        }

        public void RenderCandles(Settings settings)
        {
            foreach (OHLC ohlc in ohlcs)
            {
                Pen pen = (ohlc.closedHigher) ? penUp : penDown;
                Brush brush = (ohlc.closedHigher) ? brushUp : brushDown;

                // the wick below the box
                Point wickLowBot = settings.GetPixel(ohlc.epochSeconds, ohlc.low);
                Point wickLowTop = settings.GetPixel(ohlc.epochSeconds, ohlc.lowestOpenClose);
                settings.gfxData.DrawLine(pen, wickLowBot, wickLowTop);

                // the wick above the box
                Point wickHighBot = settings.GetPixel(ohlc.epochSeconds, ohlc.highestOpenClose);
                Point wickHighTop = settings.GetPixel(ohlc.epochSeconds, ohlc.high);
                settings.gfxData.DrawLine(pen, wickHighBot, wickHighTop);

                // the candle
                Point boxLowerLeft = settings.GetPixel(ohlc.epochSeconds, ohlc.lowestOpenClose);
                Point boxUpperRight = settings.GetPixel(ohlc.epochSeconds, ohlc.highestOpenClose);
                float boxWidth = 2;
                settings.gfxData.FillRectangle(brush, boxLowerLeft.X - boxWidth, boxUpperRight.Y, boxWidth * 2, boxLowerLeft.Y - boxUpperRight.Y);
            }
        }

        public void RenderOhlc(Settings settings)
        {
            throw new NotImplementedException("Only candles supported at this time.");
        }
    }
}
