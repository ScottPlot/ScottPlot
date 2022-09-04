using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public abstract class ColormapBase : IColormap
    {
        public abstract string Name { get; }

        public abstract Color GetColor(double position);

        public Color GetColor(double position, Range range)
        {
            if (double.IsNaN(position))
            {
                return Colors.Transparent;
            }

            if (range.Min == range.Max)
            {
                return GetColor(0);
            }

            double normalizedPosition = range.Normalize(position, true);

            return GetColor(normalizedPosition);
        }
    }
}
