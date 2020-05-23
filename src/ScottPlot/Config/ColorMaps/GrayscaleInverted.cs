using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    public class GrayscaleInverted : Colormap
    {
        public override byte[,] IntensityToRGB(double[] intensities)
        {
            intensities = intensities.Select(i => 1 - i).ToArray();
            return new Grayscale().IntensityToRGB(intensities);
        }
    }
}
