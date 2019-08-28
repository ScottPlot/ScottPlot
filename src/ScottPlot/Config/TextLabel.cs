using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class TextLabel
    {

        public TextLabel()
        {
            // set things which can't be instantiated at the class level
            color = Color.Black;
            colorBackground = Color.Magenta;
            colorBorder = Color.Black;
        }

        public Color color;
        public Color colorBackground;
        public Color colorBorder;

        private Graphics gfx = Graphics.FromHwnd(IntPtr.Zero);

        public string text = "";
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


        public SizeF size
        {
            get
            {
                return gfx.MeasureString(text, font);
            }
        }

        public float width
        {
            get
            {
                return size.Width;
            }
        }

        public float height
        {
            get
            {
                return size.Height;
            }
        }
    }
}
