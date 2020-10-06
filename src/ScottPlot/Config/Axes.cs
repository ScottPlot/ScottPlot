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
        public bool equalAxes = false;
        public Axis x = new Axis();
        public Axis y = new Axis();
        public AxisLimits2D Limits { get => new AxisLimits2D(x.min, x.max, y.min, y.max); }

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

        public double[] ToArray()
        {
            return new double[] { x.min, x.max, y.min, y.max };
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

            Set(limits[0], limits[1], limits[2], limits[3]);
        }

        public void Set(AxisLimits2D limits)
        {
            limits.MakeRational();
            Set(limits.x1, limits.x2, limits.y1, limits.y2);
        }

        public void Set(ValueTuple<double, double, double, double> limits)
        {
            Set(limits.Item1, limits.Item2, limits.Item3, limits.Item4);
        }

        public void Zoom(double xFrac = 1, double yFrac = 1, double? centerX = null, double? centerY = null)
        {
            x.Zoom(xFrac, centerX);
            y.Zoom(yFrac, centerY);
        }

        public void Reset()
        {
            x.min = 0;
            x.max = 0;
            x.hasBeenSet = false;

            y.min = 0;
            y.max = 0;
            y.hasBeenSet = false;
        }

        public void ApplyBounds()
        {
            x.ApplyBounds();
            y.ApplyBounds();
        }
    }
}
