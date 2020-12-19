using ScottPlot.Ticks.DateTimeTickUnits;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks
{
    public class TickCollection
    {
        // This class creates pretty tick labels (with offset and exponent) uses graph settings
        // to inspect the tick font and ensure tick labels will not overlap. 
        // It also respects manually defined tick spacing settings set via plt.Grid().

        public double[] tickPositionsMajor;
        public double[] tickPositionsMinor;
        public string[] tickLabels;

        public double[] manualTickPositions;
        public string[] manualTickLabels;

        public string cornerLabel;
        public float maxLabelWidth = 15;
        public float maxLabelHeight = 12;

        public bool dateFormat;
        public bool verticalAxis;
        public bool invertSign;
        public bool logScale;
        public string numericFormatString;
        public string dateTimeFormatString;

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

        public void Recalculate(PlotDimensions dims, Drawing.Font tickFont)
        {
            if (manualTickPositions is null)
            {
                // first pass uses forced density with manual label sizes to consistently approximate labels
                if (dateFormat)
                    RecalculatePositionsAutomaticDatetime(dims, 20, 24, 10);
                else
                    RecalculatePositionsAutomaticNumeric(dims, 15, 12, 10);

                // second pass calculates density using measured labels produced by the first pass
                (maxLabelWidth, maxLabelHeight) = MaxLabelSize(tickFont);
                if (dateFormat)
                    RecalculatePositionsAutomaticDatetime(dims, maxLabelWidth, maxLabelHeight, null);
                else
                    RecalculatePositionsAutomaticNumeric(dims, maxLabelWidth, maxLabelHeight, null);
            }
            else
            {
                tickPositionsMajor = manualTickPositions;
                tickPositionsMinor = null;
                tickLabels = manualTickLabels;
                cornerLabel = null;
                (maxLabelWidth, maxLabelHeight) = MaxLabelSize(tickFont);
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

            var maxLabelSize = GDI.MeasureString(largestString.Trim(), tickFont);
            return (maxLabelSize.Width, maxLabelSize.Height);
        }

        private void RecalculatePositionsAutomaticDatetime(PlotDimensions dims, float labelWidth, float labelHeight, int? forcedTickCount)
        {
            double low, high;
            int tickCount;

            if (verticalAxis)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                tickCount = (int)(dims.DataHeight / labelHeight);
                tickCount = forcedTickCount ?? tickCount;
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                tickCount = (int)(dims.DataWidth / labelWidth);
                tickCount = forcedTickCount ?? tickCount;
            }

            if (low < high)
            {
                low = Math.Max(low, DateTime.MinValue.ToOADate());
                high = Math.Min(high, DateTime.MaxValue.ToOADate());

                var dtManualUnits = (verticalAxis) ? manualDateTimeSpacingUnitY : manualDateTimeSpacingUnitX;
                var dtManualSpacing = (verticalAxis) ? manualSpacingY : manualSpacingX;

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
            cornerLabel = null;
        }

        private void RecalculatePositionsAutomaticNumeric(PlotDimensions dims, float labelWidth, float labelHeight, int? forcedTickCount)
        {
            double low, high, tickSpacing;
            int maxTickCount;

            if (verticalAxis)
            {
                low = dims.YMin - dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                high = dims.YMax + dims.UnitsPerPxY; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataHeight / labelHeight);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (manualSpacingY != 0) ? manualSpacingY : GetIdealTickSpacing(low, high, maxTickCount, radix);
            }
            else
            {
                low = dims.XMin - dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                high = dims.XMax + dims.UnitsPerPxX; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(dims.DataWidth / labelWidth);
                maxTickCount = forcedTickCount ?? maxTickCount;
                tickSpacing = (manualSpacingX != 0) ? manualSpacingX : GetIdealTickSpacing(low, high, maxTickCount, radix);
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

            if (dateFormat)
            {
                tickLabels = GetDateLabels(tickPositionsMajor, Culture);
                tickPositionsMinor = null;
            }
            else
            {
                (tickLabels, cornerLabel) = GetPrettyTickLabels(
                        tickPositionsMajor,
                        useMultiplierNotation,
                        useOffsetNotation,
                        useExponentialNotation,
                        invertSign: invertSign,
                        culture: Culture
                    );

                if (logScale)
                    tickPositionsMinor = MinorFromMajorLog(tickPositionsMajor, low, high);
                else
                    tickPositionsMinor = MinorFromMajor(tickPositionsMajor, 5, low, high);

            }
        }

        public override string ToString()
        {
            string allTickLabels = string.Join(", ", tickLabels);
            return $"Tick Collection: [{allTickLabels}] {cornerLabel}";
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

            return tickSpacings[tickSpacings.Count - 3];
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

            if (positions.Length <= 1)
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
                labels[i] = FormatLocal(adjustedPosition, culture);
                if (labels[i] == "-0")
                    labels[i] = "0";
            }

            if (useExponentialNotation)
            {
                if (multiplier != 1)
                    cornerLabel += $"e{exponent} ";
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

        public double[] MinorFromMajorLog(double[] majorTicks, double lowerLimit, double upperLimit)
        {
            if ((majorTicks == null) || (majorTicks.Length < 2))
                return null;

            double majorTickSpacing = majorTicks[1] - majorTicks[0];
            double lowerBound = majorTicks.First() - majorTickSpacing;
            double upperBound = majorTicks.Last() + majorTickSpacing;

            List<double> minorTicks = new List<double>();
            for (double majorTick = lowerBound; majorTick <= upperBound; majorTick += majorTickSpacing)
            {
                minorTicks.Add(majorTick + majorTickSpacing * (.5));
                minorTicks.Add(majorTick + majorTickSpacing * (.5 + .25));
                minorTicks.Add(majorTick + majorTickSpacing * (.5 + .25 + .125));
                minorTicks.Add(majorTick + majorTickSpacing * (.5 + .25 + .125 + .0625));
            }

            return minorTicks.Where(x => x >= lowerLimit && x <= upperLimit).ToArray();
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
    }
}
