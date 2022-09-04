using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public abstract class ArgbColormapBase : ColormapBase
    {
        public abstract uint[] Argbs { get; }
        public override Color GetColor(double normalizedIntensity)
        {
            var argb = Argbs[(int)(normalizedIntensity * (Argbs.Length - 1))];
            return Color.FromARGB(argb);
        }
    }
}
