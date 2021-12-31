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
    public class Text : IPlottable, IHasPixelOffset
    {
        // data
        public double X;
        public double Y;
        public string Label;

        // customization
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public bool BackgroundFill = false;
        public Color BackgroundColor;
        public Drawing.Font Font = new Drawing.Font();
        public Color Color { set => Font.Color = value; }
        public string FontName { set => Font.Name = value; }
        public float FontSize { set => Font.Size = value; }
        public bool FontBold { set => Font.Bold = value; }
        public Alignment Alignment { set => Font.Alignment = value; }
        public float Rotation { set => Font.Rotation = value; }
        public float BorderSize { get; set; } = 0;
        public Color BorderColor { get; set; } = Color.Black;
        public float PixelOffsetX { get; set; } = 0;
        public float PixelOffsetY { get; set; } = 0;

        public override string ToString() => $"PlottableText \"{Label}\" at ({X}, {Y})";
        public AxisLimits GetAxisLimits() => new AxisLimits(X, X, Y, Y);
        public LegendItem[] GetLegendItems() => null;

        public void ValidateData(bool deep = false)
        {
            if (double.IsNaN(X) || double.IsNaN(Y))
                throw new InvalidOperationException("X and Y cannot be NaN");

            if (double.IsInfinity(X) || double.IsInfinity(Y))
                throw new InvalidOperationException("X and Y cannot be Infinity");

            if (string.IsNullOrWhiteSpace(Label))
                throw new InvalidOperationException("text cannot be null or whitespace");
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (string.IsNullOrWhiteSpace(Label) || IsVisible == false)
                return;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fontBrush = new SolidBrush(Font.Color))
            using (var frameBrush = new SolidBrush(BackgroundColor))
            using (var outlinePen = new Pen(BorderColor, BorderSize))
            {
                float pixelX = dims.GetPixelX(X) + PixelOffsetX;
                float pixelY = dims.GetPixelY(Y) - PixelOffsetY;
                SizeF stringSize = GDI.MeasureString(gfx, Label, font);

                gfx.TranslateTransform(pixelX, pixelY);
                gfx.RotateTransform(Font.Rotation);

                (float dX, float dY) = GDI.TranslateString(Label, Font);
                gfx.TranslateTransform(-dX, -dY);

                if (BackgroundFill)
                {
                    RectangleF stringRect = new(0, 0, stringSize.Width, stringSize.Height);
                    gfx.FillRectangle(frameBrush, stringRect);
                    if (BorderSize > 0)
                        gfx.DrawRectangle(outlinePen, stringRect.X, stringRect.Y, stringRect.Width, stringRect.Height);
                }

                gfx.DrawString(Label, font, fontBrush, new PointF(0, 0));

                GDI.ResetTransformPreservingScale(gfx, dims);
            }
        }
    }
}
