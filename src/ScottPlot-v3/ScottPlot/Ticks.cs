using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class Tick
    {

        public string label;
        public double value;

        private const int tickSize = 5;

        public Tick(double value)
        {
            this.value = value;
            label = string.Format("{0:0.00}", value);
        }

        public void DrawVertical(Graphics figureGfx, Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[2];
            int xPx = settings.dataOrigin.X;
            int yPx = (int)(unitsFromAxisEdge * settings.yAxisScale);
            yPx = settings.figureSize.Height - yPx - settings.dataPadding[2];

            figureGfx.DrawLine(Pens.Black, xPx, yPx, xPx - tickSize, yPx);
            figureGfx.DrawString(label, settings.tickFont, Brushes.Black, xPx - tickSize, yPx, settings.sfEast);
        }

        public void DrawHorizontal(Graphics figureGfx, Settings settings)
        {
            double unitsFromAxisEdge = value - settings.axis[0];
            int xPx = (int)(unitsFromAxisEdge * settings.xAxisScale) + settings.dataPadding[0];
            int yPx = settings.figureSize.Height - settings.dataPadding[2];

            figureGfx.DrawLine(Pens.Black, xPx, yPx, xPx, yPx + tickSize);
            figureGfx.DrawString(label, settings.tickFont, Brushes.Black, xPx, yPx + tickSize, settings.sfNorth);
        }

    }

    public static class Ticks
    {
        public static void DrawTicks(Graphics figureGfx, Settings settings, bool recalculate = true)
        {
            if (recalculate == true || settings.ticksX == null || settings.ticksY == null)
            {
                settings.ticksX = Ticks.GetTicks(settings.axis[0], settings.axis[1], settings.xAxisScale, 40, settings.dataSize.Width);
                settings.ticksY = Ticks.GetTicks(settings.axis[2], settings.axis[3], settings.yAxisScale, 20, settings.dataSize.Height);
            }

            foreach (Tick tick in settings.ticksX)
                tick.DrawHorizontal(figureGfx, settings);

            foreach (Tick tick in settings.ticksY)
                tick.DrawVertical(figureGfx, settings);

        }

        public static List<Tick> GetTicks(double axisLow, double axisHigh, double axisScale, int tickSpacingPx, int axisSizePx)
        {
            double axisSpan = axisHigh - axisLow;
            double tickCountTarget = (double)axisSizePx / tickSpacingPx / 2;

            // determine an ideal tick spacing (multiple of 2, 5, or 10)
            double tickSpacing = 0;
            for (int tickSpacingPower = 10; tickSpacingPower > -10; tickSpacingPower--)
            {
                tickSpacing = Math.Pow(10, tickSpacingPower);
                if (tickSpacing > axisSpan)
                    continue;
                double tickCount = axisSpan / tickSpacing;
                if (tickCount >= tickCountTarget)
                {
                    if (tickCount >= tickCountTarget * 5)
                        tickSpacing *= 5;
                    else if (tickCount >= tickCountTarget * 2)
                        tickSpacing *= 2;
                    break;
                }
            }

            // create the ticks
            List<Tick> ticks = new List<Tick>();
            if (tickSpacing > 0)
            {
                int yTickCount = (int)(axisSpan / tickSpacing) + 2;
                double tickOffset = axisLow % tickSpacing;
                for (int i = 0; i < yTickCount; i++)
                {
                    double value = tickSpacing * i + axisLow - tickOffset;
                    if (value > axisLow && value < axisHigh)
                        ticks.Add(new Tick(value));
                }
            }
            return ticks;
        }
    }
}
