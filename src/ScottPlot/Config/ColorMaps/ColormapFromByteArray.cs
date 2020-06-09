using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
#pragma warning disable CS0618 // Type or member is obsolete
    public abstract class ColormapFromByteArray : Colormap
    {
        public override byte[,] IntenstitiesToRGB(double[] intensities)
        {
            byte[,] output = new byte[intensities.Length, 3];
            for (int i = 0; i < intensities.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    output[i, j] = cmap[(int)(intensities[i] * 255), j];
                }
            }
            return output;
        }

        public override int[] IntensitiesToARGB(double[] intensities)
        {
            return intensities.AsParallel().AsOrdered().Select(i => RGBToARGB(new byte[] { cmap[(int)(i * 255), 0], cmap[(int)(i * 255), 1], cmap[(int)(i * 255), 2] })).ToArray();
        }

        protected abstract byte[,] cmap { get; }
    }
}
