using ScottPlot.Drawing;
using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks
{
    public enum TickLabelFormat { Numeric, DateTime }; // TODO: add hex, binary, scientific notation, etc?
    public enum AxisOrientation { Vertical, Horizontal };
    public enum MinorTickDistribution { even, log };

    public class TickCollection
    {
        // This class creates pretty tick labels (with offset and exponent) uses graph settings
        // to inspect the tick font and ensure tick labels will not overlap.
        // It also respects manually defined tick spacing settings set via plt.Grid().

        // TODO: store these in a class
        public double[] tickPositionsMajor;
        public double[] tickPositionsMinor;
        public string[] tickLabels;

        // When populated, manual ticks are the ONLY ticks shown
        public double[] manualTickPositions;
        public string[] manualTickLabels;

        // When populated, additionalTicks are shown in addition to automatic ticks
        public double[] additionalTickPositions;
        public string[] additionalTickLabels;

        /// <summary>
        /// Controls how to translate exponential part of a number to strings
        /// </summary>
        public string CornerLabelFormat { get; set; } = "E{0}";

        /// <summary>
        /// Label to show in the corner when using multiplier or offset notation
        /// </summary>
        public string CornerLabel { get; private set; }

        /// <summary>
        /// Measured size of the largest tick label
        /// </summary>
        public float LargestLabelWidth { get; private set; } = 15;

        /// <summary>
        /// Measured size of the largest tick label
        /// </summary>
        public float LargestLabelHeight { get; private set; } = 12;

        /// <summary>
        /// Controls how to translate positions to strings
        /// </summary>
        public TickLabelFormat LabelFormat = TickLabelFormat.Numeric;

        /// <summary>
        /// If True, these ticks are placed along a vertical (Y) axis.
        /// This is used to determine whether tick density should be based on tick label width or height.
        /// </summary>
        public AxisOrientation Orientation;

        /// <summary>
        /// If True, the sign of numeric tick labels will be inverted.
        /// This is used to give the appearance of descending ticks.
        /// </summary>
        public bool LabelUsingInvertedSign;

        /// <summary>
        /// Define how minor ticks are distributed (evenly vs. log scale)
        /// </summary>
        public MinorTickDistribution MinorTickDistribution;

        public string numericFormatString;
        public string dateTimeFormatString;

        /// <summary>
        /// If defined, this function will be used to generate tick labels from positions
        /// </summary>
        public Func<double, string> ManualTickFormatter = null;

        public int radix = 10;
        public string prefix = null;

        public double manualSpacingX = 0;
        public double manualSpacingY = 0;
        public Ticks.DateTimeUnit? manualDateTimeSpacingUnitX = null;
        public Ticks.DateTimeUnit? manualDateTimeSpacingUnitY = null;

        public CultureInfo Culture = CultureInfo.DefaultThreadCurrentCulture;

        public bool useMultiplierNotation = false;
        public bool useOffsetNotation = false;
        public bool useExponentialNotation = true;

        /// <summary>
        /// Optimally packed tick labels have a density 1.0 and lower densities space ticks farther apart.
        /// </summary>
        public float TickDensity = 1.0f;

        /// <summary>
        /// Defines the minimum distance (in coordinate units) for major ticks.
        /// </summary>
        public double MinimumTickSpacing = 0;

        /// <summary>
        /// If True, non-integer tick positions will not be used.
        /// This may be desired for log10-scaled axes so tick marks are even powers of 10.
        /// </summary>
        public bool IntegerPositionsOnly = false;

        /// <summary>
        /// If minor tick distribution is log-scaled, place this many minor ticks
        /// </summary>
        public int LogScaleMinorTickCount = 10;

        /// <summary>
        /// Number of minor ticks per major tick
        /// </summary>
        public int MinorTickCount = 5;

        /// <summary>
        /// Determine tick density using a fixed formula to estimate label size instead of MeasureString().
        /// This is less accurate, but is consistent across operating systems, and is independent of font.
        /// </summary>
        public bool MeasureStringManually = false;

        public void Recalculate(PlotDimensions dims, Drawing.Font tickFont)
        {
            if (manualTickPositions is null)
            {
                // first pass uses forced density with manual label sizes to consistently approximate labels
                if (LabelFormat == TickLabelFormat.DateTime)
                    RecalculatePositionsAutomaticDatetime(dims, 20, 24, (int)(10 * TickDensity));
                else
                    RecalculatePositionsAutomaticNumeric(dims, 15, 12, (int)(10 * TickDensity));

                // second pass calculates density using measured labels produced by the first pass
                (LargestLabelWidth, LargestLabelHeight) = MaxLabelSize(tickFont);
                if (LabelFormat == TickLabelFormat.DateTime)
                    RecalculatePositionsAutomaticDatetime(dims, LargestLabelWidth, LargestLabelHeight, null);
                else
                    RecalculatePositionsAutomaticNumeric(dims, LargestLabelWidth, LargestLabelHeight, null);
            }
            else
            {
                if (manualTickPositions.Length != manualTickLabels.Length)
                    throw new InvalidOperationException($"{nameof(manualTickPositions)} must have the same length as {nameof(manualTickLabels)}");

                double min = Orientation == AxisOrientation.Vertical ? dims.YMin : dims.XMin;
                double max = Orientation == AxisOrientation.Vertical ? dims.YMax : dims.XMax;

                var visibleIndexes = Enumerable.Range(0, manualTickPositions.Count())
                    .Where(i => manualTickPositions[i] >= min)
                    .Where(i => manualTickPositions[i] <= max);

                tickPositionsMajor = visibleIndexes.Select(x => manualTickPositions[x]).ToArray();
                tickPositionsMinor = null;
                tickLabels = visibleIndexes.Select(x => manualTickLabels[x]).ToArray();
                CornerLabel = null;
                (LargestLabelWidth, LargestLabelHeight) = MaxLabelSize(tickFont);
            }
        }

        public void SetCulture(
            string shortDatePattern = null,
            string decimalSeparator = null,
            string numberGroupSeparator = null,
            int? decimalDigits = null,
            int? numberNegativePattern = null,
            int[] numberGroupSizes = null
            )
        {
            // Culture may be null if the thread culture is the same is the system culture.
            // If it is null, assigning it to a clone of the current culture solves this and also makes it mutable.
            Culture = Culture ?? (CultureInfo)CultureInfo.CurrentCulture.Clone();
            Culture.DateTimeFormat.ShortDatePattern = shortDatePattern ?? Culture.DateTimeFormat.ShortDatePattern;
            Culture.NumberFormat.NumberDecimalDigits = decimalDigits ?? Culture.NumberFormat.NumberDecimalDigits;
            Culture.NumberFormat.NumberDecimalSeparator = decimalSeparator ?? Culture.NumberFormat.NumberDecimalSeparator;
            Culture.NumberFormat.NumberGroupSeparator = numberGroupSeparator ?? Culture.NumberFormat.NumberGroupSeparator;
            Culture.NumberFormat.NumberGroupSizes = numberGroupSizes ?? Culture.NumberFormat.NumberGroupSizes;
            Culture.NumberFormat.NumberNegativePattern = numberNegativePattern ?? Culture.NumberFormat.NumberNegativePattern;
        }

        private (float width, float height) MaxLabelSize(Drawing.Font tickFont)
        {
            if (tickLabels is null || tickLabels.Length == 0)
                return (0, 0);

            string largestString = "";
            foreach (string s in tickLabels.Where(x => string.IsNullOrEmpty(x) == false))
                if (s.Length > largestString.Length)
                    largestString = s;

            if (LabelFormat == TickLabelFormat.DateTime)
            {
                // widen largest string based on the longest month name
                foreach (string s in new DateTimeFormatInfo().MonthGenitiveNames)
                {
                    string s2 = s + "\n" + "1985";
                    if (s2.Length > largestString.Length)
                        largestString = s2;
                }
            }

            if (MeasureStringManually)
            {
                float width = largestString.Trim().Length * tickFont.Size * .75f;
                float height = tickFont.Size;
                return (width, height);
            }
            else
            {
                System.Drawing.SizeF maxLabelSize = GDI.MeasureStringUsingTemporaryGraphics(largestString.Trim(), tickFont);
                return (maxLabelSize.Width, maxLabelSize.Height);
            }
        }

        private void RecalculatePositionsAutomaticDatetime(PlotDimensions dims, float labelWidth, float labelHeight, int? forcedTickCount)
        {
            double low, high;
            int tickCount;

            if (MinimumTickSpacing > 0)
                throw new InvalidOperationException("minimum tick spacing does not support DateTime ticks");

            if (Orientation == AxisOrientation.Vertical)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                tickCount = (int)(dims.DataHeight / labelHeight * TickDensity);
                tickCount = forcedTickCount ?? tickCount;
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                tickCount = (int)(dims.DataWidth / labelWidth * TickDensity);
                tickCount = forcedTickCount ?? tickCount;
            }

            if (low < high)
            {
                low = Math.Max(low, new DateTime(0100, 1, 1, 0, 0, 0).ToOADate()); // minimum OADate value
                high = Math.Min(high, DateTime.MaxValue.ToOADate());

                var dtManualUnits = (Orientation == AxisOrientation.Vertical) ? manualDateTimeSpacingUnitY : manualDateTimeSpacingUnitX;
                var dtManualSpacing = (Orientation == AxisOrientation.Vertical) ? manualSpacingY : manualSpacingX;

                try
                {
                    DateTime from = DateTime.FromOADate(low);
                    DateTime to = DateTime.FromOADate(high);

                    var unitFactory = new DateTimeUnitFactory();
                    IDateTimeUnit tickUnit = unitFactory.CreateUnit(from, to, Culture, tickCount, dtManualUnits, (int)dtManualSpacing);
                    (tickPositionsMajor, tickLabels) = tickUnit.GetTicksAndLabels(from, to, dateTimeFormatString);
                    tickLabels = tickLabels.Select(x => x.Trim()).ToArray();
                }
                catch
                {
                    tickPositionsMajor = new double[] { }; // far zoom out can produce FromOADate() exception
                }
            }
            else
            {
                tickPositionsMajor = new double[] { };
            }

            // dont forget to set all the things
            tickPositionsMinor = null;
            CornerLabel = null;
        }

        private void RecalculatePositionsAutomaticNumeric(PlotDimensions dims, float labelWidth, float labelHeight, int? forcedTickCount)
        {
            double low, high, tickSpacing;
            int maxTickCount;

            if (Orientation == AxisOrientation.Vertical)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataHeight / labelHeight * TickDensity);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (manualSpacingY != 0) ? manualSpacingY : GetIdealTickSpacing(low, high, maxTickCount, radix);
                tickSpacing = Math.Max(tickSpacing, MinimumTickSpacing);
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataWidth / labelWidth * TickDensity);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (manualSpacingX != 0) ? manualSpacingX : GetIdealTickSpacing(low, high, maxTickCount, radix);
                tickSpacing = Math.Max(tickSpacing, MinimumTickSpacing);
            }

            // now that tick spacing is known, populate the list of ticks and labels
            double firstTickOffset = low % tickSpacing;
            int tickCount = (int)((high - low) / tickSpacing) + 2;
            tickCount = tickCount > 1000 ? 1000 : tickCount;
            tickCount = tickCount < 1 ? 1 : tickCount;
            tickPositionsMajor = Enumerable.Range(0, tickCount)
                                           .Select(x => low - firstTickOffset + tickSpacing * x)
                                           .Where(x => low <= x && x <= high)
                                           .ToArray();

            if (tickPositionsMajor.Length < 2)
            {
                double tickBelow = low - firstTickOffset;
                double firstTick = tickPositionsMajor.Length > 0 ? tickPositionsMajor[0] : tickBelow;
                double nextTick = tickBelow + tickSpacing;
                tickPositionsMajor = new double[] { firstTick, nextTick };
            }

            if (IntegerPositionsOnly)
            {
                int firstTick = (int)tickPositionsMajor[0];
                tickPositionsMajor = tickPositionsMajor.Where(x => x == (int)x).Distinct().ToArray();

                if (tickPositionsMajor.Length < 2)
                    tickPositionsMajor = new double[] { firstTick - 1, firstTick, firstTick + 1 };
            }

            (tickLabels, CornerLabel) = GetPrettyTickLabels(
                    tickPositionsMajor,
                    useMultiplierNotation,
                    useOffsetNotation,
                    useExponentialNotation,
                    invertSign: LabelUsingInvertedSign,
                    culture: Culture
                );

            if (MinorTickDistribution == MinorTickDistribution.log)
                tickPositionsMinor = MinorFromMajorLog(tickPositionsMajor, low, high, LogScaleMinorTickCount);
            else
                tickPositionsMinor = MinorFromMajor(tickPositionsMajor, MinorTickCount, low, high);
        }

        public override string ToString()
        {
            string allTickLabels = string.Join(", ", tickLabels);
            return $"Tick Collection: [{allTickLabels}] {CornerLabel}";
        }

        private static double GetIdealTickSpacing(double low, double high, int maxTickCount, int radix = 10)
        {
            double range = high - low;
            int exponent = (int)Math.Log(range, radix);
            List<double> tickSpacings = new List<double>() { Math.Pow(radix, exponent) };
            tickSpacings.Add(tickSpacings.Last());
            tickSpacings.Add(tickSpacings.Last());

            double[] divBy;
            if (radix == 10)
                divBy = new double[] { 2, 2, 2.5 }; // 10, 5, 2.5, 1
            else if (radix == 16)
                divBy = new double[] { 2, 2, 2, 2 }; // 16, 8, 4, 2, 1
            else
                throw new ArgumentException($"radix {radix} is not supported");

            int divisions = 0;
            int tickCount = 0;
            while ((tickCount < maxTickCount) && (tickSpacings.Count < 1000))
            {
                tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
                tickCount = (int)(range / tickSpacings.Last());
            }

            int startIndex = 3;

            double maxSpacing = range / 2.0;
            double idealSpacing = tickSpacings[tickSpacings.Count - startIndex];

            while (idealSpacing > maxSpacing && startIndex >= 1)
            {
                idealSpacing = tickSpacings[tickSpacings.Count - startIndex];
                startIndex--;
            }

            return idealSpacing;
        }

        private string FormatLocal(double value, CultureInfo culture)
        {
            // if a custom format string exists use it
            if (numericFormatString != null)
                return value.ToString(numericFormatString, culture);

            // if the number is round or large, use the numeric format
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
            bool isRoundNumber = ((int)value == value);
            bool isLargeNumber = (Math.Abs(value) > 1000);
            if (isRoundNumber || isLargeNumber)
                return value.ToString("N0", culture);

            // otherwise the number is probably small or very precise to use the general format (with slight rounding)
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
            return Math.Round(value, 10).ToString("G", culture);
        }

        public (string[], string) GetPrettyTickLabels(
                double[] positions,
                bool useMultiplierNotation,
                bool useOffsetNotation,
                bool useExponentialNotation,
                bool invertSign,
                CultureInfo culture
            )
        {
            // given positions returns nicely-formatted labels (with offset and multiplier)

            string[] labels = new string[positions.Length];
            string cornerLabel = "";

            if (positions.Length == 0)
                return (labels, cornerLabel);

            double range = positions.Last() - positions.First();

            double exponent = (int)(Math.Log10(range));

            double multiplier = 1;
            if (useMultiplierNotation)
            {
                if (Math.Abs(exponent) > 2)
                    multiplier = Math.Pow(10, exponent);
            }

            double offset = 0;
            if (useOffsetNotation)
            {
                offset = positions.First();
                if (Math.Abs(offset / range) < 10)
                    offset = 0;
            }

            for (int i = 0; i < positions.Length; i++)
            {
                double adjustedPosition = (positions[i] - offset) / multiplier;
                if (invertSign)
                    adjustedPosition *= -1;
                labels[i] = ManualTickFormatter is null ? FormatLocal(adjustedPosition, culture) : ManualTickFormatter(adjustedPosition);
                if (labels[i] == "-0")
                    labels[i] = "0";
            }

            if (useExponentialNotation)
            {
                if (multiplier != 1)
                    cornerLabel += String.Format(CornerLabelFormat, exponent);
                if (offset != 0)
                    cornerLabel += Tools.ScientificNotation(offset);
            }
            else
            {
                if (multiplier != 1)
                    cornerLabel += FormatLocal(multiplier, culture);
                if (offset != 0)
                    cornerLabel += " +" + FormatLocal(offset, culture);
                cornerLabel = cornerLabel.Replace("+-", "-");
            }

            return (labels, cornerLabel);
        }

        public double[] MinorFromMajor(double[] majorTicks, double minorTicksPerMajorTick, double lowerLimit, double upperLimit)
        {
            if ((majorTicks == null) || (majorTicks.Length < 2))
                return null;

            double majorTickSpacing = majorTicks[1] - majorTicks[0];
            double minorTickSpacing = majorTickSpacing / minorTicksPerMajorTick;

            List<double> majorTicksWithPadding = new List<double>();
            majorTicksWithPadding.Add(majorTicks[0] - majorTickSpacing);
            majorTicksWithPadding.AddRange(majorTicks);

            List<double> minorTicks = new List<double>();
            foreach (var majorTickPosition in majorTicksWithPadding)
            {
                for (int i = 1; i < minorTicksPerMajorTick; i++)
                {
                    double minorTickPosition = majorTickPosition + minorTickSpacing * i;
                    if ((minorTickPosition > lowerLimit) && (minorTickPosition < upperLimit))
                        minorTicks.Add(minorTickPosition);
                }
            }

            return minorTicks.ToArray();
        }

        /// <summary>
        /// Return an array of log-spaced minor tick marks for the given range
        /// </summary>
        /// <param name="majorTickPositions">Locations of visible major ticks. Must be evenly spaced.</param>
        /// <param name="min">Do not include minor ticks less than this value.</param>
        /// <param name="max">Do not include minor ticks greater than this value.</param>
        /// <param name="divisions">Number of minor ranges to divide each major range into. (A range is the space between tick marks)</param>
        /// <returns>Array of minor tick positions (empty at positions occupied by major ticks)</returns>
        public double[] MinorFromMajorLog(double[] majorTickPositions, double min, double max, int divisions)
        {
            // if too few major ticks are visible, don't attempt to render minor ticks
            if (majorTickPositions is null || majorTickPositions.Length < 2)
                return null;

            double majorTickSpacing = majorTickPositions[1] - majorTickPositions[0];
            double lowerBound = majorTickPositions.First() - majorTickSpacing;
            double upperBound = majorTickPositions.Last() + majorTickSpacing;

            List<double> minorTicks = new();
            for (double majorTick = lowerBound; majorTick <= upperBound; majorTick += majorTickSpacing)
            {
                double[] positions = GetLogDistributedPoints(divisions, majorTick, majorTick + majorTickSpacing, false);
                minorTicks.AddRange(positions);
            }

            return minorTicks.Where(x => x >= min && x <= max).ToArray();
        }

        public static string[] GetDateLabels(double[] ticksOADate, CultureInfo culture)
        {

            TimeSpan dtTickSep;
            string dtFmt = null;

            try
            {
                // TODO: replace this with culture-aware format
                dtTickSep = DateTime.FromOADate(ticksOADate[1]) - DateTime.FromOADate(ticksOADate[0]);
                if (dtTickSep.TotalDays > 365 * 5)
                    dtFmt = "{0:yyyy}";
                else if (dtTickSep.TotalDays > 365)
                    dtFmt = "{0:yyyy-MM}";
                else if (dtTickSep.TotalDays > .5)
                    dtFmt = "{0:yyyy-MM-dd}";
                else if (dtTickSep.TotalMinutes > .5)
                    dtFmt = "{0:yyyy-MM-dd\nH:mm}";
                else
                    dtFmt = "{0:yyyy-MM-dd\nH:mm:ss}";
            }
            catch
            {
            }

            string[] labels = new string[ticksOADate.Length];
            for (int i = 0; i < ticksOADate.Length; i++)
            {
                try
                {
                    DateTime dt = DateTime.FromOADate(ticksOADate[i]);
                    string lbl = string.Format(culture, dtFmt, dt);
                    labels[i] = lbl;
                }
                catch
                {
                    labels[i] = "?";
                }
            }
            return labels;
        }

        private Tick[] GetMajorTicks(double min, double max)
        {
            if (tickPositionsMajor is null || tickPositionsMajor.Length == 0)
                return new Tick[] { };

            List<Tick> ticks = new();

            for (int i = 0; i < tickPositionsMajor.Length; i++)
            {
                var tick = new Tick(
                    position: tickPositionsMajor[i],
                    label: tickLabels[i],
                    isMajor: true,
                    isDateTime: LabelFormat == TickLabelFormat.DateTime);

                ticks.Add(tick);
            }

            if (additionalTickPositions is not null)
            {
                double sign = LabelUsingInvertedSign ? -1 : 1;

                for (int i = 0; i < additionalTickPositions.Length; i++)
                {
                    var tick = new Tick(
                        position: additionalTickPositions[i] * sign,
                        label: additionalTickLabels[i],
                        isMajor: true,
                        isDateTime: LabelFormat == TickLabelFormat.DateTime);

                    ticks.Add(tick);
                }
            }

            return ticks.Where(x => x.Position >= min && x.Position <= max).OrderBy(x => x.Position).ToArray();
        }

        private Tick[] GetMinorTicks()
        {
            if (tickPositionsMinor is null || tickPositionsMinor.Length == 0)
                return new Tick[] { };

            Tick[] ticks = new Tick[tickPositionsMinor.Length];
            for (int i = 0; i < ticks.Length; i++)
            {
                ticks[i] = new Tick(
                    position: tickPositionsMinor[i],
                    label: null,
                    isMajor: false,
                    isDateTime: LabelFormat == TickLabelFormat.DateTime);
            }

            return ticks;
        }

        public Tick[] GetTicks(double min, double max)
        {
            return GetMajorTicks(min, max).Concat(GetMinorTicks()).ToArray();
        }

        public Tick[] GetVisibleMajorTicks(PlotDimensions dims)
        {
            double high, low;

            if (Orientation == AxisOrientation.Vertical)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
            }

            return GetMajorTicks(low, high);
        }

        public Tick[] GetVisibleMinorTicks(PlotDimensions dims)
        {
            double high, low;
            if (Orientation == AxisOrientation.Vertical)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
            }

            return GetMinorTicks()
                .Where(t => t.Position >= low && t.Position <= high)
                .ToArray();
        }

        public Tick[] GetVisibleTicks(PlotDimensions dims)
        {
            return GetVisibleMajorTicks(dims).Concat(GetVisibleMinorTicks(dims)).ToArray();
        }

        /// <summary>
        /// Return log-distributed points between the min/max values
        /// </summary>
        /// <param name="count">number of divisions</param>
        /// <param name="min">lowest value</param>
        /// <param name="max">highest value</param>
        /// <param name="inclusive">if true, returned values will contain the min/max values themselves</param>
        /// <returns></returns>
        public static double[] GetLogDistributedPoints(int count, double min, double max, bool inclusive)
        {
            double range = max - min;
            var values = DataGen.Range(1, 10, 10.0 / count)
                .Select(x => Math.Log10(x))
                .Select(x => x * range + min);
            return inclusive ? values.ToArray() : values.Skip(1).Take(count - 2).ToArray();
        }
    }
}
