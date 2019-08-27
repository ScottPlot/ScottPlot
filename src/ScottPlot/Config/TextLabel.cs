using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public abstract class TextLabel
    {
        public string text = "?";
        public bool visible = true;

        public float fontSize = 12;
        public bool bold = false;

        private string _fontName = "Segoe UI";
        public string fontName
        {
            get
            {
                return _fontName;
            }
            set
            {
                foreach (FontFamily font in FontFamily.Families)
                {
                    if (fontName.ToUpper() == font.Name.ToUpper())
                    {
                        _fontName = value;
                        return;
                    }
                }
                throw new Exception($"Font not found: {fontName}");
            }
        }

        public Font font
        {
            get
            {
                FontFamily family = new FontFamily(fontName);
                FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
                Font font = new Font(family, fontSize, fontStyle, GraphicsUnit.Pixel);
                return font;
            }
        }

        private Color _color;
        public Color color
        {
            get
            {
                if (_color == null)
                    return Color.Black;
                else
                    return _color;
            }
            set
            {
                _color = value;
            }
        }

        public SizeF GetDimensions(Graphics gfx)
        {
            return gfx.MeasureString(text, font);
        }
    }
}
