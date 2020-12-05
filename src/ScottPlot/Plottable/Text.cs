using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace ScottPlot.Plottable
{
    public class Text : IPlottable
    {
        public double x;
        public double y;
        public string text;
        public bool FillBackground;
        public Color BackgroundColor;

        public Drawing.Font Font = new Drawing.Font();
        public Color FontColor { set => Font.Color = value; }
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Alignment alignment { set => Font.Alignment = value; }
        public float rotation { set => Font.Rotation = value; }

        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public override string ToString() => $"PlottableText \"{text}\" at ({x}, {y})";
        public AxisLimits GetAxisLimits() => new AxisLimits(x, x, y, y);
        public LegendItem[] GetLegendItems() => null;

        // TODO: add options for a border

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
                throw new InvalidOperationException("X and Y cannot be NaN");

            if (double.IsInfinity(x) || double.IsInfinity(y))
                throw new InvalidOperationException("X and Y cannot be Infinity");

            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("text cannot be null or whitespace");
        }

        /// <summary>
        /// Returns the point in pixel space shifted by the necessary amount to apply text alignment
        /// </summary>
        private (float pixelX, float pixelY) ApplyAlignmentOffset(float pixelX, float pixelY, float stringWidth, float stringHeight)
        {
            switch (Font.Alignment)
            {
                case Alignment.LowerCenter:
                    return (pixelX - stringWidth / 2, pixelY - stringHeight);
                case Alignment.LowerLeft:
                    return (pixelX, pixelY - stringHeight);
                case Alignment.LowerRight:
                    return (pixelX - stringWidth, pixelY - stringHeight);
                case Alignment.MiddleLeft:
                    return (pixelX, pixelY - stringHeight / 2);
                case Alignment.MiddleRight:
                    return (pixelX - stringWidth, pixelY - stringHeight / 2);
                case Alignment.UpperCenter:
                    return (pixelX - stringWidth / 2, pixelY);
                case Alignment.UpperLeft:
                    return (pixelX, pixelY);
                case Alignment.UpperRight:
                    return (pixelX - stringWidth, pixelY);
                case Alignment.MiddleCenter:
                    return (pixelX - stringWidth / 2, pixelY - stringHeight / 2);
                default:
                    throw new InvalidEnumArgumentException("that alignment is not recognized");
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (string.IsNullOrWhiteSpace(text) || IsVisible == false)
                return;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fontBrush = new SolidBrush(Font.Color))
            using (var frameBrush = new SolidBrush(BackgroundColor))
            {
                float pixelX = dims.GetPixelX(x);
                float pixelY = dims.GetPixelY(y);
                SizeF stringSize = GDI.MeasureString(gfx, text, font);

                if (Font.Rotation == 0)
                    (pixelX, pixelY) = ApplyAlignmentOffset(pixelX, pixelY, stringSize.Width, stringSize.Height);

                gfx.TranslateTransform(pixelX, pixelY);
                gfx.RotateTransform(Font.Rotation);

                if (FillBackground)
                {
                    RectangleF stringRect = new RectangleF(0, 0, stringSize.Width, stringSize.Height);
                    gfx.FillRectangle(frameBrush, stringRect);
                }

                gfx.DrawString(text, font, fontBrush, new PointF(0, 0));

                gfx.ResetTransform();
            }
        }
    }
}
