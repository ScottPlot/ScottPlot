using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

        public static System.Drawing.Color Mix(System.Drawing.Color colorA, System.Drawing.Color colorB, double fracA)
        {
            byte r = (byte)((colorA.R * (1 - fracA)) + colorB.R * fracA);
            byte g = (byte)((colorA.G * (1 - fracA)) + colorB.G * fracA);
            byte b = (byte)((colorA.B * (1 - fracA)) + colorB.B * fracA);
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public static System.Drawing.Color Mix(string hexA, string hexB, double fracA)
        {
            var colorA = System.Drawing.ColorTranslator.FromHtml(hexA);
            var colorB = System.Drawing.ColorTranslator.FromHtml(hexB);
            return Mix(colorA, colorB, fracA);
        }

        public static System.Drawing.Graphics Graphics(Bitmap bmp, bool lowQuality = false)
        {
            Graphics gfx = System.Drawing.Graphics.FromImage(bmp);
            gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
            gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;
            return gfx;
        }

        public static System.Drawing.Pen Pen(System.Drawing.Color color, double width = 1, LineStyle lineStyle = LineStyle.Solid, bool rounded = false)
        {
            var pen = new System.Drawing.Pen(color, (float)width);

            if (lineStyle == LineStyle.Solid || lineStyle == LineStyle.None)
            {
                /* WARNING: Do NOT apply a solid DashPattern!
                 * Setting DashPattern automatically sets a pen's DashStyle to custom.
                 * Custom DashStyles are slower and can cause diagonal rendering artifacts.
                 * Instead use the solid DashStyle.
                 * https://github.com/swharden/ScottPlot/issues/327
                 * https://github.com/swharden/ScottPlot/issues/401
                 */
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            }
            else if (lineStyle == LineStyle.Dash)
                pen.DashPattern = new float[] { 8.0F, 4.0F };
            else if (lineStyle == LineStyle.DashDot)
                pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F };
            else if (lineStyle == LineStyle.DashDotDot)
                pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F };
            else if (lineStyle == LineStyle.Dot)
                pen.DashPattern = new float[] { 2.0F, 4.0F };
            else
                throw new NotImplementedException("line style not supported");

            if (rounded)
            {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
            }

            return pen;
        }

        public static Brush Brush(Color color, double alpha) => new SolidBrush(Color.FromArgb((byte)(255 * alpha), color));

        public static Brush Brush(Color color, Color? hatchColor = null, HatchStyle hatchStyle = HatchStyle.None)
        {
            bool isHatched = hatchStyle != HatchStyle.None;

            if (isHatched)
            {
                if (hatchColor is null)
                    throw new ArgumentException("hatch color must be defined if hatch style is used");
                else
                    return new HatchBrush(ConvertToSDHatchStyle(hatchStyle).Value, hatchColor.Value, color);
            }
            else
            {
                return new SolidBrush(color);
            }
        }

        [Obsolete("use Brush()", true)]
        public static System.Drawing.Brush HatchBrush(HatchStyle pattern, Color fillColor, Color hatchColor)
        {
            if (pattern == HatchStyle.None)
                return new SolidBrush(fillColor);
            else
                return new HatchBrush(ConvertToSDHatchStyle(pattern).Value, hatchColor, fillColor);
        }

        public static System.Drawing.Drawing2D.HatchStyle? ConvertToSDHatchStyle(Drawing.HatchStyle pattern)
        {
            switch (pattern)
            {
                case HatchStyle.StripedUpwardDiagonal:
                    return System.Drawing.Drawing2D.HatchStyle.LightUpwardDiagonal;
                case HatchStyle.StripedDownwardDiagonal:
                    return System.Drawing.Drawing2D.HatchStyle.LightDownwardDiagonal;
                case HatchStyle.StripedWideUpwardDiagonal:
                    return System.Drawing.Drawing2D.HatchStyle.WideUpwardDiagonal;
                case HatchStyle.StripedWideDownwardDiagonal:
                    return System.Drawing.Drawing2D.HatchStyle.WideDownwardDiagonal;
                case HatchStyle.LargeCheckerBoard:
                    return System.Drawing.Drawing2D.HatchStyle.LargeCheckerBoard;
                case HatchStyle.SmallCheckerBoard:
                    return System.Drawing.Drawing2D.HatchStyle.SmallCheckerBoard;
                case HatchStyle.LargeGrid:
                    return System.Drawing.Drawing2D.HatchStyle.LargeGrid;
                case HatchStyle.SmallGrid:
                    return System.Drawing.Drawing2D.HatchStyle.SmallGrid;
                case HatchStyle.DottedDiamond:
                    return System.Drawing.Drawing2D.HatchStyle.DottedDiamond;

                case HatchStyle.None:
                default:
                    return null;

            }
        }

        public static System.Drawing.Font Font(string fontName = null, float fontSize = 12, bool bold = false)
        {
            string validFontName = Config.Fonts.GetValidFontName(fontName);
            FontStyle fontStyle = bold ? FontStyle.Bold : FontStyle.Regular;
            return new Font(validFontName, fontSize, fontStyle, GraphicsUnit.Pixel);
        }
    }
}
