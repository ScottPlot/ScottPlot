using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot.Drawing
{
    public static class GDI
    {
        private const float xMultiplierLinux = 1;
        private const float yMultiplierLinux = 27.16f / 22;

        private const float xMultiplierMacOS = 82.82f / 72;
        private const float yMultiplierMacOS = 27.16f / 20;

        public static System.Drawing.SizeF MeasureString(System.Drawing.Graphics gfx, string text, string fontName, double fontSize, bool bold = false)
        {
            if (gfx is null)
                throw new ArgumentException("a valid Graphics object is required");

            var unit = System.Drawing.GraphicsUnit.Pixel;
            var fontStyle = (bold) ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;
            using (var font = new System.Drawing.Font(fontName, (float)fontSize, fontStyle, unit))
            {
                System.Drawing.SizeF size = gfx.MeasureString(text, font);

                // compensate for OS-specific differences in font scaling
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    size.Width *= xMultiplierLinux;
                    size.Height *= yMultiplierLinux;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    size.Width *= xMultiplierMacOS;
                    size.Height *= yMultiplierMacOS;
                }

                return size;
            }
        }

        public static System.Drawing.SizeF MeasureString(System.Drawing.Graphics gfx, string text, System.Drawing.Font font)
        {
            return MeasureString(gfx, text, font.Name, font.Size, font.Style == System.Drawing.FontStyle.Bold);
        }
    }
}
