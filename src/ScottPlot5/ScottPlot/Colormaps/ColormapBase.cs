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

            double normalized = range.Normalize(position, true);

            return GetColor(normalized);
        }
    }
}
