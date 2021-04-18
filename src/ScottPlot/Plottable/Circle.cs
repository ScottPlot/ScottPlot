using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display a text label at an X/Y position in coordinate space
    /// </summary>
    public class Circle : IPlottable
    {
        // data
        public double X;
        public double Y;
        public string Label;
        public double Radius;
        public bool AllowEllipse;
        public bool Fill = true;

        // customization
        public bool IsVisible { get; set; } = true;
        public float LineWidth { get; set; } = 1;
        public Color LineColor { get; set; }

        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public Color FillColor;

        public Color HatchColor = Color.Transparent;
        public HatchStyle HatchStyle = HatchStyle.None;

        public Drawing.Font Font = new Drawing.Font();

        public Color FontColor { set => Font.Color = value; }
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public float Rotation { set => Font.Rotation = value; }

        public override string ToString() => $"PlottableCircle \"{Label}\" at ({X}, {Y}) with R=({Radius})";
        public AxisLimits GetAxisLimits() => new AxisLimits(X - Radius, X + Radius, Y - Radius, Y + Radius);
        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsNaN(Y) || double.IsNaN(Radius))
                throw new InvalidOperationException("X, Y and Radius cannot be NaN");

            if (double.IsInfinity(X) || double.IsInfinity(Y) || double.IsInfinity(Radius))
                throw new InvalidOperationException("X, Y and Radius cannot be Infinity");

        }

        // TODO: It would be nice to be able to reuse this code that was copied from Text.ApplyAlignmentOffset. For that reason I have made it a static function here.
        // Ideally make this a public function, put it somewhere appropriate (perhaps Alignment.cs itself), and change the Text calls to use it

        /// <summary>
        /// Returns the point in pixel space shifted by the necessary amount to apply text alignment
        /// </summary>
        private static (float pixelX, float pixelY) ApplyAlignmentOffset(Alignment alignment, float pixelX, float pixelY, float stringWidth, float stringHeight)
        {
            switch (alignment)
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
            if (string.IsNullOrWhiteSpace(Label) || IsVisible == false)
                return;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(Font))
            using (Brush fillBrush = GDI.Brush(FillColor, HatchColor, HatchStyle))
            using (var fontBrush = new SolidBrush(Font.Color))
            using (Pen pen = GDI.Pen(LineColor, LineWidth))
            {
                float pixelX = dims.GetPixelX(X);
                float pixelY = dims.GetPixelY(Y);
                float xRad = (float)(Radius * dims.PxPerUnitX);
                float yRad = (float)(Radius * dims.PxPerUnitY);
                if (!AllowEllipse)
                {
                    xRad = Math.Max(xRad, yRad);
                    yRad = xRad;
                }
                var rect = new RectangleF(pixelX - xRad, pixelY - yRad, 2 * xRad, 2 * yRad);

                if (Fill)
                    gfx.FillEllipse(fillBrush, rect);
                gfx.DrawEllipse(pen, rect);

                if (!string.IsNullOrEmpty(Label))
                {
                    SizeF stringSize = GDI.MeasureString(gfx, Label, font);

                    if (Font.Rotation == 0)
                        (pixelX, pixelY) = ApplyAlignmentOffset(Font.Alignment, pixelX, pixelY, stringSize.Width, stringSize.Height);

                    gfx.TranslateTransform(pixelX, pixelY);
                    gfx.RotateTransform(Font.Rotation);

                    gfx.DrawString(Label, font, fontBrush, new PointF(0, 0));

                    gfx.ResetTransform();
                }
            }
        }
    }
}
