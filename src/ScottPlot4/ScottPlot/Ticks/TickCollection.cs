using ScottPlot.Drawing;
using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks
{
    public enum TickLabelFormat { Numeric, DateTime };
    public enum AxisOrientation { Vertical, Horizontal };

    /// <summary>
    /// This class contains logic to generate ticks given plot size and axis dimensions. 
    /// </summary>
    public class TickCollection
    {
        private TickPositions TickPositions = TickPositions.Empty;

        // When populated, manual ticks are the ONLY ticks shown
        private TickPositions ManualTickPositions = null;

        // When populated, additionalTicks are shown in addition to automatic ticks
        private TickPositions AdditionalTickPositions = null;

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
        /// Logic for generator minor tick positions
        /// </summary>
        public IMinorTickGenerator MinorTickGenerator { get; set; } = new MinorTickGenerators.EvenlySpaced();

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
        /// Determine tick density using a fixed formula to estimate label size instead of MeasureString().
        /// This is less accurate, but is consistent across operating systems, and is independent of font.
        /// </summary>
        public bool MeasureStringManually = false;

        public void Recalculate(PlotDimensions dims, Drawing.Font tickFont)
        {
            if (ManualTickPositions is not null)
            {
                RecalculateManual(dims, tickFont);
                return;
            }

            RecalculateAutomatic(dims, tickFont);
        }

        public void UseManualTicks(double[] positions, string[] labels)
        {
            ManualTickPositions = new(positions, null, labels);
        }

        public void AddAdditionalTicks(double[] positions, string[] labels)
        {
            AdditionalTickPositions = new(positions, null, labels);
        }

        public void UseAutomaticTicks()
        {
            ManualTickPositions = null;
            AdditionalTickPositions = null;
        }

        private void RecalculateAutomatic(PlotDimensions dims, Drawing.Font tickFont)
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

        private void RecalculateManual(PlotDimensions dims, Drawing.Font tickFont)
        {
            double min = Orientation == AxisOrientation.Vertical ? dims.YMin : dims.XMin;
            double max = Orientation == AxisOrientation.Vertical ? dims.YMax : dims.XMax;

            var visibleIndexes = Enumerable.Range(0, ManualTickPositions.Major.Count())
                .Where(i => ManualTickPositions.Major[i] >= min)
                .Where(i => ManualTickPositions.Major[i] <= max);

            double[] tickPositionsMajor = visibleIndexes.Select(x => ManualTickPositions.Major[x]).ToArray();
            double[] tickPositionsMinor = null;
            string[] tickLabels = visibleIndexes.Select(x => ManualTickPositions.Labels[x]).ToArray();
            TickPositions = new(tickPositionsMajor, tickPositionsMinor, tickLabels);

            CornerLabel = null;
            (LargestLabelWidth, LargestLabelHeight) = MaxLabelSize(tickFont);
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
            if (TickPositions.Labels is null || TickPositions.Labels.Length == 0)
                return (0, 0);

            string largestString = "";
            foreach (string s in TickPositions.Labels.Where(x => string.IsNullOrEmpty(x) == false))
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
                    (double[] tickPositionsMajor, string[] tickLabels) = tickUnit.GetTicksAndLabels(from, to, dateTimeFormatString);
                    tickLabels = tickLabels.Select(x => x.Trim()).ToArray();
                    TickPositions = new TickPositions(tickPositionsMajor, null, tickLabels);
                }
                catch
                {
                    // zooming too far can produce exceptions (numic or DateTime)
                    TickPositions = TickPositions.Empty;
                }
            }
            else
            {
                TickPositions = TickPositions.Empty;
            }

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
            double[] tickPositionsMajor = Enumerable.Range(0, tickCount)
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

            (string[] tickLabels, CornerLabel) = GetPrettyTickLabels(
                    tickPositionsMajor,
                    useMultiplierNotation,
                    useOffsetNotation,
                    useExponentialNotation,
                    invertSign: LabelUsingInvertedSign,
                    culture: Culture
                );

            double[] tickPositionsMinor = MinorTickGenerator.GetMinorPositions(tickPositionsMajor, low, high);

            TickPositions = new(tickPositionsMajor, tickPositionsMinor, tickLabels);
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
            if (TickPositions.Major is null || TickPositions.Major.Length == 0)
                return new Tick[] { };

            List<Tick> ticks = new();

            for (int i = 0; i < TickPositions.Major.Length; i++)
            {
                Tick tick = new(
                    position: TickPositions.Major[i],
                    label: TickPositions.Labels[i],
                    isMajor: true,
                    isDateTime: LabelFormat == TickLabelFormat.DateTime);

                ticks.Add(tick);
            }

            if (AdditionalTickPositions is not null)
            {
                double sign = LabelUsingInvertedSign ? -1 : 1;

                for (int i = 0; i < AdditionalTickPositions.Major.Length; i++)
                {
                    var tick = new Tick(
                        position: AdditionalTickPositions.Major[i] * sign,
                        label: AdditionalTickPositions.Labels[i],
                        isMajor: true,
                        isDateTime: LabelFormat == TickLabelFormat.DateTime);

                    ticks.Add(tick);
                }
            }

            return ticks.Where(x => x.Position >= min && x.Position <= max).OrderBy(x => x.Position).ToArray();
        }

        private Tick[] GetMinorTicks()
        {
            if (TickPositions.Minor is null || TickPositions.Minor.Length == 0)
                return new Tick[] { };

            Tick[] ticks = new Tick[TickPositions.Minor.Length];
            for (int i = 0; i < ticks.Length; i++)
            {
                ticks[i] = new Tick(
                    position: TickPositions.Minor[i],
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
    }
}
