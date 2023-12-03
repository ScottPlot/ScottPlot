using ScottPlot.Drawing;
using ScottPlot.Ticks.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks
{
    public enum TickLabelFormat { Numeric, DateTime };

    /// <summary>
    /// This class contains logic to generate ticks given plot size and axis dimensions
    /// and many configuration settings to customize this behavior.
    /// </summary>
    public class TickGenerator
    {
        /// <summary>
        /// Automatically calculated ticks
        /// </summary>
        private TickCollection Ticks = TickCollection.Empty;

        /// <summary>
        /// When populated, manual ticks are the ONLY ticks shown
        /// </summary>
        private TickCollection ManualTicks = null;

        /// <summary>
        /// When populated, additionalTicks are shown in addition to automatic ticks
        /// </summary>
        private TickCollection AdditionalTicks = null;

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
        public bool IsVertical = true;

        /// <summary>
        /// If True, the sign of numeric tick labels will be inverted.
        /// This is used to give the appearance of descending ticks.
        /// </summary>
        public bool LabelUsingInvertedSign;

        /// <summary>
        /// Logic for generator minor tick positions
        /// </summary>
        public IMinorTickGenerator MinorTickGenerator { get; set; } = new MinorTickGenerators.EvenlySpaced();

        /// <summary>
        /// Format string for generating tick labels from numeric tick positions
        /// </summary>
        public string NumericFormatString;

        /// <summary>
        /// Format string for generating tick labels from DateTime tick positions
        /// </summary>
        public string DateTimeFormatString;

        /// <summary>
        /// If defined, this function will be used to generate tick labels from positions
        /// </summary>
        public Func<double, string> ManualTickFormatter = null;

        /// <summary>
        /// Culture to use for generating tick labels
        /// </summary>
        public CultureInfo Culture { get; set; } = CultureInfo.DefaultThreadCurrentCulture;

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

        /// <summary>
        /// Only show a single tick if the axis has a very small pixel size
        /// </summary>
        public bool ShowSingleTickForSmallPlots = true;

        // legacy configuration options
        public int radix = 10;
        public string prefix = null;
        public double ManualSpacingX = 0;
        public double ManualSpacingY = 0;
        public Ticks.DateTimeUnit? ManualDateTimeSpacingUnitX = null;
        public Ticks.DateTimeUnit? ManualDateTimeSpacingUnitY = null;
        public bool UseMultiplierNotation = false;
        public bool UseOffsetNotation = false;
        public bool UseExponentialNotation = true;

        /// <summary>
        /// Recalculate ticks based on the given plot information.
        /// Access ticks later by calling <see cref="GetTicks(double, double)"/>
        /// </summary>
        public void Recalculate(PlotDimensions dims, Font tickFont)
        {
            if (ManualTicks is not null)
            {
                RecalculateManualTicks(dims, tickFont);
            }
            else
            {
                RecalculateAutomatic(dims, tickFont);
            }
        }

        private void LimitNumberOfTicksForSmallPlots(PlotDimensions dims)
        {
            if (IsVertical)
            {
                if (dims.DataHeight < LargestLabelHeight && Ticks.Major.Length > 0)
                {
                    Ticks = TickCollection.First(Ticks);
                    return;
                }

                if (dims.DataHeight < LargestLabelHeight * 3 && Ticks.Major.Length > 1)
                {
                    Ticks = TickCollection.Two(Ticks);
                    return;
                }
            }
            else
            {
                if (dims.DataWidth < LargestLabelWidth && Ticks.Major.Length > 0)
                {
                    Ticks = TickCollection.First(Ticks);
                    return;
                }

                if (dims.DataWidth < LargestLabelWidth * 5 && Ticks.Major.Length > 1)
                {
                    Ticks = TickCollection.Two(Ticks);
                    return;
                }
            }
        }

        public void UseManualTicks(double[] positions, string[] labels)
        {
            ManualTicks = new(positions, null, labels);
        }

        public void AddAdditionalTicks(double[] positions, string[] labels)
        {
            AdditionalTicks = new(positions, null, labels);
        }

        public void UseAutomaticTicks()
        {
            ManualTicks = null;
            AdditionalTicks = null;
        }

        private void RecalculateAutomatic(PlotDimensions dims, Drawing.Font tickFont)
        {
            // first pass uses forced density with manual label sizes to consistently approximate labels
            int initialTickCount = (int)(10 * TickDensity);
            if (LabelFormat == TickLabelFormat.DateTime)
            {
                float labelWidth = 20;
                float labelHeight = 24;
                RecalculatePositionsAutomaticDatetime(dims, labelWidth, labelHeight, initialTickCount);
            }
            else
            {
                float labelWidth = 15;
                float labelHeight = 12;
                RecalculatePositionsAutomaticNumeric(dims, labelWidth, labelHeight, initialTickCount);
            }

            // use the results of the first pass to estimate the size of the largest tick label
            (LargestLabelWidth, LargestLabelHeight) = MaxLabelSize(tickFont);

            // second pass calculates density using measured labels produced by the first pass
            if (LabelFormat == TickLabelFormat.DateTime)
            {
                RecalculatePositionsAutomaticDatetime(dims, LargestLabelWidth, LargestLabelHeight, null);
            }
            else
            {
                RecalculatePositionsAutomaticNumeric(dims, LargestLabelWidth, LargestLabelHeight, null);
            }

            LimitNumberOfTicksForSmallPlots(dims);
        }

        private void RecalculateManualTicks(PlotDimensions dims, Drawing.Font tickFont)
        {
            double min = IsVertical ? dims.YMin : dims.XMin;
            double max = IsVertical ? dims.YMax : dims.XMax;

            var visibleIndexes = Enumerable.Range(0, ManualTicks.Major.Count())
                .Where(i => ManualTicks.Major[i] >= min)
                .Where(i => ManualTicks.Major[i] <= max);

            double[] tickPositionsMajor = visibleIndexes.Select(x => ManualTicks.Major[x]).ToArray();
            double[] tickPositionsMinor = null;
            string[] tickLabels = visibleIndexes.Select(x => ManualTicks.Labels[x]).ToArray();
            Ticks = new(tickPositionsMajor, tickPositionsMinor, tickLabels);

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
            Culture ??= (CultureInfo)CultureInfo.CurrentCulture.Clone();
            Culture.DateTimeFormat.ShortDatePattern = shortDatePattern ?? Culture.DateTimeFormat.ShortDatePattern;
            Culture.NumberFormat.NumberDecimalDigits = decimalDigits ?? Culture.NumberFormat.NumberDecimalDigits;
            Culture.NumberFormat.NumberDecimalSeparator = decimalSeparator ?? Culture.NumberFormat.NumberDecimalSeparator;
            Culture.NumberFormat.NumberGroupSeparator = numberGroupSeparator ?? Culture.NumberFormat.NumberGroupSeparator;
            Culture.NumberFormat.NumberGroupSizes = numberGroupSizes ?? Culture.NumberFormat.NumberGroupSizes;
            Culture.NumberFormat.NumberNegativePattern = numberNegativePattern ?? Culture.NumberFormat.NumberNegativePattern;
        }

        /// <summary>
        /// Return the size of the largest expected tick label
        /// </summary>
        private (float width, float height) MaxLabelSize(Drawing.Font tickFont)
        {
            if (Ticks.Labels is null || Ticks.Labels.Length == 0)
                return (0, 0);

            string largestString = "00";
            foreach (string s in Ticks.Labels.Where(x => string.IsNullOrEmpty(x) == false))
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
            if (MinimumTickSpacing > 0)
                throw new InvalidOperationException("minimum tick spacing does not support DateTime ticks");

            CornerLabel = null; // should not be shown for DateTime ticks

            double low;
            double high;
            int tickCount;

            if (IsVertical)
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
                double minimumValidOADate = new DateTime(0100, 1, 1, 0, 0, 0).ToOADate();
                low = Math.Max(low, minimumValidOADate);
                high = Math.Min(high, DateTime.MaxValue.ToOADate());

                Ticks.DateTimeUnit? dtManualUnits = IsVertical ? ManualDateTimeSpacingUnitY : ManualDateTimeSpacingUnitX;
                double dtManualSpacing = IsVertical ? ManualSpacingY : ManualSpacingX;

                try
                {
                    DateTime from = DateTime.FromOADate(low);
                    DateTime to = DateTime.FromOADate(high);
                    DateTimeUnitFactory unitFactory = new();
                    IDateTimeUnit tickUnit = unitFactory.CreateUnit(from, to, Culture, tickCount, dtManualUnits, (int)dtManualSpacing);
                    (double[] tickPositionsMajor, string[] tickLabels) = tickUnit.GetTicksAndLabels(from, to, DateTimeFormatString);
                    tickLabels = tickLabels.Select(x => x.Trim()).ToArray();
                    Ticks = new TickCollection(tickPositionsMajor, null, tickLabels);
                }
                catch
                {
                    // zooming too far can produce exceptions (numic or DateTime)
                    Ticks = TickCollection.Empty;
                }
            }
            else
            {
                Ticks = TickCollection.Empty;
            }
        }

        private void RecalculatePositionsAutomaticNumeric(PlotDimensions dims, float labelWidth, float labelHeight, int? forcedTickCount)
        {
            double low, high, tickSpacing;
            int maxTickCount;

            if (IsVertical)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataHeight / labelHeight * TickDensity);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (ManualSpacingY != 0) ? ManualSpacingY : GetIdealTickSpacing(low, high, maxTickCount, radix);
                tickSpacing = Math.Max(tickSpacing, MinimumTickSpacing);
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataWidth / labelWidth * TickDensity);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (ManualSpacingX != 0) ? ManualSpacingX : GetIdealTickSpacing(low, high, maxTickCount, radix);
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

            (string[] tickLabels, CornerLabel) = GetTickLabels(tickPositionsMajor);

            double[] tickPositionsMinor = MinorTickGenerator.GetMinorPositions(tickPositionsMajor, low, high);

            Ticks = new(tickPositionsMajor, tickPositionsMinor, tickLabels);
        }

        /// <summary>
        /// Return a round number that would make a good spacing between major ticks.
        /// </summary>
        private static double GetIdealTickSpacing(double low, double high, int maxTickCount, int radix = 10)
        {
            double range = high - low;
            int exponent = (int)Math.Log(range, radix);
            List<double> tickSpacings = new() { Math.Pow(radix, exponent) };
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

        /// <summary>
        /// Apply various scaling, inversion, and notation operations and return the labels to show on the plot
        /// </summary>
        public (string[] labels, string cornerLabel) GetTickLabels(double[] positions)
        {
            string[] labels = new string[positions.Length];
            string cornerLabel = "";

            if (positions.Length == 0)
                return (labels, cornerLabel);

            double range = positions.Last() - positions.First();

            double exponent = (int)(Math.Log10(range));

            double multiplier = 1;
            if (UseMultiplierNotation)
            {
                if (Math.Abs(exponent) > 2)
                    multiplier = Math.Pow(10, exponent);
            }

            double offset = 0;
            if (UseOffsetNotation)
            {
                offset = positions.First();
                if (Math.Abs(offset / range) < 10)
                    offset = 0;
            }

            for (int i = 0; i < positions.Length; i++)
            {
                double adjustedPosition = (positions[i] - offset) / multiplier;

                if (LabelUsingInvertedSign)
                    adjustedPosition *= -1;

                labels[i] = ManualTickFormatter is null
                    ? GetNumericLabel(adjustedPosition)
                    : ManualTickFormatter(adjustedPosition);

                if (labels[i] == "-0")
                    labels[i] = "0";
            }

            if (UseExponentialNotation)
            {
                if (multiplier != 1)
                    cornerLabel += string.Format(CornerLabelFormat, exponent);
                if (offset != 0)
                    cornerLabel += Tools.ScientificNotation(offset);
            }
            else
            {
                if (multiplier != 1)
                    cornerLabel += GetNumericLabel(multiplier);
                if (offset != 0)
                    cornerLabel += " +" + GetNumericLabel(offset);
                cornerLabel = cornerLabel.Replace("+-", "-");
            }

            return (labels, cornerLabel);
        }

        /// <summary>
        /// Create a tick label for the given number according to <see cref="Culture"/>
        /// </summary>
        private string GetNumericLabel(double value)
        {
            // if a custom format string exists use it
            if (NumericFormatString is not null)
                return value.ToString(NumericFormatString, Culture);

            // if the number is round or large, use the numeric format
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
            bool isRoundNumber = ((int)value == value);
            bool isLargeNumber = (Math.Abs(value) > 1000);
            if (isRoundNumber || isLargeNumber)
                return value.ToString("N0", Culture);

            // otherwise the number is probably small or very precise to use the general format (with slight rounding)
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
            return Math.Round(value, 10).ToString("G", Culture);
        }

        #region methods to create modern Ticks from the ticks stored in this class

        private Tick[] GetMajorTicks(double min, double max)
        {
            if (Ticks.Major is null || Ticks.Major.Length == 0)
                return new Tick[] { };

            List<Tick> ticks = new();

            for (int i = 0; i < Ticks.Major.Length; i++)
            {
                Tick tick = new(
                    position: Ticks.Major[i],
                    label: Ticks.Labels[i],
                    isMajor: true,
                    isDateTime: LabelFormat == TickLabelFormat.DateTime);

                ticks.Add(tick);
            }

            if (AdditionalTicks is not null)
            {
                double sign = LabelUsingInvertedSign ? -1 : 1;

                for (int i = 0; i < AdditionalTicks.Major.Length; i++)
                {
                    var tick = new Tick(
                        position: AdditionalTicks.Major[i] * sign,
                        label: AdditionalTicks.Labels[i],
                        isMajor: true,
                        isDateTime: LabelFormat == TickLabelFormat.DateTime);

                    ticks.Add(tick);
                }
            }

            return ticks.Where(x => x.Position >= min && x.Position <= max).OrderBy(x => x.Position).ToArray();
        }

        private Tick[] GetMinorTicks()
        {
            if (ManualTicks is not null)
            {
                return ManualTicks.Minor.Select(x => new Tick(x, string.Empty, false, false)).ToArray();
            }

            if (Ticks.Minor is null || Ticks.Minor.Length == 0)
                return new Tick[] { };

            Tick[] ticks = new Tick[Ticks.Minor.Length];
            for (int i = 0; i < ticks.Length; i++)
            {
                ticks[i] = new Tick(
                    position: Ticks.Minor[i],
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

        public void SetTicks(Tick[] ticks)
        {
            ManualTicks = new TickCollection(ticks);
        }

        public Tick[] GetVisibleMajorTicks(PlotDimensions dims)
        {
            double high, low;

            if (IsVertical)
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
            if (IsVertical)
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

        #endregion
    }
}
