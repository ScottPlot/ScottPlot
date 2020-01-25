using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    // Axes (the plural of Axis) represents an X and Y axis
    public class Axes
    {
        public Axis x = new Axis();
        public Axis y = new Axis();

        public bool hasBeenSet
        {
            get
            {
                return ((x.hasBeenSet) || (y.hasBeenSet));
            }
        }

        public double[] limits
        {
            get
            {
                return new double[] { x.min, x.max, y.min, y.max };
            }
        }

        public override string ToString()
        {
            return $"Axes: X=({x.min}, {x.max}), Y=({x.min}, {x.max})";
        }

        public void Set(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            x.min = x1 ?? x.min;
            x.max = x2 ?? x.max;
            y.min = y1 ?? y.min;
            y.max = y2 ?? y.max;
        }

        public void Set(double[] limits)
        {
            if ((limits == null) || (limits.Length != 4))
                throw new ArgumentException();

            x.min = limits[0];
            x.max = limits[1];
            y.min = limits[2];
            y.max = limits[3];
        }

        public void Expand(double[] limits, bool xExpandOnly = false, bool yExpandOnly = false)
        {
            if ((limits == null) || (limits.Length != 4))
                throw new ArgumentException();

            if (!yExpandOnly)
            {
                x.min = Math.Min(limits[0], x.min);
                x.max = Math.Max(limits[1], x.max);
            }
            if (!xExpandOnly)
            {
                y.min = Math.Min(limits[2], y.min);
                y.max = Math.Max(limits[3], y.max);
            }
        }

        public void Zoom(double xFrac = 1, double yFrac = 1, PointF? zoomCenter = null)
        {
            if (zoomCenter == null)
            {
                x.Zoom(xFrac);
                y.Zoom(yFrac);
            }
            else
            {
                x.Zoom(xFrac, (double)((PointF)zoomCenter).X);
                y.Zoom(yFrac, (double)((PointF)zoomCenter).Y);
            }
        }
    }
}
