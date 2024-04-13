using System;
using System.Drawing;
using ScottPlot.Drawing;
using ScottPlot.Styles;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// An L-shaped scalebar rendered in the corner of the data area
    /// </summary>
    public class ScaleBar : IPlottable, IStylable, IHasColor
    {
        /// <summary>
        /// Width of the scalebar in cooridinate units
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Height of the scalebar in cooridinate units
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Distance in pixels from the edge of the data area
        /// </summary>
        public float Padding { get; set; } = 10;

        public string HorizontalLabel { get; set; }
        public string VerticalLabel { get; set; }
        public float LineWidth { get; set; } = 2;
        public Color LineColor { get; set; } = Color.Black;
        public readonly Drawing.Font Font = new();
        public float FontSize { get => Font.Size; set => Font.Size = value; }
        public Color FontColor { get => Font.Color; set => Font.Color = value; }
        public bool FontBold { get => Font.Bold; set => Font.Bold = value; }
        public Color Color { get => LineColor; set { LineColor = value; FontColor = value; } }

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public void ValidateData(bool deep = false) { }

        public override string ToString() => $"PlottableScaleBar ({HorizontalLabel}={Width}, {VerticalLabel}={Height})";

        public void SetStyle(Color? tickMarkColor, Color? tickFontColor)
        {
            LineColor = tickMarkColor ?? LineColor;
            FontColor = tickFontColor ?? Font.Color;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fontBrush = new SolidBrush(Font.Color))
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
