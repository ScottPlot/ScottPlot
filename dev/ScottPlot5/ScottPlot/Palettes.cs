using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;

namespace ScottPlot
{
    public static class Palettes
    {
        public static Palette Default => Palette.FromColors(
            new Color[]
            {
                Colors.Red,
                Colors.Orange,
                Colors.Yellow,
                Colors.Green,
                Colors.Blue,
                Colors.Indigo,
                Colors.Violet,
            });
    }
}
