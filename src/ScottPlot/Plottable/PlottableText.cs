using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;

namespace ScottPlot.Plottable
{
    public class PlottableText : IPlottable
    {
        /// <summary>
        /// Horizontal position in coordinate space
        /// </summary>
        public double x;

        /// <summary>
        /// Vertical position in coordinate space
        /// </summary>
        public double y;

        /// <summary>
        /// Rotation of text in degrees. If rotation is used text alignment is ignored and the top left corner is fixed.
        /// </summary>
        public double rotation;

        /// <summary>
        /// Text to display on the plot
        /// </summary>
        public string text;

        /// <summary>
        /// Defines where the x/y point is relative to the text. 
        /// Alignment is ignored when rotation is enabled.
        /// </summary>
        public Alignment alignment;

        /// <summary>
        /// Whether or not to draw a border around the text
        /// </summary>
        public bool frame;

        /// <summary>
        /// Color of the border around the text
        /// </summary>
        public Color frameColor;

        /// <summary>
        /// Color of the text
        /// </summary>
        public Color FontColor = Color.Black;

        /// <summary>
        /// Name of the text font.
        /// If this font does not exist a system default will be used.
        /// </summary>
        public string FontName;

        /// <summary>
        /// Font size (in pixels)
        /// </summary>
        public float FontSize = 12;

        /// <summary>
        /// Renders bold font if true
        /// </summary>
        public bool FontBold;

        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// The Text plot type displays a string at an X/Y position in coordinate space.
        /// </summary>
        public PlottableText() { }

        public override string ToString() => $"PlottableText \"{text}\" at ({x}, {y})";

        public AxisLimits2D GetLimits() => new AxisLimits2D(x, x, y, y);

        public void Render(Settings settings) => throw new NotImplementedException("Use the other Render method");

        public int PointCount { get => 1; }

        public LegendItem[] LegendItems { get => null; }

        public string ErrorMessage(bool deepValidation = false)
        {
            try
            {
                Validate.AssertIsReal("x", x);
                Validate.AssertIsReal("y", y);
                Validate.AssertIsReal("rotation", rotation);
                Validate.AssertHasText("text", text);
                return null;
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Returns the point in pixel space shifted by the necessary amount to apply text alignment
        /// </summary>
        private (float pixelX, float pixelY) ApplyAlignmentOffset(float pixelX, float pixelY, float stringWidth, float stringHeight)
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
            if (string.IsNullOrWhiteSpace(text))
                return; // no render needed

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            {
                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                float pixelX = dims.GetPixelX(x);
                float pixelY = dims.GetPixelY(y);
                SizeF stringSize = GDI.MeasureString(gfx, text, font);
                RectangleF stringRect = new RectangleF(0, 0, stringSize.Width, stringSize.Height);

                if (rotation == 0)
                    (pixelX, pixelY) = ApplyAlignmentOffset(pixelX, pixelY, stringSize.Width, stringSize.Height);

                gfx.TranslateTransform(pixelX, pixelY);
                gfx.RotateTransform((float)rotation);

                if (frame)
                {
                    using (var frameBrush = new SolidBrush(frameColor))
                    {
                        gfx.FillRectangle(frameBrush, stringRect);
                    }
                }

                using (var fontBrush = new SolidBrush(FontColor))
                {
                    gfx.DrawString(text, font, fontBrush, new PointF(0, 0));
                }

                gfx.ResetTransform();
            }
        }
    }
}
