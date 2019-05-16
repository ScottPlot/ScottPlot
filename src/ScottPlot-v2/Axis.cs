using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* code in this file relates to management of axes and pixel/unit conversions */

namespace ScottPlot
{
    public class Tick
    {
        public double value;
        public string label;
        public int pixel;
        public override string ToString()
        {
            return $"tick at {value} ({pixel} px) labeled \"{label}\"";
        }
    }

    public class Axis
    {
        public double x1 { get; private set; }
        public double x2 { get; private set; }
        public int px { get; private set; }
        public int tickSpacingPx { get; private set; }
        public Tick[] ticksMajor;
        public Tick[] ticksMinor;

        public double span { get; private set; }
        public double center { get; private set; }

        public double unitsPerPx { get; private set; }
        public double pxPerUnit { get; private set; }

        public Axis(int px = 100, double x1 = -10, double x2 = 10, int tickSpacingPx = 100)
        {
            this.px = px;
            this.x1 = x1;
            this.x2 = x2;
            this.tickSpacingPx = tickSpacingPx;
            RecalculateScale();
        }

        public Axis(Axis axis)
        {
            px = axis.px;
            x1 = axis.x1;
            x2 = axis.x2;
            tickSpacingPx = axis.tickSpacingPx;
            RecalculateScale();
        }

        public override string ToString()
        {
            return $"ScottPlot.Axis [{x1} - {x2}] over {px} px";
        }

        public void Set(double? x1, double? x2)
        {
            this.x1 = x1 ?? this.x1;
            this.x2 = x2 ?? this.x2;
            if (x1 == x2)
                throw new Exception("axis min and max must be different");
            if (x1 > x2)
                throw new Exception("axis limits cannot invert");
            RecalculateScale();
        }

        public void Resize(int px)
        {
            this.px = px;
            RecalculateScale();
        }

        /// <summary>
        /// Zoom in on the center of Axis by a fraction (2 zooms in, 0.5 zooms out)
        /// </summary>
        public void Zoom(double zoomFrac)
        {
            double newSpan = span / zoomFrac;
            x1 = center - newSpan / 2;
            x2 = center + newSpan / 2;
            RecalculateScale();
        }

        public void ZoomPx(int px)
        {
            double dX = px * unitsPerPx;
            double dXFrac = dX / (Math.Abs(dX) + span);
            Zoom(Math.Pow(10, dXFrac));
        }

        public void Pan(double delta)
        {
            x1 += delta;
            x2 += delta;
            RecalculateScale();
        }

        public void PanPx(int px)
        {
            Pan(px * unitsPerPx);
        }

        // call after changing width (px) or axis (x1 or x2)
        private void RecalculateScale()
        {
            span = x2 - x1;
            center = (x1 + x2) / 2.0;
            unitsPerPx = span / px;
            pxPerUnit = px / span;
            ticksMajor = GetTicks(true);
            ticksMinor = GetTicks(false);
        }

        public int UnitToPixel(double value)
        {
            return (int)((value - x1) * pxPerUnit);
        }

        private double GetIdealTickSpacing(int tickCountTarget)
        {
            double tickSpacing;
            for (int powerOfTen = 10; powerOfTen > -10; powerOfTen--)
            {
                tickSpacing = Math.Pow(10, powerOfTen);
                if (tickSpacing > span)
                    continue;
                double tickCount = span / tickSpacing;
                if (tickCount >= tickCountTarget)
                {
                    // we are now within one order of magnitude of a good tick density
                    if (tickCount >= tickCountTarget * 5)
                        return tickSpacing * 5;
                    if (tickCount >= tickCountTarget * 2)
                        return tickSpacing * 2;
                    return tickSpacing;
                }
            }
            return 0;
        }

        private Tick[] GetTicks(bool major = true)
        {
            var ticks = new List<Tick>();

            // if the data window is very small, don't make ticks
            if (px < tickSpacingPx / 2)
                return ticks.ToArray();

            // determine the ideal tick spacing and number
            int minimumTickCount = px / tickSpacingPx;
            if (major==false)
                 minimumTickCount *= 5;
            double tickSpacing = GetIdealTickSpacing(minimumTickCount);
            int tickCount = (int)(span / tickSpacing) + 1;

            double tickOffsetFromX1 = x1 % tickSpacing;

            // determine the value, pixel position, and label for each tick
            for (int i = 0; i < tickCount + 1; i++)
            {
                var tick = new Tick();
                double tickDelta = i * tickSpacing - tickOffsetFromX1;
                tick.value = x1 + tickDelta;
                if (tick.value < x1)
                    continue;
                if (tick.value > x2)
                    break;
                if (tickSpacing < 1)
                {
                    tick.label = string.Format("{0:0.00}", tick.value);
                } else
                {
                    tick.label = string.Format("{0:0}", tick.value);
                }
                
                tick.pixel = (int)(tickDelta * pxPerUnit);
                ticks.Add(tick);
            }

            return ticks.ToArray();
        }
    }
}
