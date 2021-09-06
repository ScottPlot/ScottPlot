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
    /// 
    /// Data is managed using a single array where each element is asigned to each gauge.
    /// Internally this data is stored in a single array and is converted to angular paramters,
    /// through ComputeAngularData(), which are more suitable for drawing purposes and stored in a 2D array.
    /// </summary>
    public class RadialGaugePlot : IPlottable
    {
        /// <summary>
        /// Scalar data values (arbitrary units) used to create the angular values.
        /// This array is copied from the input given during construction or Update().
        /// </summary>
        public double[] DataRaw { get; private set; }

        /// <summary>
        /// Angular data calculated from DataRaw according to RadialGaugeMode.
        /// Columns represent initial and swept angles.
        /// </summary>
        private double[,] DataAngular;

        /// <summary>
        /// The maximum value (in arbitrary data units) used for scaling the gauges
        /// </summary>
        private double MaxScale;

        /// <summary>
        /// The minimum value (in arbitrary data units) used for scaling the gauges
        /// </summary>
        private double MinScale;

        /// <summary>
        /// The maximum angle for the gauges (default is 360)
        /// </summary>
        public double AngleRange
        {
            set
            {
                if (value < 0 || value > 360)
                    throw new InvalidOperationException("Maximum angle must be [0-360]");
                _AngleRange = value;
                ComputeAngularData();
            }
            get => _AngleRange;

        }
        private double _AngleRange = 360;

        /// <summary>
        /// Labels for each gauge.
        /// Array must contain same number of elements as the original data.
        /// </summary>
        public string[] GaugeLabels; // TODO: make this private and accept it as an argument in the contstructor and Update?

        /// <summary>
        /// Colors for each gauge (optionally dimmed at render time). 
        /// Array must contain same number of elements as the original data.
        /// </summary>
        public Color[] GaugeColors; // TODO: make this private and accept it as an argument in the contstructor and Update?

        /// <summary>
        /// Describes how intense the color of each gauge is (0 to 1).
        /// Low values are white-washed, high values are intense colors.
        /// </summary>
        public double ColorDimFraction
        {
            get => _ColorDimFraction;
            set
            {
                if (value < 0 || value > 1)
                    throw new InvalidOperationException("fraction must be 0 to 1");
                _ColorDimFraction = value;
            }
        }
        private double _ColorDimFraction = .9;

        /// <summary>
        /// Determines whether the gauges are drawn clockwise (default value) or anti-clockwise (counter clockwise).
        /// </summary>
        public RadialGaugeDirection GaugeDirection
        {
            get => _GaugeDirection;
            set
            {
                _GaugeDirection = value;
                ComputeAngularData();
            }
        }
        private RadialGaugeDirection _GaugeDirection = RadialGaugeDirection.Clockwise;

        /// <summary>
        /// Determines whether the gauges are drawn stacked (dafault value), sequentially, or as a single gauge (ressembling a pie plot).
        /// </summary>
        public RadialGaugeMode GaugeMode
        {
            get => _GaugeMode;
            set
            {
                _GaugeMode = value;
                ComputeMaxMin();
                ComputeAngularData();
            }
        }
        private RadialGaugeMode _GaugeMode = RadialGaugeMode.Stacked;

        /// <summary>
        /// Defines where the gauge label is written on the gage as a fraction of its length.
        /// Low values place the label near the base and high values place the label at its tip.
        /// </summary>
        public double GaugeLabelPosition
        {
            get => _GaugeLabelPosition;
            set
            {
                if (value < 0 || value > 1)
                    throw new InvalidOperationException("position must be a value from 0 to 1");
                _GaugeLabelPosition = value;
            }
        }
        private double _GaugeLabelPosition = 1;

        /// <summary>
        /// <see langword="True"/> if the gauges' background is adjusted to <see cref="StartingAngleGauges"/>.
        /// Default value is set to <see langword="False"/>.
        /// </summary>
        public bool NormBackGauge { get; set; } = false;

        /// <summary>
        /// Angle (in degrees) at which the gauges start: 270° for North (default value), 0° for East, 90° for South, 180° for West, and so on.
        /// Expected values in the range [0°-360°], otherwise unexpected side-effects might happen.
        /// </summary>
        public float StartingAngleGauges
        {
            get => _StartingAngleGauges;
            set
            {
                _StartingAngleGauges = (float)ReduceAngle(value);
                ComputeAngularData();
            }
        }
        private float _StartingAngleGauges = 270f;

        /// <summary>
        /// The initial angle (in degrees) where the background gauges begin. Default value is 270° the same as <see cref="StartingAngleGauges"/>.
        /// </summary>
        private float StartingAngleBackGauges = 270f;

        /// <summary>
        /// The empty space between gauges as a percentage of the gauge width.
        /// Values in the range [0-100], default value is 50 [percent]. Other values might produce unexpected side-effects.
        /// </summary>
        public double GaugeSpaceFraction
        {
            get => _GaugeSpaceFraction;
            set
            {
                if (value < 0 || value > 1)
                    throw new InvalidOperationException("fraction must be from 0 to 1");
                _GaugeSpaceFraction = value;
            }
        }
        private double _GaugeSpaceFraction = .5f;

        /// <summary>
        /// <see langword="Color"/> of the value labels drawn inside the gauges.
        /// </summary>
        public Color GaugeLabelsColor { get; set; } = Color.White; // TODO: use an array here?

        /// <summary>
        /// Size of the gague label text as a fraction of the gauge width.
        /// </summary>
        public double FontSizeFraction = .75;

        /// <summary>
        /// <see langword="Font"/> used for labeling values on the plot
        /// </summary>
        public Drawing.Font Font { get; set; } = new() { Bold = true };

        /// <summary>
        /// <see langword="True"/> if value labels are shown inside the gauges.
        /// Size of the text is set by <see cref="FontSizeFraction"/> and color by <see cref="GaugeLabelsColor"/>.
        /// </summary>
        public bool ShowGaugeValues { get; set; } = true;

        public System.Drawing.Drawing2D.LineCap EndCap { get; set; } = System.Drawing.Drawing2D.LineCap.Triangle;

        public System.Drawing.Drawing2D.LineCap StartCap { get; set; } = System.Drawing.Drawing2D.LineCap.Round;


        // These 3 properties are needed as part of IPlottable
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Initializes the plot instance.
        /// </summary>
        /// <param name="values">Array of values to be plotted as gauges.</param>
        /// <param name="lineColors">Array colors for the gauges.</param>
        public RadialGaugePlot(double[] values, Color[] lineColors)
        {
            GaugeColors = lineColors;
            Update(values);
        }

        public override string ToString() =>
            $"RadialGauge with {DataRaw.Length} points.";

        /// <summary>
        /// Replace the data values with new ones. This passed data is copied and stored in <see cref="DataRaw"/>.
        /// Implicitly calls the <see cref="ComputeAngularData"/> routine
        /// </summary>
        /// <param name="values">Array of values to be plotted as gauges.</param>
        public void Update(double[] values)
        {
            DataRaw = new double[values.Length];
            Array.Copy(values, 0, DataRaw, 0, values.Length);

            // Compute MaxScale and MinScale values and transform DataRaw into DataAngular
            ComputeMaxMin();
            ComputeAngularData();
        }

        /// <summary>
        /// Returns the original data in case it should be needed.
        /// </summary>
        /// <returns>Original data vector</returns>
        public double[] GetData() => DataRaw;

        /// <summary>
        /// Returns the computed angular data in case it should be needed.
        /// </summary>
        /// <returns>Angular data vector</returns>
        public double[,] GetAngularData() => DataAngular;

        /// <summary>
        /// Converts <see cref="DataRaw"/> into <see cref="DataAngular"/>.
        /// Depends on <see cref="DataRaw"/>, <see cref="GaugeMode"/>, <see cref="GaugeDirection"/>, <see cref="StartingAngleGauges"/>, <see cref="AngleRange"/>, and <see cref="MaxScale"/>
        /// </summary>
        private void ComputeAngularData()
        {
            // Check if there's data
            if (DataRaw == null) return;
            DataAngular = new double[DataRaw.Length, 2];

            // Internal variables
            float angleSum = StartingAngleGauges;

            // Loop through DataRaw and compute DataAngular
            for (int i = 0; i < DataRaw.Length; i++)
            {
                float angleSwept = (float)(AngleRange * DataRaw[i] / (MaxScale - MinScale));
                if (GaugeDirection == RadialGaugeDirection.AntiClockwise)
                    angleSwept *= -1;

                double initialAngle = GaugeMode == RadialGaugeMode.Stacked ? StartingAngleGauges : angleSum;
                angleSum += angleSwept;

                DataAngular[i, 0] = initialAngle;
                DataAngular[i, 1] = angleSwept;
            }

            // Compute the initial angle for the background gauges
            StartingAngleBackGauges = StartingAngleGauges;
            StartingAngleBackGauges += (GaugeDirection == RadialGaugeDirection.AntiClockwise ? -1 : 1) * (float)(AngleRange * MinScale / (MaxScale - MinScale));
        }

        /// <summary>
        /// Sets the value of <see cref="MaxScale"/> and <see cref="MinScale"/> properties
        /// </summary>
        private void ComputeMaxMin()
        {
            if (GaugeMode == RadialGaugeMode.Sequential || GaugeMode == RadialGaugeMode.SingleGauge)
            {
                MaxScale = DataRaw.Sum(x => Math.Abs(x));
                MinScale = 0;
            }
            else
            {
                MaxScale = DataRaw.Max(x => Math.Abs(x));
                var min = DataRaw.Min();
                MinScale = min < 0 ? min : 0;
            }
        }

        /// <summary>
        /// Needed as part of IPlottable in ScottPlot.ScottForm
        /// </summary>
        /// <param name="deep"></param>
        public void ValidateData(bool deep = false)
        {
            if (GaugeLabels != null && GaugeLabels.Length != DataRaw.Length)
                throw new InvalidOperationException("Gauge labels must match size of data values");
        }

        public LegendItem[] GetLegendItems()
        {
            if (GaugeLabels is null)
                return null;

            List<LegendItem> legendItems = new List<LegendItem>();
            for (int i = 0; i < GaugeLabels.Length; i++)
            {
                var item = new LegendItem()
                {
                    label = GaugeLabels[i],
                    color = GaugeColors[i],
                    lineWidth = 10,
                    markerShape = MarkerShape.none
                };
                legendItems.Add(item);
            }

            return legendItems.ToArray();
        }

        /// <summary>
        /// Needed as part of IPlottable in ScottPlot.ScottForm
        /// </summary>
        /// <returns></returns>
        public AxisLimits GetAxisLimits()
        {
            // TODO: update this to depend on line width and number of groups
            if (GaugeLabels is null)
                return new AxisLimits(-2.5, 2.5, -2.5, 2.5);
            else
                return new AxisLimits(-3.5, 3.5, -3.5, 3.5);
        }

        /// <summary>
        /// Reduces an angle into the range [0°-360°].
        /// Angles greater than 360 will roll-over (370º becomes 10º).
        /// Angles less than 0 will roll-under (-10º becomes 350º).
        /// </summary>
        /// <param name="angle">Angle value</param>
        /// <returns>Angle whithin [0°-360°]</returns>
        private double ReduceAngle(double angle)
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

            int numGroups = DataRaw.Length;
            float pxPerUnit = (float)Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);
            PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Pen pen = GDI.Pen(Color.Black);
            using Pen penCircle = GDI.Pen(Color.Black);
            using Brush labelBrush = GDI.Brush(GaugeLabelsColor);

            float lineWidth = pxPerUnit / (numGroups * ((float)GaugeSpaceFraction + 1));
            float radiusSpace = lineWidth * ((float)GaugeSpaceFraction + 1);
            float gaugeRadius = numGroups * radiusSpace;  // By default, the outer-most radius is computed
            float maxBackAngle = (GaugeDirection == RadialGaugeDirection.AntiClockwise ? -1 : 1) * (NormBackGauge ? (float)AngleRange : 360);

            pen.Width = (float)lineWidth;
            pen.StartCap = StartCap;
            pen.EndCap = EndCap;
            penCircle.Width = (float)lineWidth;
            penCircle.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            penCircle.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            using System.Drawing.Font fontGauge = new(Font.Name, lineWidth * (float)FontSizeFraction, FontStyle.Bold);

            // TODO: use this so font size is in pixels not pt
            //Font.Size = lineWidth * (float)FontSizeFraction;
            //using System.Drawing.Font fontGauge = GDI.Font(Font);

            int index;
            for (int i = 0; i < numGroups; i++)
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
                    index = numGroups - i - 1;
                }

                // Set color values
                pen.Color = GaugeColors[index];
                penCircle.Color = ChangeColorBrightness(GaugeColors[index], (float)ColorDimFraction);

                // Draw gauge background
                if (GaugeMode != RadialGaugeMode.SingleGauge)
                    gfx.DrawArc(
                        penCircle,
                        (origin.X - gaugeRadius),
                        (origin.Y - gaugeRadius),
                        (gaugeRadius * 2),
                        (gaugeRadius * 2),
                        StartingAngleBackGauges,
                        maxBackAngle);

                // Draw gauge
                gfx.DrawArc(
                    pen,
                    (origin.X - gaugeRadius),
                    (origin.Y - gaugeRadius),
                    (gaugeRadius * 2),
                    (gaugeRadius * 2),
                    (float)DataAngular[index, 0],
                    (float)DataAngular[index, 1]);

                // Draw gauge labels
                if (ShowGaugeValues)
                {
                    DrawTextOnCircle(gfx,
                        fontGauge,
                        labelBrush,
                        new RectangleF(dims.DataOffsetX, dims.DataOffsetY, dims.DataWidth, dims.DataHeight),
                        gaugeRadius,
                        (float)DataAngular[index, 0],
                        (float)DataAngular[index, 1],
                        origin.X,
                        origin.Y,
                        DataRaw[index].ToString("0.##"),    // Could be set as a user-choice paremeter?
                        GaugeLabelPosition);
                }

            }

        }

        /// <summary>Creates color with corrected brightness.</summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>Corrected <see cref="Color"/> structure.</returns>
        /// <seealso href="http://gist.github.com/zihotki/09fc41d52981fb6f93a81ebf20b35cd5"/>
        private Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            // TODO: consider replacing this with a simple alpha modulation.

            // TODO: ensure proper licensing/attribution for this method.
            // https://stackoverflow.com/questions/801406/c-create-a-lighter-darker-color-based-on-a-system-color
            // https://www.pvladov.com/2012/09/make-color-lighter-or-darker.html
            // https://gist.github.com/zihotki/09fc41d52981fb6f93a81ebf20b35cd5

            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = correctionFactor < -1 ? 0 : 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                if (correctionFactor > 1) correctionFactor = 1;
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            // TODO: this may throw if <0 or >255
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
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
