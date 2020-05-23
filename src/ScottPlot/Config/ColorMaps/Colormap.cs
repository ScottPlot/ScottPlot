using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    public abstract class Colormap
    {
        public abstract byte[,] IntensityToRGB(double[] intensities);
    }
}
