﻿using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using ScottPlot.Plottable;
using System.Linq;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class Legend : IRenderable
    {
        /// <summary>
        /// List of items appearing in the legend during the last render
        /// </summary>
        private LegendItem[] LegendItems = new LegendItem[] { };

        /// <summary>
        /// Number of items appearing in the legend during the last render
        /// </summary>
        public int Count => (LegendItems is null) ? 0 : LegendItems.Length;

        /// <summary>
        /// Returns true if the legend contained items during the last render
        /// </summary>
        public bool HasItems => LegendItems is not null && LegendItems.Length > 0;

        public Alignment Location = Alignment.LowerRight;
        public bool FixedLineWidth = false;
        public bool ReverseOrder = false;
        public bool AntiAlias = true;
        public bool IsVisible { get; set; } = false;
        public bool IsDetached { get; set; } = false;

        public System.Drawing.Color FillColor = System.Drawing.Color.White;
        public System.Drawing.Color OutlineColor = System.Drawing.Color.Black;
        public System.Drawing.Color ShadowColor = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Black);
        public float ShadowOffsetX = 2;
        public float ShadowOffsetY = 2;

        public Drawing.Font Font = new Drawing.Font();
        public string FontName { set { Font.Name = value; } }
        public float FontSize { set { Font.Size = value; } }
        public System.Drawing.Color FontColor { set { Font.Color = value; } }
        public bool FontBold { set { Font.Bold = value; } }

        public float Padding = 5;
        private float SymbolWidth { get { return 40 * Font.Size / 12; } }
        private float SymbolPad { get { return Font.Size / 3; } }
        private float MarkerWidth { get { return Font.Size / 2; } }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible is false || LegendItems is null || LegendItems.Length == 0)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            using (var font = GDI.Font(Font))
            {
                var (maxLabelWidth, maxLabelHeight, width, height) = GetDimensions(gfx, LegendItems, font);
                var (x, y) = GetLocationPx(dims, width, height);
                RenderOnBitmap(gfx, LegendItems, font, x, y, width, height, maxLabelHeight);
            }
        }

        /// <summary>
        /// Creates and returns a Bitmap containing all legend items displayed at the last render.
        /// This will be 1px transparent Bitmap if no render was performed previously or if there are no legend items.
        /// </summary>
        public Bitmap GetBitmap(bool lowQuality = false, double scale = 1.0)
        {
            if (LegendItems.Length == 0)
                return new Bitmap(1, 1);

            // use a temporary bitmap and graphics (without scaling) to measure how large the final image should be
            using var tempBitmap = new Bitmap(1, 1);
            using var tempGfx = GDI.Graphics(tempBitmap, lowQuality, scale);
            using var legendFont = GDI.Font(Font);
            var (maxLabelWidth, maxLabelHeight, totalLabelWidth, totalLabelHeight) = GetDimensions(tempGfx, LegendItems, legendFont);

            // create the actual legend bitmap based on the scaled measured size
            int width = (int)(totalLabelWidth * scale);
            int height = (int)(totalLabelHeight * scale);
            Bitmap bmp = new(width, height, PixelFormat.Format32bppPArgb);
            using var gfx = GDI.Graphics(bmp, lowQuality, scale);
            RenderOnBitmap(gfx, LegendItems, legendFont, 0, 0, totalLabelWidth, totalLabelHeight, maxLabelHeight);

            return bmp;
        }

        private (float maxLabelWidth, float maxLabelHeight, float width, float height)
            GetDimensions(Graphics gfx, LegendItem[] items, System.Drawing.Font font)
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

            float width = SymbolWidth + maxLabelWidth + SymbolPad;
            float height = maxLabelHeight * items.Length;

            return (maxLabelWidth, maxLabelHeight, width, height);
        }

        private void RenderOnBitmap(Graphics gfx, LegendItem[] items, System.Drawing.Font font,
            float locationX, float locationY, float width, float height, float maxLabelHeight,
            bool shadow = true, bool outline = true)
        {
            using (var fillBrush = new SolidBrush(FillColor))
            using (var shadowBrush = new SolidBrush(ShadowColor))
            using (var textBrush = new SolidBrush(Font.Color))
            using (var outlinePen = new Pen(OutlineColor))
            using (var legendItemHideBrush = GDI.Brush(FillColor, 100))
            {
                RectangleF rectShadow = new RectangleF(locationX + ShadowOffsetX, locationY + ShadowOffsetY, width, height);
                RectangleF rectFill = new RectangleF(locationX, locationY, width, height);

                if (shadow)
                    gfx.FillRectangle(shadowBrush, rectShadow);

                gfx.FillRectangle(fillBrush, rectFill);

                if (outline)
                    gfx.DrawRectangle(outlinePen, Rectangle.Round(rectFill));

                for (int i = 0; i < items.Length; i++)
                {
                    LegendItem item = items[i];
                    float verticalOffset = i * maxLabelHeight;

                    // draw text
                    gfx.DrawString(item.label, font, textBrush, locationX + SymbolWidth, locationY + verticalOffset);

                    // prepare values for drawing a line
                    outlinePen.Color = item.color;
                    outlinePen.Width = 1;
                    float lineY = locationY + verticalOffset + maxLabelHeight / 2;
                    float lineX1 = locationX + SymbolPad;
                    float lineX2 = lineX1 + SymbolWidth - SymbolPad * 2;


                    if (item.ShowAsRectangleInLegend)
                    {
                        // prepare values for drawing a rectangle
                        PointF rectOrigin = new PointF(lineX1, (float)(lineY - 5));
                        SizeF rectSize = new SizeF(lineX2 - lineX1, 10);
                        RectangleF rect = new RectangleF(rectOrigin, rectSize);
                        // draw a rectangle
                        using (var legendItemFillBrush = GDI.Brush(item.color, item.hatchColor, item.hatchStyle))
                        using (var legendItemOutlinePen = GDI.Pen(item.borderColor, item.borderWith, item.borderLineStyle))
                        {
                            gfx.FillRectangle(legendItemFillBrush, rect);
                            gfx.DrawRectangle(legendItemOutlinePen, rect.X, rect.Y, rect.Width, rect.Height);
                        }
                    }
                    else
                    {
                        // draw a line
                        if (item.lineWidth > 0 && item.lineStyle != LineStyle.None)
                        {
                            using var linePen = GDI.Pen(item.LineColor, item.lineWidth, item.lineStyle, false);
                            gfx.DrawLine(linePen, lineX1, lineY, lineX2, lineY);
                        }

                        // and perhaps a marker in the middle of the line
                        float lineXcenter = (lineX1 + lineX2) / 2;
                        PointF markerPoint = new PointF(lineXcenter, lineY);
                        if ((item.markerShape != MarkerShape.none) && (item.markerSize > 0))
                            MarkerTools.DrawMarker(gfx, markerPoint, item.markerShape, item.markerSize, item.MarkerColor, item.markerLineWidth);
                    }

                    // Typically invisible legend items don't make it in the list.
                    // If they do, display them simulating semi-transparency.
                    if (!item.Parent.IsVisible)
                    {
                        PointF hideRectOrigin = new(lineX1, locationY + verticalOffset);
                        SizeF hideRectSize = new(width, maxLabelHeight);
                        RectangleF hideRect = new(hideRectOrigin, hideRectSize);
                        gfx.FillRectangle(legendItemHideBrush, hideRect);
                    }
                }
            }
        }

        public void UpdateLegendItems(Plot plot, bool includeHidden = false)
        {
            LegendItems = plot.GetPlottables()
                .Where(x => x.IsVisible || includeHidden)
                .Where(x => x.GetLegendItems() != null)
                .SelectMany(x => x.GetLegendItems())
                .Where(x => !string.IsNullOrWhiteSpace(x.label))
                .ToArray();

            if (ReverseOrder)
                Array.Reverse(LegendItems);
        }

        /// <summary>
        /// Returns an array of legend items displayed in the last render
        /// </summary>
        public LegendItem[] GetItems() => LegendItems.ToArray();

        private (float x, float y) GetLocationPx(PlotDimensions dims, float width, float height)
        {
            float leftX = dims.DataOffsetX + Padding;
            float rightX = dims.DataOffsetX + dims.DataWidth - Padding - width;
            float centerX = dims.DataOffsetX + dims.DataWidth / 2 - width / 2;

            float topY = dims.DataOffsetY + Padding;
            float bottomY = dims.DataOffsetY + dims.DataHeight - Padding - height;
            float centerY = dims.DataOffsetY + dims.DataHeight / 2 - height / 2;

            switch (Location)
            {
                case Alignment.UpperLeft:
                    return (leftX, topY);
                case Alignment.UpperCenter:
                    return (centerX, topY);
                case Alignment.UpperRight:
                    return (rightX, topY);
                case Alignment.MiddleRight:
                    return (rightX, centerY);
                case Alignment.LowerRight:
                    return (rightX, bottomY);
                case Alignment.LowerCenter:
                    return (centerX, bottomY);
                case Alignment.LowerLeft:
                    return (leftX, bottomY);
                case Alignment.MiddleLeft:
                    return (leftX, centerY);
                case Alignment.MiddleCenter:
                    return (centerX, centerY);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
