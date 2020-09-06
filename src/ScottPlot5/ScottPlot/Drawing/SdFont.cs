using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Drawing
{
    public static class SdFont
    {
        public static Font Monospace(float size, bool bold = false)
        {
            string name = FontFamily.GenericMonospace.Name;
            FontStyle fs = bold ? FontStyle.Bold : FontStyle.Regular;
            return new Font(name, size, fs, GraphicsUnit.Pixel);
        }

        public static Font Sans(float size, bool bold = false)
        {
            string name = FontFamily.GenericSansSerif.Name;
            FontStyle fs = bold ? FontStyle.Bold : FontStyle.Regular;
            return new Font(name, size, fs, GraphicsUnit.Pixel);
        }

        public static Font Serif(float size, bool bold = false)
        {
            string name = FontFamily.GenericSerif.Name;
            FontStyle fs = bold ? FontStyle.Bold : FontStyle.Regular;
            return new Font(name, size, fs, GraphicsUnit.Pixel);
        }
    }
}
