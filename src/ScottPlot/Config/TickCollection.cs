using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Config
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
        public SizeF maxLabelSize;
        public bool dateFormat;
        private bool verticalAxis;
        public bool invertSign;
        public bool logScale;
        public string numericFormatString;
        public string dateTimeFormatString;

        public int radix = 10;
        public string prefix = null;


        public TickCollection(bool verticalAxis)
        {
            this.verticalAxis = verticalAxis;
        }

        public SizeF LargestLabel(Settings settings, string[] labels)
        {
            SizeF max = new SizeF(0, 0);
            foreach (string label in labels)
            {
                SizeF tickLabelSize = Drawing.GDI.MeasureString(settings.gfxData, label, settings.ticks.font);
                max.Width = Math.Max(max.Width, tickLabelSize.Width);
                max.Height = Math.Max(max.Height, tickLabelSize.Height);
            }
            return max;
        }

        public void Recalculate(Settings settings)
        {
            if (manualTickPositions is null)
            {
                if (dateFormat)
                    RecalculatePositionsAutomaticDatetime(settings);
                else
                    RecalculatePositionsAutomaticNumeric(settings);
            }
            else
            {
                double min = verticalAxis ? settings.axes.y.min : settings.axes.x.min;
                double max = verticalAxis ? settings.axes.y.max : settings.axes.x.max;
                tickPositionsMajor = manualTickPositions.Where(x => x >= min && x <= max).ToArray();
                tickPositionsMinor = null;
                tickLabels = manualTickLabels;
                maxLabelSize = LargestLabel(settings, manualTickLabels);
                cornerLabel = null;
            }
        }

        private void RecalculatePositionsAutomaticDatetime(Settings settings)
        {
            // the goal of this function is to set tickPositionsMajor, tickLabels, tickPositionsMinor, cornerLabel, and maxLabelSize
            double low, high;
            int tickCount;

            // predict maxLabelSize up front using predetermined label sizes
            maxLabelSize = Drawing.GDI.MeasureString(settings.gfxData, "2019-08-20\n8:42:17 PM", settings.ticks.font);

            if (verticalAxis)
            {
                low = settings.axes.y.min - settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.y.max + settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                tickCount = (int)(settings.dataSize.Height / maxLabelSize.Height);
            }
            else
            {
                low = settings.axes.x.min - settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.x.max + settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                tickCount = (int)(settings.dataSize.Width / maxLabelSize.Width);
            }

            if (low < high)
            {
                low = Math.Max(low, DateTime.MinValue.ToOADate());
                high = Math.Min(high, DateTime.MaxValue.ToOADate());

                var dtManualUnits = (verticalAxis) ? settings.ticks.manualDateTimeSpacingUnitY : settings.ticks.manualDateTimeSpacingUnitX;
                var dtManualSpacing = (verticalAxis) ? settings.ticks.manualSpacingY : settings.ticks.manualSpacingX;

                try
                {
                    DateTime from = DateTime.FromOADate(low);
                    DateTime to = DateTime.FromOADate(high);

                    var unitFactory = new DateTimeUnitFactory();
                    IDateTimeUnit tickUnit = unitFactory.CreateUnit(from, to, settings.culture, tickCount, dtManualUnits, (int)dtManualSpacing);
                    (tickPositionsMajor, tickLabels) = tickUnit.GetTicksAndLabels(from, to, dateTimeFormatString);
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
            maxLabelSize = LargestLabel(settings, tickLabels);
        }

        private (double low, double high) PopulateTickPositions(Settings settings, double tickCountMultiplier = 1)
        {
            double low, high, tickSpacing;
            int maxTickCount;

            if (verticalAxis)
            {
                low = settings.axes.y.min - settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.y.max + settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(tickCountMultiplier * settings.dataSize.Height / maxLabelSize.Height);
                tickSpacing = (settings.ticks.manualSpacingY != 0) ? settings.ticks.manualSpacingY : GetIdealTickSpacing(low, high, maxTickCount, radix);
            }
            else
            {
                low = settings.axes.x.min - settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.x.max + settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(1.2 * tickCountMultiplier * settings.dataSize.Width / maxLabelSize.Width);
                tickSpacing = (settings.ticks.manualSpacingX != 0) ? settings.ticks.manualSpacingX : GetIdealTickSpacing(low, high, maxTickCount, radix);
            }

            double firstTickOffset = low % tickSpacing;
            int tickCount = (int)((high - low) / tickSpacing) + 2;
            tickCount = tickCount > 1000 ? 1000 : tickCount;
            tickCount = tickCount < 1 ? 1 : tickCount;
            tickPositionsMajor = Enumerable.Range(0, tickCount)
                                           .Select(x => low - firstTickOffset + tickSpacing * x)
                                           .Where(x => low <= x && x <= high)
                                           .ToArray();

            return (low, high);
        }

        private void RecalculatePositionsAutomaticNumeric(Settings settings)
        {
            // predict maxLabelSize up front using predetermined label sizes
            string sampleDateFormatLabel = (dateTimeFormatString is null) ? "2019-08-20\n20:42:17" : dateTimeFormatString;
            string longestLabel = (dateFormat) ? sampleDateFormatLabel : "-8888";
            maxLabelSize = Drawing.GDI.MeasureString(settings.gfxData, longestLabel, settings.ticks.font);

            // now that tick spacing is known, populate the list of ticks and labels
            double low;
            double high;
            (low, high) = PopulateTickPositions(settings);

            // if only one tick, try again with a higher tick density
            if (tickPositionsMajor.Length <= 1)
                (low, high) = PopulateTickPositions(settings, 3);

            if (dateFormat)
            {
                tickLabels = GetDateLabels(tickPositionsMajor, settings.culture);
                tickPositionsMinor = null;
            }
            else
            {
                (tickLabels, cornerLabel) = GetPrettyTickLabels(
                        tickPositionsMajor,
                        settings.ticks.useMultiplierNotation,
                        settings.ticks.useOffsetNotation,
                        settings.ticks.useExponentialNotation,
                        invertSign: invertSign,
                        culture: settings.culture
                    );

                if (logScale)
                    tickPositionsMinor = MinorFromMajorLog(tickPositionsMajor, low, high);
                else
                    tickPositionsMinor = MinorFromMajor(tickPositionsMajor, 5, low, high);

            }

            // now set the maximum label size based on the actual labels created
            maxLabelSize = LargestLabel(settings, tickLabels);
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
            bool isRoundNumber = ((int)value == value);
            bool isLargeNumber = (Math.Abs(value) > 1000);

            // round it to 10 digits to fix accumulated floating-point errors
            value = Math.Round(value, 10);

            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            if (numericFormatString is null)
            {
                string defaultFormat = (isRoundNumber || isLargeNumber) ? "N0" : "G";
                return value.ToString(defaultFormat, culture);
            }
            else
            {
                return value.ToString(numericFormatString, culture);
            }
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

            if (positions.Length < 1)
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
