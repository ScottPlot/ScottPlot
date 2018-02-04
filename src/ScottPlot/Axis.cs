using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{

    public class Axis
    {
        // user provides these
        public double min { get; set; }
        public double max { get; set; }
        public int pxSize { get; private set; }
        public bool pxInverted { get; set; }

        // these are calculated
        public double unitsPerPx { get; private set; }
        public double pxPerUnit { get; private set; }
        public double span { get { return max - min; } }
        public double center { get { return (max + min)/2.0; } }

        // these relate to ticks
        public double[] TickValues { get; private set; }
        public string[] TickLabels { get; private set; }

        /// <summary>
        /// Single-dimensional axis (i.e., x-axis)
        /// </summary>
        /// <param name="Min">lower bound (units)</param>
        /// <param name="Max">upper bound (units)</param>
        /// <param name="pxSize">size of this axis (pixels)</param>
        /// <param name="pxInverted">inverted axis vs. pixel position (common for Y-axis)</param>
        public Axis(double Min, double Max, int pxSize = 500, bool pxInverted = false)
        {
            this.min = Min;
            this.max = Max;
            this.pxInverted = pxInverted;
            ResizePx(pxSize);
        }

        /// <summary>
        /// Tell the Axis how large it will be on the screen
        /// </summary>
        /// <param name="sizePx">size of this axis (pixels)</param>
        public void ResizePx(int sizePx)
        {
            this.pxSize = sizePx;
            RecalculateScale();
        }

        /// <summary>
        /// Update units/pixels conversion scales.
        /// </summary>
        public void RecalculateScale()
        {
            this.pxPerUnit = (double)pxSize / (max - min);
            this.unitsPerPx = (max - min) / (double)pxSize;
            RecalculateTicks();
        }

        /// <summary>
        /// Shift the Axis by a specified amount
        /// </summary>
        /// <param name="Shift">distance (units)</param>
        public void Pan(double Shift)
        {
            min += Shift;
            max += Shift;
            RecalculateScale();
        }

        /// <summary>
        /// Zoom in on the center of Axis by a fraction. 
        /// A fraction of 2 means that the new width will be 1/2 as wide as the old width.
        /// A fraction of 0.1 means the new width will show 10 times more axis length.
        /// </summary>
        /// <param name="zoomFrac">Fractional amount to zoom</param>
        public void Zoom(double zoomFrac)
        {
            double newSpan = span / zoomFrac;
            double newCenter = center;
            min = newCenter - newSpan / 2;
            max = newCenter + newSpan / 2;
            RecalculateScale();
        }

        /// <summary>
        /// Given a position on the axis (in units), return its position on the screen (in pixels).
        /// Returned values may be negative, or greater than the pixel width.
        /// </summary>
        /// <param name="unit">position (units)</param>
        /// <returns></returns>
        public int UnitToPx(double unit)
        {
            int px = (int)((unit - min) * pxPerUnit);
            if (pxInverted) px = pxSize - px;
            return px;
        }

        /// <summary>
        /// Given a position on the screen (in pixels), return its location on the axis (in units).
        /// </summary>
        /// <param name="px">position (pixels)</param>
        /// <returns></returns>
        public double PxToUnit(int px)
        {
            if (pxInverted) px = pxSize - px;
            return min + (double)px * unitsPerPx;
        }
        
        /// <summary>
        /// Given an arbitrary number, return the nearerest round number
        /// (i.e., 1000, 500, 100, 50, 10, 5, 1, .5, .1, .05, .01)
        /// </summary>
        /// <param name="target">the number to approximate</param>
        /// <returns></returns>
        private double RoundNumberNear(double target)
        {
            target = Math.Abs(target);
            int lastDivision = 2;
            double round = 1000000000000;
            while (round > 0.00000000001)
            {
                if (round <= target) return round;
                round /= lastDivision;
                if (lastDivision == 2) lastDivision = 5;
                else lastDivision = 2;
            }
            return 0;
        }

        /// <summary>
        /// Return an array of tick objects given a custom target tick count
        /// </summary>
        public Tick[] CustomTicks(int targetTickCount)
        {
            List<Tick> ticks = new List<Tick>();

            if (targetTickCount > 0)
            {
                double tickSize = RoundNumberNear(((max - min) / targetTickCount) * 1.5);
                int lastTick = 123456789;
                for (int i = 0; i < pxSize; i++)
                {
                    double thisPosition = i * unitsPerPx + min;
                    int thisTick = (int)(thisPosition / tickSize);
                    if (thisTick != lastTick)
                    {
                        lastTick = thisTick;
                        double thisPositionRounded = (double)((int)(thisPosition / tickSize) * tickSize);
                        if (thisPositionRounded > min && thisPositionRounded < max)
                        {
                            ticks.Add(new Tick(thisPositionRounded, UnitToPx(thisPositionRounded), max - min));
                        }
                    }
                }
            }
            return ticks.ToArray();
        }
        
        /// <summary>
        /// Pre-prepare recommended major and minor ticks
        /// </summary>
        public Tick[] minorTicks;
        public Tick[] majorTicks;
        private void RecalculateTicks()
        {
            double tick_density_x = pxSize / 70; // approx. 1 tick per this many pixels
            minorTicks = CustomTicks((int)(tick_density_x * 5)); // relative density of minor to major ticks
            majorTicks = CustomTicks((int)(tick_density_x * 1));
        }

    }

    /// <summary>
    /// The Tick object stores details about a single tick and can generate relevant labels.
    /// </summary>
    public class Tick
    {
        public double value { get; set; }
        public int pixel { get; set; }
        public double axisSpan { get; set; }

        public Tick(double value, int pixel, double axisSpan)
        {
            this.value = value;
            this.pixel = pixel;
            this.axisSpan = axisSpan;
        }

        public string label
        {
            get
            {
                if (axisSpan < .01) return string.Format("{0:0.0000}", value);
                if (axisSpan < .1) return string.Format("{0:0.000}", value);
                if (axisSpan < 1) return string.Format("{0:0.00}", value);
                if (axisSpan < 10) return string.Format("{0:0.0}", value);
                return string.Format("{0:0}", value);
            }
        }
    }

}
