using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class GrayscaleInverted : Colormap
    {
        public override int[] IntensitiesToARGB(double[] intensities)
        {
            Grayscale grayscale = new Grayscale();
            return grayscale.IntensitiesToARGB(intensities.AsParallel().AsOrdered().Select(i => 1 - i).ToArray());
        }

        public override byte[,] IntenstitiesToRGB(double[] intensities)
        {
            intensities = intensities.Select(i => 1 - i).ToArray();
            return new Grayscale().IntenstitiesToRGB(intensities);
        }
    }
}
