using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    public abstract class Colormap
    {
        public abstract byte[,] IntenstitiesToRGB(double[] intensities);
        public abstract int[] IntensitiesToARGB(double[] intensities);
        public int RGBToARGB(byte[] rgb)
        {
            return (0xFF << 24) + (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
        }
    }
}
