using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// This class represents a single radial gauge.
    /// It has level and styling options and can be rendered onto an existing bitmap using any radius.
    /// </summary>
    internal class RadialGauge
    {
        /// <summary>
        /// Location of the base of the gauge (degrees)
        /// </summary>
        public double StartAngle { get; set; }

        /// <summary>
        /// Current level of this gauge (degrees)
        /// </summary>
        public double SweepAngle { get; set; }

        /// <summary>
        /// Maximum angular size of the gauge (swept degrees)
        /// </summary>
        public double MaximumSizeAngle { get; set; }

        /// <summary>
        /// Angle where the background starts (degrees)
        /// </summary>
        public double BackStartAngle { get; set; }

        /// <summary>
        /// If true angles end clockwise relative to their base
        /// </summary>
        public bool Clockwise { get; set; }

        /// <summary>
        /// Used internally to get the angle swept by the gauge background. It's equal to 360 degrees if CircularBackground is set to true. Also, returns a positive value is the gauge is drawn clockwise and a negative one otherwise
        /// </summary>
        internal double BackAngleSweep
        {
            get
            {
                double maxBackAngle = CircularBackground ? 360 : MaximumSizeAngle;
                if (!Clockwise) maxBackAngle = -maxBackAngle;
                return maxBackAngle;
            }
            private set { BackAngleSweep = value; } // Added for the sweepAngle check in DrawArc due to System.Drawing throwing an OutOfMemoryException.
        }

        /// <summary>
        /// If true the background will always be drawn as a complete circle regardless of MaximumSizeAngle
        /// </summary>
        public bool CircularBackground { get; set; } = true;

        /// <summary>
        /// Font used to render values at the tip of the gauge
        /// </summary>
        public Drawing.Font Font { get; set; }

        /// <summary>
        /// Size of the font relative to the line thickness
        /// </summary>
        public double FontSizeFraction { get; set; }

        /// <summary>
        /// Text to display on top of the label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Location of the label text along the length of the gauge.
        /// Low values place the label near the base and high values place the label at its tip.
        /// </summary>
        public double LabelPositionFraction { get; set; }

        /// <summary>
        /// Size of the gauge (pixels)
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Color of the gauge foreground
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Color of the gauge background
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Style of the base of the gauge
        /// </summary>
        public System.Drawing.Drawing2D.LineCap StartCap { get; set; } = System.Drawing.Drawing2D.LineCap.Round;

        /// <summary>
        /// Style of the tip of the gauge
        /// </summary>
        public System.Drawing.Drawing2D.LineCap EndCap { get; set; } = System.Drawing.Drawing2D.LineCap.Round;

        /// <summary>
        /// Defines the location of each gauge relative to the start angle and distance from the center
        /// </summary>
        public RadialGaugeMode Mode { get; set; }

        /// <summary>
        /// Indicates whether or not labels will be rendered as text
        /// </summary>
        public bool ShowLabels { get; set; }

        /// <summary>
        /// Render the gauge onto an existing Bitmap
        /// </summary>
        /// <param name="gfx">active graphics object</param>
        /// <param name="dims">plot dimensions (used to determine pixel scaling)</param>
        /// <param name="centerPixel">pixel location on the bitmap to center the gauge on</param>
        /// <param name="radius">distance from the center (pixel units) to render the gauge</param>
        public void Render(Graphics gfx, PlotDimensions dims, PointF centerPixel, float radius)
        {
            RenderBackground(gfx, centerPixel, radius);
            RenderGaugeForeground(gfx, centerPixel, radius);
            RenderGaugeLabels(gfx, dims, centerPixel, radius);
        }

        private void RenderBackground(Graphics gfx, PointF center, float radius)
        {
            if (Mode == RadialGaugeMode.SingleGauge)
                return;

            using Pen backgroundPen = GDI.Pen(BackgroundColor);
            backgroundPen.Width = (float)Width;
            backgroundPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            backgroundPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            // This check is specific to System.Drawing since DrawArc throws an OutOfMemoryException when the sweepAngle is very small.
            if (Math.Abs(BackAngleSweep) <= 0.01)
                BackAngleSweep = 0;

            gfx.DrawArc(backgroundPen,
                        (center.X - radius),
                        (center.Y - radius),
                        (radius * 2),
                        (radius * 2),
                        (float)BackStartAngle,
                        (float)BackAngleSweep);
        }

        public void RenderGaugeForeground(Graphics gfx, PointF center, float radius)
        {
            using Pen pen = GDI.Pen(Color);
            pen.Width = (float)Width;
            pen.StartCap = StartCap;
            pen.EndCap = EndCap;

            // This check is specific to System.Drawing since DrawArc throws an OutOfMemoryException when the sweepAngle is very small.
            if (Math.Abs(SweepAngle) <= 0.01)
                SweepAngle = 0;

            gfx.DrawArc(
                pen,
                (center.X - radius),
                (center.Y - radius),
                (radius * 2),
                (radius * 2),
                (float)StartAngle,
                (float)SweepAngle);
        }

        private const double DEG_PER_RAD = 180.0 / Math.PI;

        private void RenderGaugeLabels(Graphics gfx, PlotDimensions dims, PointF center, float radius)
        {
            if (!ShowLabels)
                return;

            // TODO: use this so font size is in pixels not pt
            //Font.Size = lineWidth * (float)FontSizeFraction;
            //using System.Drawing.Font fontGauge = GDI.Font(Font);

            using Brush brush = GDI.Brush(Font.Color);
            using System.Drawing.Font font = new(Font.Name, (float)Width * (float)FontSizeFraction, FontStyle.Bold);

            using StringFormat sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Middle);

            RectangleF[] letterRectangles = MeasureCharacters(gfx, font, Label);
            double totalLetterWidths = letterRectangles.Select(rect => rect.Width).Sum();
            double textWidthFrac = totalLetterWidths / radius;

            double angle = ReduceAngle(StartAngle + SweepAngle * LabelPositionFraction);
            double angle2 = (1 - 2 * LabelPositionFraction) * DEG_PER_RAD * textWidthFrac / 2;
            bool isPositive = (SweepAngle > 0);
            angle += isPositive ? angle2 : -angle2;

            bool isBelow = angle < 180 && angle > 0;
            int sign = isBelow ? 1 : -1;
            double theta = angle * Math.PI / 180;
            theta += textWidthFrac / 2 * sign;

            for (int i = 0; i < Label.Length; i++)
            {
                theta -= letterRectangles[i].Width / 2 / radius * sign;
                double rotation = (theta - Math.PI / 2 * sign) * DEG_PER_RAD;
                float x = center.X + radius * (float)Math.Cos(theta);
                float y = center.Y + radius * (float)Math.Sin(theta);

                gfx.RotateTransform((float)rotation);
                gfx.TranslateTransform(x, y, System.Drawing.Drawing2D.MatrixOrder.Append);
                gfx.DrawString(Label[i].ToString(), font, brush, 0, 0, sf);
                GDI.ResetTransformPreservingScale(gfx, dims);

                theta -= letterRectangles[i].Width / 2 / radius * sign;
            }
        }

        /// <summary>
        /// Return an array indicating the size of each character in a string.
        /// Specifiy the maximum expected size to avoid issues associated with text wrapping.
        /// </summary>
        private static RectangleF[] MeasureCharacters(Graphics gfx, System.Drawing.Font font, string text, int maxWidth = 800, int maxHeight = 100)
        {
            using StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.None,
                FormatFlags = StringFormatFlags.MeasureTrailingSpaces,
            };

            CharacterRange[] charRanges = Enumerable.Range(0, text.Length)
                .Select(x => new CharacterRange(x, 1))
                .ToArray();

            stringFormat.SetMeasurableCharacterRanges(charRanges);

            RectangleF imageRectangle = new(0, 0, maxWidth, maxHeight);
            Region[] characterRegions = gfx.MeasureCharacterRanges(text, font, imageRectangle, stringFormat);
            RectangleF[] characterRectangles = characterRegions.Select(x => x.GetBounds(gfx)).ToArray();

            return characterRectangles;
        }

        /// <summary>
        /// Reduces an angle into the range [0°-360°].
        /// Angles greater than 360 will roll-over (370º becomes 10º).
        /// Angles less than 0 will roll-under (-10º becomes 350º).
        /// </summary>
        /// <param name="angle">Angle value</param>
        /// <returns>Angle whithin [0°-360°]</returns>
        public static double ReduceAngle(double angle)
        {
            angle %= 360;

            if (angle < 0)
                angle += 360;

            return angle;
        }
    }
}
