using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public class TextLabel
    {
        public string Text = string.Empty;

        public float FontSize = 12;
        public string FontName = "Consolas";
        public float FontWeight = 400;
        public Color FontColor = Colors.Black;

        public Color BackgroundColor = Colors.Transparent;

        public float OutlineWidth = 0;
        public Color OutlineColor = Colors.Black;

        public PixelSize Measure(ICanvas canvas)
        {
            float width = Text.Length * 10;
            float height = FontSize * 1.25f;
            return new PixelSize(width, height);
        }
    }
}
