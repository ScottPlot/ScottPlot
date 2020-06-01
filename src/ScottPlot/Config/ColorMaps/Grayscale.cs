using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    public class Grayscale : Colormap
    {
        public override int[] IntensitiesToARGB(double[] intensities)
        {
            return intensities.AsParallel().AsOrdered().Select(i => RGBToARGB(new byte[] { (byte)(255 * i), (byte)(255 * i), (byte)(255 * i) })).ToArray();
        }

        public override byte[,] IntenstitiesToRGB(double[] intensities)
        {
            byte[,] outputGrayscale = new byte[intensities.Length, 3];
            for (int i = 0; i < intensities.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    outputGrayscale[i, j] = (byte)(intensities[i] * 255);
                }
            }
            return outputGrayscale;
        }
    }
}
