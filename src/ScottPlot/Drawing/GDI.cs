﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

        /// <summary>
        /// Return the display scale ratio being used.
        /// A scaling ratio of 1.0 means scaling is not active.
        /// </summary>
        public static float GetScaleRatio()
        {
            const int DEFAULT_DPI = 96;
            using Graphics gfx = GDI.Graphics(new Bitmap(1, 1));
            return gfx.DpiX / DEFAULT_DPI;
        }

        public static SizeF MeasureString(string text, Font font)
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics gfx = Graphics(bmp, lowQuality: true))
            {
                return MeasureString(gfx, text, font.Name, font.Size, font.Bold);
            }
        }

        public static SizeF MeasureString(Graphics gfx, string text, string fontName, double fontSize, bool bold = false)
        {
            var fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
            using (var font = new System.Drawing.Font(fontName, (float)fontSize, fontStyle, GraphicsUnit.Pixel))
            {
                return MeasureString(gfx, text, font);
            }
        }

        public static SizeF MeasureString(Graphics gfx, string text, System.Drawing.Font font)
        {
            SizeF size = gfx.MeasureString(text, font);

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

            // ensure the measured height is at least the font size
            size.Height = Math.Max(font.Size, size.Height);

            return size;
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

        public static System.Drawing.Graphics Graphics(Bitmap bmp, bool lowQuality = false, double scale = 1.0)
        {
            Graphics gfx = System.Drawing.Graphics.FromImage(bmp);
            gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
            gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;
            gfx.ScaleTransform((float)scale, (float)scale);
            return gfx;
        }

        public static System.Drawing.Graphics Graphics(Bitmap bmp, PlotDimensions dims, bool lowQuality = false, bool clipToDataArea = true)
        {
            Graphics gfx = Graphics(bmp, lowQuality, dims.ScaleFactor);

            if (clipToDataArea)
                gfx.Clip = new Region(new RectangleF(dims.DataOffsetX, dims.DataOffsetY, dims.DataWidth, dims.DataHeight));
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

        public static System.Drawing.Font Font(ScottPlot.Drawing.Font font) =>
            Font(font.Name, font.Size, font.Bold);

        public static System.Drawing.Font Font(string fontName = null, float fontSize = 12, bool bold = false)
        {
            string validFontName = InstalledFont.ValidFontName(fontName);
            FontStyle fontStyle = bold ? FontStyle.Bold : FontStyle.Regular;
            return new System.Drawing.Font(validFontName, fontSize, fontStyle, GraphicsUnit.Pixel);
        }

        public static StringFormat StringFormat(HorizontalAlignment h = HorizontalAlignment.Left, VerticalAlignment v = VerticalAlignment.Lower)
        {
            var sf = new StringFormat();

            if (h == HorizontalAlignment.Left)
                sf.Alignment = StringAlignment.Near;
            else if (h == HorizontalAlignment.Center)
                sf.Alignment = StringAlignment.Center;
            else if (h == HorizontalAlignment.Right)
                sf.Alignment = StringAlignment.Far;
            else
                throw new NotImplementedException();

            if (v == VerticalAlignment.Upper)
                sf.LineAlignment = StringAlignment.Near;
            else if (v == VerticalAlignment.Middle)
                sf.LineAlignment = StringAlignment.Center;
            else if (v == VerticalAlignment.Lower)
                sf.LineAlignment = StringAlignment.Far;

            return sf;
        }

        public static Bitmap Resize(Image bmp, int width, int height)
        {
            var bmp2 = new Bitmap(width, height);
            var rect = new Rectangle(0, 0, width, height);

            using (var gfx = System.Drawing.Graphics.FromImage(bmp2))
            using (var attribs = new ImageAttributes())
            {
                gfx.CompositingMode = CompositingMode.SourceCopy;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                attribs.SetWrapMode(WrapMode.TileFlipXY);
                gfx.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attribs);
            }

            return bmp2;
        }
    }
}
