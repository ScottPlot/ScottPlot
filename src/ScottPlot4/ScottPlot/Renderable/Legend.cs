﻿using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace ScottPlot.Renderable
{
    public class Legend : IRenderable
    {
        public Direction Location = Direction.SE;
        public bool FixedLineWidth = false;
        public bool ReverseOrder = false;
        public bool AntiAlias = true;
        public bool Visible = false;

        public Color FillColor = Color.White;
        public Color OutlineColor = Color.Black;
        public Color ShadowColor = Color.FromArgb(50, Color.Black);
        public float ShadowOffsetX = 2;
        public float ShadowOffsetY = 2;

        private string _fontName = Fonts.GetDefaultFontName();
        public string FontName { get { return _fontName; } set { _fontName = Fonts.GetValidFontName(FontName); } }
        public float FontSize = 14;
        public Color FontColor = Color.Black;
        public bool FontBold = false;
        private FontStyle FontStyle { get { return FontBold ? FontStyle.Bold : FontStyle.Regular; } }

        public float Padding = 5;
        private float symbolWidth { get { return 40 * FontSize / 12; } }
        private float symbolPad { get { return FontSize / 3; } }
        private float markerWidth { get { return FontSize / 2; } }

        public void Render(Settings settings)
        {
            if (Visible is false)
                return;

            using (var gfx = Graphics.FromImage(settings.bmpFigure))
            using (var font = new Font(FontName, FontSize, FontStyle, GraphicsUnit.Pixel))
            {
                var items = GetLegendItems(settings);
                if (items.Length == 0)
                    return;

                var (maxLabelWidth, maxLabelHeight, width, height) = GetDimensions(gfx, items, font);
                var (x, y) = GetLocationPx(settings, width, height);
                RenderOnBitmap(gfx, items, font, x, y, width, height, maxLabelHeight);
            }
        }

        public Bitmap GetBitmap(Settings settings)
        {
            using (var bmpTemp = new Bitmap(1, 1))
            using (var gfxTemp = Graphics.FromImage(bmpTemp))
            using (var font = new Font(FontName, FontSize, FontStyle, GraphicsUnit.Pixel))
            {
                var items = GetLegendItems(settings);
                if (items.Length == 0)
                    return null;

                var (maxLabelWidth, maxLabelHeight, width, height) = GetDimensions(gfxTemp, items, font);
                Bitmap bmp = new Bitmap((int)width, (int)height, PixelFormat.Format32bppPArgb);

                using (var gfx = Graphics.FromImage(bmp))
                    RenderOnBitmap(gfx, items, font, 0, 0, width, height, maxLabelHeight);

                return bmp;
            }
        }

        private (float maxLabelWidth, float maxLabelHeight, float width, float height)
            GetDimensions(Graphics gfx, LegendItem[] items, Font font)
        {
            // determine maximum label size and use it to define legend size
            float maxLabelWidth = 0;
            float maxLabelHeight = 0;
            for (int i = 0; i < items.Length; i++)
            {
                var labelSize = gfx.MeasureString(items[i].label, font);
                maxLabelWidth = Math.Max(maxLabelWidth, labelSize.Width);
                maxLabelHeight = Math.Max(maxLabelHeight, labelSize.Height);
            }

            float width = symbolWidth + maxLabelWidth + symbolPad;
            float height = maxLabelHeight * items.Length;

            return (maxLabelWidth, maxLabelHeight, width, height);
        }

        private void RenderOnBitmap(Graphics gfx, LegendItem[] items, Font font,
            float locationX, float locationY, float width, float height, float maxLabelHeight,
            bool shadow = true, bool fill = true, bool outline = true)
        {
            using (var fillBrush = new SolidBrush(FillColor))
            using (var shadowBrush = new SolidBrush(ShadowColor))
            using (var textBrush = new SolidBrush(FontColor))
            using (var outlinePen = new Pen(OutlineColor))
            {
                if (AntiAlias)
                {
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                }

                RectangleF rectShadow = new RectangleF(locationX + ShadowOffsetX, locationY + ShadowOffsetY, width, height);
                RectangleF rectFill = new RectangleF(locationX, locationY, width, height);

                if (shadow)
                    gfx.FillRectangle(shadowBrush, rectShadow);

                gfx.FillRectangle(fillBrush, rectFill);

                if (outline)
                    gfx.DrawRectangle(outlinePen, Rectangle.Round(rectFill));

                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    float verticalOffset = i * maxLabelHeight;

                    // draw text
                    gfx.DrawString(item.label, font, textBrush, locationX + symbolWidth, locationY + verticalOffset);

                    // draw line
                    outlinePen.Color = item.color;
                    outlinePen.Width = 1;
                    float lineY = locationY + verticalOffset + maxLabelHeight / 2;
                    float lineX1 = locationX + symbolPad;
                    float lineX2 = lineX1 + symbolWidth - symbolPad * 2;
                    using (var linePen = GDI.Pen(item.color, item.lineWidth, item.lineStyle, false))
                        gfx.DrawLine(linePen, lineX1, lineY, lineX2, lineY);

                    // draw marker
                    float lineXcenter = (lineX1 + lineX2) / 2;
                    PointF markerPoint = new PointF(lineXcenter, lineY);
                    MarkerTools.DrawMarker(gfx, markerPoint, item.markerShape, markerWidth, item.color);
                }
            }
        }

        public LegendItem[] GetLegendItems(Settings settings)
        {
            var items = new List<LegendItem>();

            foreach (Plottable plottable in settings.plottables)
            {
                var legendItems = plottable.GetLegendItems();

                if (legendItems is null)
                    continue;

                foreach (var plottableItem in legendItems)
                    if (plottableItem.label != null)
                        items.Add(plottableItem);
            }

            if (ReverseOrder)
                items.Reverse();

            return items.ToArray();
        }

        private (float x, float y) GetLocationPx(Settings settings, float width, float height)
        {
            float leftX = settings.dataOrigin.X + Padding;
            float rightX = settings.dataOrigin.X + settings.dataSize.Width - Padding - width;
            float centerX = settings.dataOrigin.X + settings.dataSize.Width / 2 - width / 2;

            float topY = settings.dataOrigin.Y + Padding;
            float bottomY = settings.dataOrigin.Y + settings.dataSize.Height - Padding - height;
            float centerY = settings.dataOrigin.Y + settings.dataSize.Height / 2 - height / 2;

            switch (Location)
            {
                case Direction.NW:
                    return (leftX, topY);
                case Direction.N:
                    return (centerX, topY);
                case Direction.NE:
                    return (rightX, topY);
                case Direction.E:
                    return (rightX, centerY);
                case Direction.SE:
                    return (rightX, bottomY);
                case Direction.S:
                    return (centerX, bottomY);
                case Direction.SW:
                    return (leftX, bottomY);
                case Direction.W:
                    return (leftX, centerY);
                case Direction.C:
                    return (centerX, centerY);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
