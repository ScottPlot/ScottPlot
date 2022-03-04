using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public class Label
    {
        public string Text = string.Empty;
        public Color Color = Colors.Black;
        public Font Font = new();
        public Color BackColor = Colors.Transparent;
        public Color BorderColor = Colors.Transparent;
        public float BorderWidth = 0;

        public void Draw(ICanvas canvas, Pixel location, HorizontalAlignment hAlign, VerticalAlignment vAlign, float rotate)
        {

        }
    }
}
