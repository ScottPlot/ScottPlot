using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public abstract class ByteColormapBase : ColormapBase
    {
        public abstract (byte r, byte g, byte b)[] Rgbs { get; }
        public override Color GetColor(double normalizedIntensity)
        {
            var rgb = Rgbs?[(int)(normalizedIntensity * (Rgbs.Length - 1))] ?? (0, 0, 0);
            return new(rgb.r, rgb.g, rgb.b);
        }
    }
}
