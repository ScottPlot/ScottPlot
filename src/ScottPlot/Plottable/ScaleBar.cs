﻿using System.Drawing;
using ScottPlot.Drawing;
using ScottPlot.Renderable;

namespace ScottPlot.Plottable
{
    public class ScaleBar : IRenderable
    {
        /// <summary>
        /// Width of the scalebar in cooridinate units
        /// </summary>
        public double Width;

        /// <summary>
        /// Height of the scalebar in cooridinate units
        /// </summary>
        public double Height;

        /// <summary>
        /// Horizontal scalebar label
        /// </summary>
        public string HorizontalLabel;

        /// <summary>
        /// Vertical scalebar label
        /// </summary>
        public string VerticalLabel;

        /// <summary>
        /// Distance in pixels from the edge of the data area
        /// </summary>
        public float Padding;

        /// <summary>
        /// Thickness of the scalebar in pixels
        /// </summary>
        public float LineWidth = 2;

        /// <summary>
        /// Color of the scalebar lines
        /// </summary>
        public Color LineColor = Color.Black;

        /// <summary>
        /// Color of the font
        /// </summary>
        public Color FontColor = Color.Black;

        /// <summary>
        /// Name of the font to use for the labels.
        /// If this font does not exist a system default will be used.
        /// </summary>
        public string FontName;

        /// <summary>
        /// Font size in pixels
        /// </summary>
        public float FontSize = 12;

        /// <summary>
        /// Renders bold font if true
        /// </summary>
        public bool FontBold = false;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableScaleBar ({HorizontalLabel}={Width}, {VerticalLabel}={Height})";

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var linePen = new Pen(LineColor, LineWidth))
            using (var sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center })
            using (var sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
            {
                // determine where the corner of the scalebar will be
                float widthPx = (float)(Width * dims.PxPerUnitX);
                float heightPx = (float)(Height * dims.PxPerUnitY);
                PointF cornerPoint = new PointF(dims.GetPixelX(dims.XMax) - Padding, dims.GetPixelY(dims.YMin) - Padding);

                // move the corner point away from the edge to accommodate label size
                var xLabelSize = GDI.MeasureString(gfx, HorizontalLabel, font);
                var yLabelSize = GDI.MeasureString(gfx, VerticalLabel, font);
                cornerPoint.X -= yLabelSize.Width * 1.2f;
                cornerPoint.Y -= yLabelSize.Height;

                // determine all other points relative to the corner point
                PointF horizPoint = new PointF(cornerPoint.X - widthPx, cornerPoint.Y);
                PointF vertPoint = new PointF(cornerPoint.X, cornerPoint.Y - heightPx);
                PointF horizMidPoint = new PointF((cornerPoint.X + horizPoint.X) / 2, cornerPoint.Y);
                PointF vertMidPoint = new PointF(cornerPoint.X, (cornerPoint.Y + vertPoint.Y) / 2);

                // draw the scalebar
                gfx.DrawLines(linePen, new PointF[] { horizPoint, cornerPoint, vertPoint });
                gfx.DrawString(HorizontalLabel, font, fontBrush, horizMidPoint.X, cornerPoint.Y, sfNorth);
                gfx.DrawString(VerticalLabel, font, fontBrush, cornerPoint.X, vertMidPoint.Y, sfWest);
            }
        }
    }
}
