using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

// Inspired by MicroCharts:
// https://github.com/dotnet-ad/Microcharts/blob/main/Sources/Microcharts/Charts/RadialGaugeChart.cs

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A radial gauge chart is a graphical method of displaying scalar data in the form of 
    /// a chart made of circular gauges so that each scalar is represented by each gauge.
    /// </summary>
    public class RadialGaugePlot : IPlottable
    {
        /// <summary>
        /// This array holds a copy of the original values used to calculate radial gauge positions.
        /// Values are stored here so positions can be recalculated if configuration options change.
        /// </summary>
        public double[] Levels { get; private set; }

        /// <summary>
        /// Number of gauges.
        /// </summary>
        public int GaugeCount => Levels.Length;

        /// <summary>
        /// Maximum size (degrees) for the gauge.
        /// 180 is a semicircle and 360 is a full circle.
        /// </summary>
        public double GaugeSize = 360;

        /// <summary>
        /// Controls whether the backgrounds of the gauges are full circles or stop at the maximum angle.
        /// </summary>
        public bool CircularBackground { get; set; } = true;

        /// <summary>
        /// Labels that appear in the legend for each gauge.
        /// May be null if gauges are not to appear in the legend.
        /// </summary>
        public string[] Labels;

        /// <summary>
        /// Base colors for each gauge.
        /// These colors are adjusted at rendering time.
        /// </summary>
        public Color[] Colors;

        /// <summary>
        /// Describes how transparent the unfilled background of each gauge is (0 to 1).
        /// The larger the number the darker the background becomes.
        /// </summary>
        public double BackgroundTransparencyFraction = .15;

        /// <summary>
        /// Indicates whether gauges fill clockwise as levels increase.
        /// </summary>
        public bool Clockwise = true;

        /// <summary>
        /// Determines whether the gauges are drawn stacked (dafault value), sequentially, or as a single gauge (ressembling a pie plot).
        /// </summary>
        public RadialGaugeMode GaugeMode = RadialGaugeMode.Stacked;

        /// <summary>
        /// Defines where the gauge label is written on the gage as a fraction of its length.
        /// Low values place the label near the base and high values place the label at its tip.
        /// </summary>
        public double GaugeLabelPosition = 1;

        /// <summary>
        /// Angle (in degrees) at which the gauges start: 270° for North (default value), 0° for East, 90° for South, 180° for West, and so on.
        /// Expected values in the range [0°-360°], otherwise unexpected side-effects might happen.
        /// </summary>
        public float StartingAngleGauges = 270;

        /// <summary>
        /// The empty space between gauges as a fraction of the gauge width.
        /// </summary>
        public double GaugeSpaceFraction = .5f;

        /// <summary>
        /// Size of the gague label text as a fraction of the gauge width.
        /// </summary>
        public double FontSize = .75;

        /// <summary>
        /// Describes labels drawn on each gauge.
        /// </summary>
        public readonly Drawing.Font Font = new() { Bold = true, Color = Color.White };

        /// <summary>
        /// Controls if value labels are shown inside the gauges.
        /// </summary>
        public bool ShowValueLabels { get; set; } = true;

        /// <summary>
        /// Style of the tip of the gauge
        /// </summary>
        public System.Drawing.Drawing2D.LineCap EndCap { get; set; } = System.Drawing.Drawing2D.LineCap.Triangle;

        /// <summary>
        /// Style of the base of the gauge
        /// </summary>
        public System.Drawing.Drawing2D.LineCap StartCap { get; set; } = System.Drawing.Drawing2D.LineCap.Round;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Initializes the plot instance.
        /// </summary>
        /// <param name="values">Values to be plotted as gauges.</param>
        /// <param name="colors">Gauge background colors.</param>
        public RadialGaugePlot(double[] values, Color[] colors)
        {
            Update(values, colors);
        }

        public override string ToString() => $"RadialGaugePlot with {GaugeCount} gauges.";

        /// <summary>
        /// Replace gauge levels and labels with new ones.
        /// </summary>
        public void Update(double[] values, Color[] colors = null)
        {
            if (values is null || values.Length == 0)
                throw new ArgumentException("values must not be null or empty");

            bool numberOfGroupsChanged = (Levels is null) || (values.Length != Levels.Length);
            if (numberOfGroupsChanged)
            {
                if (colors is null || colors.Length != values.Length)
                    throw new ArgumentException("when changing the number of values a new colors array must be provided");

                Colors = new Color[colors.Length];
                Array.Copy(colors, 0, Colors, 0, colors.Length);
            }

            Levels = new double[values.Length];
            Array.Copy(values, 0, Levels, 0, values.Length);
        }

        /// <summary>
        /// Calculates angular gauge positions from the raw data
        /// </summary>
        private static (double[] startAngles, double[] sweepAngles, double backStartAngle) ComputeAngularData(
            double[] values,
            double angleStart,
            double angleRange,
            bool clockwise,
            RadialGaugeMode mode)
        {
            (double scaleMin, double scaleMax) = ComputeMaxMin(values, mode);
            double scaleRange = scaleMax - scaleMin;

            int gaugeCount = values.Length;
            double[] startAngles = new double[gaugeCount];
            double[] sweepAngles = new double[gaugeCount];

            angleStart = ReduceAngle(angleStart);
            double angleSum = angleStart;
            for (int i = 0; i < gaugeCount; i++)
            {
                double angleSwept = angleRange * values[i] / scaleRange;
                if (!clockwise)
                    angleSwept *= -1;

                double initialAngle = (mode == RadialGaugeMode.Stacked) ? angleStart : angleSum;
                angleSum += angleSwept;

                startAngles[i] = initialAngle;
                sweepAngles[i] = angleSwept;
            }

            // Compute the initial angle for the background gauges
            double backOffset = angleRange * scaleMin / scaleRange;
            if (!clockwise)
                backOffset *= -1;

            double backStartAngle = angleStart + backOffset;

            return (startAngles, sweepAngles, backStartAngle);
        }

        /// <summary>
        /// Return the min/max values to use for the gauge with the given scale mode
        /// </summary>
        private static (double min, double max) ComputeMaxMin(double[] values, RadialGaugeMode mode)
        {
            if (mode == RadialGaugeMode.Sequential || mode == RadialGaugeMode.SingleGauge)
            {
                double max = values.Sum(x => Math.Abs(x));
                double min = 0;
                return (min, max);
            }
            else
            {
                double max = values.Max(x => Math.Abs(x));
                double min = Math.Min(0, values.Min());
                return (min, max);
            }
        }

        public void ValidateData(bool deep = false)
        {
            if (Colors.Length != GaugeCount)
                throw new InvalidOperationException($"{nameof(Colors)} must be an array with length equal to number of values");

            if (Labels != null && Labels.Length != GaugeCount)
                throw new InvalidOperationException($"If {nameof(Labels)} is not null, it must be the same length as the number of values");

            if (GaugeSize < 0 || GaugeSize > 360)
                throw new InvalidOperationException($"{nameof(GaugeSize)} must be [0-360]");

            if (GaugeLabelPosition < 0 || GaugeLabelPosition > 1)
                throw new InvalidOperationException($"{nameof(GaugeLabelPosition)} must be a value from 0 to 1");

            if (GaugeSpaceFraction < 0 || GaugeSpaceFraction > 1)
                throw new InvalidOperationException($"{nameof(GaugeSpaceFraction)} must be from 0 to 1");
        }

        public LegendItem[] GetLegendItems()
        {
            if (Labels is null)
                return null;

            List<LegendItem> legendItems = new();
            for (int i = 0; i < Labels.Length; i++)
            {
                var item = new LegendItem()
                {
                    label = Labels[i],
                    color = Colors[i],
                    lineWidth = 10,
                    markerShape = MarkerShape.none
                };
                legendItems.Add(item);
            }

            return legendItems.ToArray();
        }

        public AxisLimits GetAxisLimits()
        {
            double radius = GaugeCount / 4;
            return new AxisLimits(-radius, radius, -radius, radius);
        }

        /// <summary>
        /// Reduces an angle into the range [0°-360°].
        /// Angles greater than 360 will roll-over (370º becomes 10º).
        /// Angles less than 0 will roll-under (-10º becomes 350º).
        /// </summary>
        /// <param name="angle">Angle value</param>
        /// <returns>Angle whithin [0°-360°]</returns>
        private static double ReduceAngle(double angle)
        {
            angle %= 360;

            if (angle < 0)
                angle += 360;

            return angle;
        }

        /// <summary>
        /// This is where the drawing of the plot is done
        /// </summary>
        /// <param name="dims">Plot dimensions</param>
        /// <param name="bmp">Bitmap where the drawing is done</param>
        /// <param name="lowQuality">Image quality</param>
        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            // TODO: attribution / licensing
            // https://github.com/falahati/CircularProgressBar/blob/master/CircularProgressBar/CircularProgressBar.cs
            // https://github.com/aalitor/AltoControls/blob/on-development/AltoControls/Controls/Circular%20Progress%20Bar.cs

            (double[] startAngles, double[] sweepAngles, double StartingAngleBackGauges) = ComputeAngularData(
                values: Levels,
                angleStart: StartingAngleGauges,
                angleRange: GaugeSize,
                clockwise: Clockwise,
                mode: GaugeMode);

            float pxPerUnit = (float)Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);
            PointF origin = new(dims.GetPixelX(0), dims.GetPixelY(0));

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Pen pen = GDI.Pen(Color.Black);
            using Pen backgroundPen = GDI.Pen(Color.Black);
            using Brush labelBrush = GDI.Brush(Font.Color);

            float lineWidth = pxPerUnit / (GaugeCount * ((float)GaugeSpaceFraction + 1));
            float radiusSpace = lineWidth * ((float)GaugeSpaceFraction + 1);
            float gaugeRadius = GaugeCount * radiusSpace;  // By default, the outer-most radius is computed

            float maxBackAngle = CircularBackground ? 360 : (float)GaugeSize;
            if (!Clockwise)
                maxBackAngle = -maxBackAngle;

            pen.Width = (float)lineWidth;
            pen.StartCap = StartCap;
            pen.EndCap = EndCap;
            backgroundPen.Width = (float)lineWidth;
            backgroundPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            backgroundPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            using System.Drawing.Font fontGauge = new(Font.Name, lineWidth * (float)FontSize, FontStyle.Bold);

            // TODO: use this so font size is in pixels not pt
            //Font.Size = lineWidth * (float)FontSizeFraction;
            //using System.Drawing.Font fontGauge = GDI.Font(Font);

            int index;
            for (int i = 0; i < GaugeCount; i++)
            {
                // Data is reversed in case SingleGauge is selected
                // If OutsideToInside is selected, radius is reversed
                if (GaugeMode != RadialGaugeMode.SingleGauge)
                {
                    index = i;
                    gaugeRadius = (i + 1) * radiusSpace;
                }
                else
                {
                    index = GaugeCount - i - 1;
                }

                // Set color values
                pen.Color = Colors[index];
                int backgroundAlpha = (int)(255 * BackgroundTransparencyFraction);
                backgroundAlpha = Math.Max(0, backgroundAlpha);
                backgroundAlpha = Math.Min(255, backgroundAlpha);
                backgroundPen.Color = Color.FromArgb(backgroundAlpha, Colors[index]);

                // Draw gauge background
                if (GaugeMode != RadialGaugeMode.SingleGauge)
                    gfx.DrawArc(
                        backgroundPen,
                        (origin.X - gaugeRadius),
                        (origin.Y - gaugeRadius),
                        (gaugeRadius * 2),
                        (gaugeRadius * 2),
                        (float)StartingAngleBackGauges,
                        maxBackAngle);

                // Draw gauge
                gfx.DrawArc(
                    pen,
                    (origin.X - gaugeRadius),
                    (origin.Y - gaugeRadius),
                    (gaugeRadius * 2),
                    (gaugeRadius * 2),
                    (float)startAngles[index],
                    (float)sweepAngles[index]);

                // Draw gauge labels
                if (ShowValueLabels)
                {
                    DrawTextOnCircle(gfx,
                        fontGauge,
                        labelBrush,
                        new RectangleF(dims.DataOffsetX, dims.DataOffsetY, dims.DataWidth, dims.DataHeight),
                        gaugeRadius,
                        (float)startAngles[index],
                        (float)sweepAngles[index],
                        origin.X,
                        origin.Y,
                        Levels[index].ToString("0.##"),    // Could be set as a user-choice paremeter?
                        GaugeLabelPosition);
                }

            }

        }

        /// <summary>
        /// Draw text centered on the top and the bottom of the circle.
        /// </summary>
        /// <param name="gfx"><see langword="keyword">Graphic</see> object used to draw</param>
        /// <param name="font"><see langword="keyword">Font</see> used to draw the text</param>
        /// <param name="brush"><see langword="keyword">Brush</see> used to draw the text</param>
        /// <param name="clientRectangle"><see langword="keyword">Rectangle</see> of the ScottPlot control</param>
        /// <param name="radius">Radius of the circle in pixels</param>
        /// <param name="start">Staring angle (in degrees) of the circle</param>
        /// <param name="sweep">Angular span (in degrees) of the circle</param>
        /// <param name="cx">The x-coordinate of the circle centre</param>
        /// <param name="cy">The y-coordinate of the circle centre</param>
        /// <param name="text">String to be drawn</param>
        /// <param name="positionFraction">Position of text as a fraction of the angle swept</param>
        /// <seealso href="http://csharphelper.com/blog/2018/02/draw-text-on-a-circle-in-c/"/>
        private void DrawTextOnCircle(Graphics gfx, System.Drawing.Font font,
            Brush brush, RectangleF clientRectangle, float radius, float start, float sweep, float cx, float cy,
            string text, double positionFraction)
        {
            // TODO: move to Drawing module.

            // TODO: verify source / license / attribution.
            // Text on path
            // http://csharphelper.com/blog/2018/02/draw-text-on-a-circle-in-c/
            // http://csharphelper.com/blog/2016/01/draw-text-on-a-curve-in-c/

            // Modify anglePos to be in the range [0, 360]
            if (start >= 0)
                start -= 360f * (int)(start / 360);
            else
                start += 360f;

            // Use a StringFormat to draw the middle top of each character at (0, 0).
            using StringFormat string_format = new StringFormat();
            string_format.Alignment = StringAlignment.Center;
            string_format.LineAlignment = StringAlignment.Center;

            // Used to scale from radians to degrees.
            double RadToDeg = 180.0 / Math.PI;
            double width_to_angle = 1 / radius;

            // Measure the characters. Use LINQ to add up the character widths.
            List<RectangleF> rects = MeasureCharacters(gfx, font, clientRectangle, text);
            double text_width = (rects.Select(rect => rect.Width)).Sum() / radius;

            // Angular data
            bool isPositive = sweep >= 0;
            double angle = ReduceAngle(start + sweep * positionFraction);
            angle += (1 - 2 * positionFraction) * (isPositive ? 1 : -1) * RadToDeg * text_width / 2; // Set the position to the middle of the text

            bool isBelow = angle < 180 && angle > 0;
            double theta = angle * Math.PI / 180;
            theta += (isBelow ? 1 : -1) * text_width / 2;   // Set the position to the beginning of the text

            // Draw the characters.
            for (int i = 0; i < text.Length; i++)
            {
                // Increment theta half the angular width of the current character
                theta -= (isBelow ? 1 : -1) * rects[i].Width / 2 * width_to_angle;

                // Calculate the position of the upper-left corner
                float x = cx + radius * (float)Math.Cos(theta);
                float y = cy + radius * (float)Math.Sin(theta);

                // Transform to position the character.
                double halfPi = Math.PI / 2;
                double rotationRadians = isBelow ? theta - halfPi : theta + halfPi;
                double rotationDegrees = rotationRadians * RadToDeg;

                gfx.RotateTransform((float)rotationDegrees);
                gfx.TranslateTransform(x, y, System.Drawing.Drawing2D.MatrixOrder.Append);
                gfx.DrawString(text[i].ToString(), font, brush, 0, 0, string_format);
                gfx.ResetTransform();

                // Increment theta the remaining half character.
                theta -= (isBelow ? 1 : -1) * rects[i].Width / 2 * width_to_angle;
            }
        }

        /// <summary>
        /// Measure the characters in a string with no more than 32 characters.
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="font"></param>
        /// <param name="clientRectangle"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<RectangleF> MeasureCharactersInWord(Graphics gfx, System.Drawing.Font font, RectangleF clientRectangle, string text)
        {
            // TODO: verify source / license / attribution.
            // TODO: move to Drawing module.

            List<RectangleF> result = new();

            using StringFormat string_format = new();

            string_format.Alignment = StringAlignment.Center;
            string_format.LineAlignment = StringAlignment.Center;
            string_format.Trimming = StringTrimming.None;
            string_format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            CharacterRange[] ranges = new CharacterRange[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                ranges[i] = new CharacterRange(i, 1);
            }
            string_format.SetMeasurableCharacterRanges(ranges);

            // Find the character ranges.
            RectangleF rect = new RectangleF(0, 0, 10000, 100);
            Region[] regions =
                gfx.MeasureCharacterRanges(
                    text, font, clientRectangle,
                    string_format);

            // Convert the regions into rectangles.
            foreach (Region region in regions)
                result.Add(region.GetBounds(gfx));

            return result;
        }

        /// <summary>
        /// Measure the characters in the string.
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="font"></param>
        /// <param name="clientRectangle"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<RectangleF> MeasureCharacters(Graphics gfx, System.Drawing.Font font, RectangleF clientRectangle, string text)
        {
            // TODO: verify source / license / attribution.
            // TODO: move to Drawing module.

            List<RectangleF> results = new List<RectangleF>();

            // The X location for the next character.
            float x = 0;

            // Get the character sizes 31 characters at a time.
            for (int start = 0; start < text.Length; start += 32)
            {
                // Get the substring.
                int len = 32;
                if (start + len >= text.Length) len = text.Length - start;
                string substring = text.Substring(start, len);

                // Measure the characters.
                List<RectangleF> rects =
                    MeasureCharactersInWord(gfx, font, clientRectangle, substring);

                // Remove lead-in for the first character.
                if (start == 0)
                    x += rects[0].Left;

                // Save all but the last rectangle.
                for (int i = 0; i < rects.Count + 1 - 1; i++)
                {
                    RectangleF new_rect = new RectangleF(
                        x, rects[i].Top,
                        rects[i].Width, rects[i].Height);
                    results.Add(new_rect);

                    // Move to the next character's X position.
                    x += rects[i].Width;
                }
            }

            // Return the results.
            return results;
        }
    }
}
