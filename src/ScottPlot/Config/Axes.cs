using System;
using System.Collections.Generic;
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

        public bool hasBeenSet = false;

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
    }
}
