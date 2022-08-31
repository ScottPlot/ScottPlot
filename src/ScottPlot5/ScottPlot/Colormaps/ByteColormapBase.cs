using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public abstract class ByteColormapBase : IColormap
    {
        public abstract string Name { get; }
        public abstract (byte r, byte g, byte b)[] Rgbs { get; }
        public Color GetColor(double intensity, Range? domain)
        {
            if (double.IsNaN(intensity))
            {
                return Colors.Transparent;
            }

            domain ??= Range.UnitRange;

            double normalized = domain.Value.NormalizeAndClampToUnitRange(intensity);

            var rgb = Rgbs?[(int)(normalized * (Rgbs.Length - 1))] ?? (0, 0, 0);
            return new(rgb.r, rgb.g, rgb.b);
        }
    }
}
