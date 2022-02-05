using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using System.Linq;

namespace ScottPlot
{
    public class Palette
    {
        readonly List<Color> Colors = new();

        public Color GetColor(int index) => Colors[index % Colors.Count];

        public int Count => Colors.Count;

        public Palette(IEnumerable<Color> colors)
        {
            if (colors is null || colors.Count() == 0)
                throw new ArgumentException($"{nameof(colors)} must contain colors");

            Colors.AddRange(colors);
        }

        public static Palette FromColors(IEnumerable<Color> colors) => new(colors);
    }
}
