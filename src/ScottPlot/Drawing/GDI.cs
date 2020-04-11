using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Drawing
{
    public static class GDI
    {
        public static System.Drawing.SizeF MeasureString(System.Drawing.Graphics gfx, string text, string fontName, double fontSize, bool bold = false)
        {
            if (gfx is null)
                throw new ArgumentException("a valid Graphics object is required");

            var unit = System.Drawing.GraphicsUnit.Pixel;
            var fontStyle = (bold) ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;
            using (var font = new System.Drawing.Font(fontName, (float)fontSize, fontStyle, unit))
            {
                return (MeasureString(gfx, text, font));
            }
        }

        public static System.Drawing.SizeF MeasureString(System.Drawing.Graphics gfx, string text, System.Drawing.Font font)
        {
            if (gfx is null)
                throw new ArgumentException("a valid Graphics object is required");

            System.Drawing.SizeF stringSize = gfx.MeasureString(text, font);
            return stringSize;
        }
    }
}
