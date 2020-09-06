﻿using System;
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

        public static System.Drawing.Pen Pen(System.Drawing.Color color, double width = 1, LineStyle lineStyle = LineStyle.Solid, bool rounded = false)
        {
            var pen = new System.Drawing.Pen(color, (float)width);

            if (lineStyle == LineStyle.Solid)
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
    }
}
