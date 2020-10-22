using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;

namespace ScottPlot
{
    public class PlottableAnnotation : Plottable, IPlottable
    {
        // TODO: capitalize these fields in a future version
        public double xPixel;
        public double yPixel;
        public string label;

        public string FontName;
        public Color FontColor = Color.Red;
        public float FontSize = 12;
        public bool FontBold = false;

        public bool Background = true;
        public Color BackgroundColor = Color.White;

        public bool Shadow = true;
        public Color ShadowColor = Color.FromArgb(25, Color.Black);

        public bool Border = true;
        public float BorderWidth = 2;
        public Color BorderColor = Color.Black;

        public override string ToString() => $"PlottableAnnotation at ({xPixel} px, {yPixel} px)";
        public override int GetPointCount() => 1;
        public override AxisLimits2D GetLimits() => new AxisLimits2D();
        public override LegendItem[] GetLegendItems() => null;
        public override void Render(Settings settings) => throw new InvalidOperationException("Use other Render method");

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            try
            {
                Validate.AssertIsReal("xPixel", xPixel);
                Validate.AssertIsReal("yPixel", yPixel);
                Validate.AssertHasText("label", label);
            }
            catch (ArgumentException e)
            {
                ValidationErrorMessage = e.Message;
                return false;
            }

            ValidationErrorMessage = null;
            return true;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var shadowBrush = new SolidBrush(ShadowColor))
            using (var backgroundBrush = new SolidBrush(BackgroundColor))
            using (var borderPen = new Pen(BorderColor, BorderWidth))
            {
                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                SizeF size = GDI.MeasureString(gfx, label, font);

                double x = (xPixel >= 0) ? xPixel : dims.DataWidth + xPixel - size.Width;
                double y = (yPixel >= 0) ? yPixel : dims.DataHeight + yPixel - size.Height;
                PointF location = new PointF((float)x, (float)y);

                if (Background && Shadow)
                    gfx.FillRectangle(shadowBrush, location.X + 5, location.Y + 5, size.Width, size.Height);

                if (Background)
                    gfx.FillRectangle(backgroundBrush, location.X, location.Y, size.Width, size.Height);

                if (Border)
                    gfx.DrawRectangle(borderPen, location.X, location.Y, size.Width, size.Height);

                gfx.DrawString(label, font, fontBrush, location);
            }
        }
    }
}
