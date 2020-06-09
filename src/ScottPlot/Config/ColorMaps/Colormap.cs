using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    [Obsolete("This class is experimental and may change in subsequent versions")]
    public abstract class Colormap
    {
        public abstract byte[,] IntenstitiesToRGB(double[] intensities);
        public abstract int[] IntensitiesToARGB(double[] intensities);
        public int RGBToARGB(byte[] rgb)
        {
            return (0xFF << 24) + (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
        }
        public (byte r, byte g, byte b) Lookup(double intensity)
        {
            byte[,] rgb = IntenstitiesToRGB(new double[] { intensity });
            return (rgb[0, 0], rgb[0, 1], rgb[0, 2]);
        }
        public (byte r, byte g, byte b) Lookup(byte value)
        {
            return Lookup(value / 255d);
        }
    }
}
