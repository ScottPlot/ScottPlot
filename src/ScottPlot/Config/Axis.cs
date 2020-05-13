using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Axis
    {
        public bool hasBeenSet = false;
        public double boundMin = double.NegativeInfinity;
        public double boundMax = double.PositiveInfinity;

        private double _min;
        public double min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                hasBeenSet = true;
            }
        }

        private double _max;
        public double max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                hasBeenSet = true;
            }
        }

        public double span
        {
            get
            {
                return max - min;
            }
        }

        public double center
        {
            get
            {
                return (max + min) / 2;
            }
        }

        public void Pan(double delta)
        {
            if (delta == 0)
                return;

            if ((delta < 0) && (min + delta < boundMin))
            {
                var originalSpan = span;
                min = boundMin;
                max = min + originalSpan;
                return;
            }

            if ((delta > 0) && (max + delta > boundMax))
            {
                var originalSpan = span;
                max = boundMax;
                min = max - originalSpan;
                return;
            }

            min += delta;
            max += delta;
        }

        public void Zoom(double frac = 1, double? zoomTo = null)
        {
            zoomTo = zoomTo ?? center;
            double spanLeft = (double)zoomTo - min;
            double spanRight = max - (double)zoomTo;
            min = (double)zoomTo - spanLeft / frac;
            max = (double)zoomTo + spanRight / frac;
            ApplyBounds();
        }

        public override string ToString()
        {
            return $"axis [{min} to {max}]";
        }

        public void ApplyBounds()
        {
            min = Math.Max(min, boundMin);
            max = Math.Min(max, boundMax);
        }
    }
}
