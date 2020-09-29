using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderer
{
    public class Font
    {
        public string Name { get; set; }
        public float Size { get; set; }

        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlignment = VerticalAlignment.Bottom;

        public Font(string fontName, float fontSize, 
            HorizontalAlignment hAlign = HorizontalAlignment.Left, 
            VerticalAlignment vAlign = VerticalAlignment.Top)
        {
            Name = fontName;
            Size = fontSize;
            HorizontalAlignment = hAlign;
            VerticalAlignment = vAlign;
        }
    }
}
