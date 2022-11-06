using ScottPlot.Ticks;
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
        private LegendItem[] LegendItems = Array.Empty<LegendItem>();

        /// <summary>
        /// Number of items appearing in the legend during the last render
        /// </summary>
        public int Count => (LegendItems is null) ? 0 : LegendItems.Length;

        /// <summary>
        /// Returns true if the legend contained items during the last render
        /// </summary>
        public bool HasItems => LegendItems is not null && LegendItems.Length > 0;

        public Alignment Location { get; set; } = Alignment.LowerRight;
        public bool FixedLineWidth { get; set; } = false;
        public bool ReverseOrder { get; set; } = false;
        public bool AntiAlias { get; set; } = true;
        public bool IsVisible { get; set; } = false;
        public bool IsDetached { get; set; } = false;

        public Color FillColor { get; set; } = Color.White;
        public Color OutlineColor { get; set; } = Color.Black;
        public Color ShadowColor { get; set; } = Color.FromArgb(50, Color.Black);
        public float ShadowOffsetX { get; set; } = 2;
        public float ShadowOffsetY { get; set; } = 2;

        public Orientation Orientation { get; set; } = Orientation.Vertical;

        public Drawing.Font Font { get; } = new Drawing.Font();
        public string FontName { set { Font.Name = value; } }
        public float FontSize { set { Font.Size = value; } }
        public Color FontColor { set { Font.Color = value; } }
        public bool FontBold { set { Font.Bold = value; } }

        public float Padding { get; set; } = 5;
        private float SymbolWidth { get { return 40 * Font.Size / 12; } }
        private float SymbolPad { get { return Font.Size / 3; } }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible is false || LegendItems is null || LegendItems.Length == 0)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            using (var font = GDI.Font(Font))
            {
                var (maxLabelWidth, maxLabelHeight, width, height) = GetDimensions(gfx, LegendItems, font);
                var (x, y) = GetLocationPx(dims, width, height);
                Render(gfx, LegendItems, font, x, y, width, height, maxLabelHeight);
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
            Render(gfx, LegendItems, legendFont, 0, 0, totalLabelWidth, totalLabelHeight, maxLabelHeight);

            return bmp;
        }

        private (float maxLabelWidth, float maxLabelHeight, float width, float height) GetDimensions(Graphics gfx, LegendItem[] items, System.Drawing.Font font)
        {
            // determine maximum label size and use it to define legend size
            float maxLabelWidth = 0;
            float maxLabelHeight = 0;
            float totalLegendWidthWhenHorizontal = 0;

            for (int i = 0; i < items.Length; i++)
            {
                SizeF labelSize = gfx.MeasureString(items[i].label, font);
                maxLabelWidth = Math.Max(maxLabelWidth, labelSize.Width);

                totalLegendWidthWhenHorizontal += SymbolWidth + labelSize.Width + SymbolPad;
                maxLabelHeight = Math.Max(maxLabelHeight, labelSize.Height);
            }

            float width = 0;
            float height = 0;

            if (Orientation == Orientation.Vertical)
            {
                width = SymbolWidth + maxLabelWidth + SymbolPad;
                height = maxLabelHeight * items.Length;
            }
            else if (Orientation == Orientation.Horizontal)
            {
                width = totalLegendWidthWhenHorizontal;
                height = maxLabelHeight;
            }

            return (maxLabelWidth, maxLabelHeight, width, height);
        }

        private void Render(Graphics gfx, LegendItem[] items, System.Drawing.Font font,
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

                float offsetX = 0;
                float offsetY = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    SizeF labelSize = gfx.MeasureString(items[i].label, font);

                    LegendItem item = items[i];

                    float itemStartXLocation = locationX + offsetX;
                    float itemStartYLocation = locationY + offsetY;

                    item.Render(gfx, itemStartXLocation, itemStartYLocation,
                        labelSize.Width, maxLabelHeight, font,
                        SymbolWidth, SymbolPad, outlinePen, textBrush, legendItemHideBrush);

                    if (Orientation == Orientation.Vertical)
                    {
                        offsetY += maxLabelHeight;
                    }
                    else if (Orientation == Orientation.Horizontal)
                    {
                        offsetX += SymbolWidth + labelSize.Width + SymbolPad;
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
