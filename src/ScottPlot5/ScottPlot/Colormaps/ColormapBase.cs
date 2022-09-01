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

        public Color GetColor(double intensity, Range? intensityRange)
        {
            if (double.IsNaN(intensity))
            {
                return Colors.Transparent;
            }

            intensityRange ??= Range.UnitRange;

            double normalized = intensityRange.Value.Normalize(intensity, true);

            return GetColor(normalized);
        }

        public abstract Color GetColor(double normalizedIntensity);
    }
}
