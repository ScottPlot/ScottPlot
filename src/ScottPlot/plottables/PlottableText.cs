using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using ScottPlot.plottables;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableText : Plottable, IPlottable
    {
        public double x;
        public double y;
        public double rotation;
        public string text;
        public TextAlignment alignment;
        public bool frame;
        public Color frameColor;
        public string label;

        public Color FontColor;
        public string FontName;
        public float FontSize;
        public bool FontBold;

        public PlottableText() { }

        [Obsolete("use the new constructor with data-only arguments", true)]
        public PlottableText(string text, double x, double y, Color color, string fontName, double fontSize, bool bold, string label, TextAlignment alignment, double rotation, bool frame, Color frameColor)
        {
            this.text = text ?? throw new Exception("Text cannot be null");
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.label = label;
            this.alignment = alignment;
            this.frame = frame;
            this.frameColor = frameColor;

            (FontColor, FontName, FontSize, FontBold) = (color, fontName, (float)fontSize, bold);
        }

        public override string ToString() => $"PlottableText \"{text}\" at ({x}, {y})";

        public override AxisLimits2D GetLimits() => new AxisLimits2D(x, x, y, y);

        public override void Render(Settings settings) => throw new NotImplementedException("Use the other Render method");

        public override int GetPointCount() => 1;

        public override LegendItem[] GetLegendItems() => null; // never show in legend

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            StringBuilder sb = new StringBuilder();

            if (double.IsInfinity(x) || double.IsNaN(x))
                sb.AppendLine("X must be a rational number");

            if (double.IsInfinity(y) || double.IsNaN(y))
                sb.AppendLine("Y must be a rational number");

            if (double.IsInfinity(rotation) || double.IsNaN(rotation))
                sb.AppendLine("rotation must be a rational number");

            if (FontSize < 1)
                sb.AppendLine("font must be at least size 1");

            ValidationErrorMessage = sb.ToString();

            return ValidationErrorMessage.Length == 0;
        }

        private (float pixelX, float pixelY) ApplyAlignmentOffset(float pixelX, float pixelY, float stringWidth, float stringHeight)
        {
            switch (alignment)
            {
                case TextAlignment.lowerCenter:
                    return (pixelX - stringWidth / 2, pixelY - stringHeight);
                case TextAlignment.lowerLeft:
                    return (pixelX, pixelY - stringHeight);
                case TextAlignment.lowerRight:
                    return (pixelX - stringWidth, pixelY - stringHeight);
                case TextAlignment.middleLeft:
                    return (pixelX, pixelY - stringHeight / 2);
                case TextAlignment.middleRight:
                    return (pixelX - stringWidth, pixelY - stringHeight / 2);
                case TextAlignment.upperCenter:
                    return (pixelX - stringWidth / 2, pixelY);
                case TextAlignment.upperLeft:
                    return (pixelX, pixelY);
                case TextAlignment.upperRight:
                    return (pixelX - stringWidth, pixelY);
                case TextAlignment.middleCenter:
                    return (pixelX - stringWidth / 2, pixelY - stringHeight / 2);
                default:
                    throw new InvalidEnumArgumentException("that alignment is not recognized");
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp)
        {
            if (IsValidData() == false)
                throw new InvalidOperationException($"Invalid data: {ValidationErrorMessage}");

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var frameBrush = new SolidBrush(frameColor))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            {
                float pixelX = dims.GetPixelX(x);
                float pixelY = dims.GetPixelY(y);
                SizeF stringSize = GDI.MeasureString(gfx, text, font);
                RectangleF stringRect = new RectangleF(0, 0, stringSize.Width, stringSize.Height);

                if (rotation == 0)
                    (pixelX, pixelY) = ApplyAlignmentOffset(pixelX, pixelY, stringSize.Width, stringSize.Height);

                gfx.TranslateTransform(pixelX, pixelY);
                gfx.RotateTransform((float)rotation);

                if (frame)
                    gfx.FillRectangle(frameBrush, stringRect);

                gfx.DrawString(text, font, fontBrush, new PointF(0, 0));

                gfx.ResetTransform();
            }
        }
    }
}
