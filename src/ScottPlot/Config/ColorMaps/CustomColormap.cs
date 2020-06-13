using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config.ColorMaps
{
    public class CustomColormap : ColormapFromByteArray
    {
        public CustomColormap(byte[,] cmap)
        {
            cmaplocal = cmap;
        }

        private byte[,] cmaplocal;
        protected override byte[,] cmap { get { return cmaplocal; } }
    }
}
