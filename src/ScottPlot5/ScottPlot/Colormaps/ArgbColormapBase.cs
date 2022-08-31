using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public abstract class ArgbColormapBase : IColormap
    {
        public abstract string Name { get; }
        public abstract uint[] Argbs { get; }
        public Color GetColor(double intensity, Range? domain)
        {
            if (double.IsNaN(intensity))
            {
                return Colors.Transparent;
            }

            domain ??= Range.UnitRange;

            double normalized = domain.Value.NormalizeAndClampToUnitRange(intensity);

            var argb = Argbs[(int)(normalized * (Argbs.Length - 1))];
            return Color.FromARGB(argb);
        }
    }
}
