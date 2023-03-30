using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

// This plot type was inspired by MicroCharts:
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
        /// This array holds the original levels passed-in by the user. 
        /// These levels are used to calculate radial gauge positions on every render.
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
        public double MaximumAngle { get; set; } = 360;

        /// <summary>
        /// Controls whether the backgrounds of the gauges are full circles or stop at the maximum angle.
        /// </summary>
        public bool CircularBackground { get; set; } = true;

        /// <summary>
        /// Labels that appear in the legend for each gauge.
        /// Number of labels must equal number of gauges.
        /// May be null if gauges are not to appear in the legend.
        /// </summary>
        public string[] Labels { get; set; }

        /// <summary>
        /// Colors for each gauge.
        /// Number of colors must equal number of gauges.
        /// </summary>
        public Color[] Colors { get; set; }

        /// <summary>
        /// Describes how transparent the unfilled background of each gauge is (0 to 1).
        /// The larger the number the darker the background becomes.
        /// </summary>
        public double BackgroundTransparencyFraction { get; set; } = .15;

        /// <summary>
        /// Indicates whether gauges fill clockwise as levels increase.
        /// If false, gauges will fill counter-clockwise (anti-clockwise).
        /// </summary>
        public bool Clockwise { get; set; } = true;

        /// <summary>
        /// Determines whether the gauges are drawn stacked (dafault value), sequentially, or as a single gauge (ressembling a pie plot).
        /// </summary>
        public RadialGaugeMode GaugeMode { get; set; } = RadialGaugeMode.Stacked;

        /// <summary>
        /// Controls whether gauges will be dwan inside-out (true) or outside-in (false)
        /// </summary>
        public bool OrderInsideOut { get; set; } = true;

        /// <summary>
        /// Defines where the gauge label is written on the gage as a fraction of its length.
        /// Low values place the label near the base and high values place the label at its tip.
        /// </summary>
        public double LabelPositionFraction { get; set; } = 1;

        /// <summary>
        /// Angle (degrees) at which the gauges start.
        /// 270° for North (default value), 0° for East, 90° for South, 180° for West, etc.
        /// Expected values in the range [0°-360°], otherwise unexpected side-effects might happen.
        /// </summary>
        public float StartingAngle { get; set; } = 270;

        /// <summary>
        /// The empty space between gauges as a fraction of the gauge width.
        /// </summary>
        public double SpaceFraction { get; set; } = .5f;

        /// <summary>
        /// Size of the gague label text as a fraction of the gauge width.
        /// </summary>
        public double FontSizeFraction { get; set; } = .75;

        /// <summary>
        /// Describes labels drawn on each gauge.
        /// </summary>
        public readonly Drawing.Font Font = new() { Bold = true, Color = Color.White };

        /// <summary>
        /// Controls if value labels are shown inside the gauges.
        /// </summary>
        public bool ShowLevels { get; set; } = true;

        /// <summary>
        /// String formatter to use for converting gauge levels to text
        /// </summary>
        public string LevelTextFormat { get; set; } = "0.##";

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

        public RadialGaugePlot(double[] levels, Color[] colors)
        {
            Update(levels, colors);
        }

        public override string ToString() => $"RadialGaugePlot with {GaugeCount} gauges.";

        /// <summary>
        /// Replace gauge levels with new ones.
        /// </summary>
        public void Update(double[] levels, Color[] colors = null)
        {
            if (levels is null || levels.Length == 0)
                throw new ArgumentException("values must not be null or empty");

            bool numberOfGroupsChanged = (Levels is null) || (levels.Length != Levels.Length);
            if (numberOfGroupsChanged)
            {
                if (colors is null || colors.Length != levels.Length)
                    throw new ArgumentException("when changing the number of values a new colors array must be provided");

                Colors = new Color[colors.Length];
                Array.Copy(colors, 0, Colors, 0, colors.Length);
            }

            Levels = levels;
        }

        /// <summary>
        /// Calculate the rotational angles for each gauge from the original data values
        /// </summary>
        private static (double[] startAngles, double[] sweepAngles, double backStartAngle) GetGaugeAngles(
            double[] values,
            double angleStart,
            double angleRange,
            bool clockwise,
            RadialGaugeMode mode)
        {
            double scaleMin = Math.Min(0, values.Min());
            double scaleMax = values.Max(x => Math.Abs(x));

            if (mode == RadialGaugeMode.Sequential || mode == RadialGaugeMode.SingleGauge)
            {
                scaleMax = values.Sum(x => Math.Abs(x));
                scaleMin = 0;
            }

            double scaleRange = scaleMax - scaleMin;

            int gaugeCount = values.Length;
            double[] startAngles = new double[gaugeCount];
            double[] sweepAngles = new double[gaugeCount];

            angleStart = RadialGauge.ReduceAngle(angleStart);
            double angleSum = angleStart;
            for (int i = 0; i < gaugeCount; i++)
            {
                double angleSwept = 0;
                if (scaleRange > 0)
                    angleSwept = angleRange * values[i] / scaleRange;

                if (!clockwise)
                    angleSwept *= -1;

                double initialAngle = (mode == RadialGaugeMode.Stacked) ? angleStart : angleSum;
                angleSum += angleSwept;

                startAngles[i] = initialAngle;
                sweepAngles[i] = angleSwept;
            }

            double backOffset = angleRange * scaleMin / scaleRange;
            if (!clockwise)
                backOffset *= -1;

            double backStartAngle = angleStart + backOffset;

            return (startAngles, sweepAngles, backStartAngle);
        }

        public void ValidateData(bool deep = false)
        {
            if (Colors.Length != GaugeCount)
                throw new InvalidOperationException($"{nameof(Colors)} must be an array with length equal to number of values");

            if (Labels != null && Labels.Length != GaugeCount)
                throw new InvalidOperationException($"If {nameof(Labels)} is not null, it must be the same length as the number of values");

            if (MaximumAngle < 0 || MaximumAngle > 360)
                throw new InvalidOperationException($"{nameof(MaximumAngle)} must be [0-360]");

            if (LabelPositionFraction < 0 || LabelPositionFraction > 1)
                throw new InvalidOperationException($"{nameof(LabelPositionFraction)} must be a value from 0 to 1");

            if (SpaceFraction < 0 || SpaceFraction > 1)
                throw new InvalidOperationException($"{nameof(SpaceFraction)} must be from 0 to 1");
        }

        public LegendItem[] GetLegendItems()
        {
            if (Labels is null)
                return LegendItem.None;

            List<LegendItem> legendItems = new();
            for (int i = 0; i < Labels.Length; i++)
            {
                var item = new LegendItem(this)
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

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            ValidateData();

            (double[] startAngles, double[] sweepAngles, double StartingAngleBackGauges) = GetGaugeAngles(
                values: Levels,
                angleStart: StartingAngle,
                angleRange: MaximumAngle,
                clockwise: Clockwise,
                mode: GaugeMode);

            PointF centerPixel = new(dims.GetPixelX(0), dims.GetPixelY(0));
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);

            float pxPerUnit = (float)Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);

            if (GaugeCount == 3)
                pxPerUnit /= 2.1f;
            else if (GaugeCount == 2)
                pxPerUnit /= 2.2f;
            else if (GaugeCount == 1)
                pxPerUnit /= 2.5f;

            float gaugeWidthPx = pxPerUnit / (GaugeCount * ((float)SpaceFraction + 1));
            float radiusPixels = gaugeWidthPx * ((float)SpaceFraction + 1);

            int backgroundAlpha = (int)(255 * BackgroundTransparencyFraction);
            backgroundAlpha = Math.Max(0, backgroundAlpha);
            backgroundAlpha = Math.Min(255, backgroundAlpha);

            int index;
            int position;
            for (int i = 0; i < GaugeCount; i++)
            {
                if (GaugeMode == RadialGaugeMode.SingleGauge)
                {
                    index = GaugeCount - i - 1;
                    position = GaugeCount;
                }
                else
                {
                    index = i;
                    position = OrderInsideOut ? i + 1 : (GaugeCount - i);
                }

                RadialGauge gauge = new()
                {
                    MaximumSizeAngle = MaximumAngle,
                    StartAngle = startAngles[index],
                    SweepAngle = sweepAngles[index],
                    Color = Colors[index],
                    BackgroundColor = Color.FromArgb(backgroundAlpha, Colors[index]),
                    Width = gaugeWidthPx,
                    CircularBackground = CircularBackground,
                    Clockwise = Clockwise,
                    BackStartAngle = StartingAngleBackGauges,
                    StartCap = StartCap,
                    EndCap = EndCap,
                    Mode = GaugeMode,
                    Font = Font,
                    FontSizeFraction = FontSizeFraction,
                    Label = Levels[index].ToString(LevelTextFormat),
                    LabelPositionFraction = LabelPositionFraction,
                    ShowLabels = ShowLevels,
                };

                float radiusPx = position * radiusPixels;
                gauge.Render(gfx, dims, centerPixel, radiusPx);
            }
        }
    }
}
