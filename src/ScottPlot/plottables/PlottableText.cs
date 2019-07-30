using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public string text;
        public Brush brush;
        public Font font;

        public PlottableText(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label)
        {
            if (text == null)
                throw new Exception("Text cannot be null");

            this.text = text;
            this.x = x;
            this.y = y;
            this.label = label;
            brush = new SolidBrush(color);
            FontStyle fontStyle = (bold == true) ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontName, (float)fontSize, fontStyle);

            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
        }

        public override double[] GetLimits()
        {
            return new double[] { x, x, y, y };
        }

        public override void Render(Settings settings)
        {
            settings.gfxData.DrawString(text, font, brush, settings.GetPixel(x, y));
        }

        public override void SaveCSV(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
