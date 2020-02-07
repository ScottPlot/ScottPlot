using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TickCollection(bool verticalAxis)
        {
            this.verticalAxis = verticalAxis;
        }

        public SizeF LargestLabel(Settings settings, string[] labels)
        {
            SizeF max = new SizeF(0, 0);
            foreach (string label in labels)
            {
                SizeF tickLabelSize = settings.gfxData.MeasureString(label, settings.ticks.font);
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
                tickPositionsMajor = manualTickPositions;
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
            maxLabelSize = settings.gfxData.MeasureString("2019-08-20\n8:42:17 PM", settings.ticks.font);

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
                var dateTicks = DateTimeTicks.GetTicks(DateTime.FromOADate(low), DateTime.FromOADate(high), tickCount, settings.culture);
                tickPositionsMajor = Tools.DateTimesToDoubles(dateTicks.Item1);
                tickLabels = dateTicks.Item2;
                for (int i = 0; i < tickLabels.Length; i++)
                {
                    if (tickLabels[i].Contains(", "))
                        tickLabels[i] = tickLabels[i].Replace(", ", "\n");
                    else
                        tickLabels[i] += "\n "; // auto-layout works better if dates are always two lines
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

        private void RecalculatePositionsAutomaticNumeric(Settings settings)
        {
            // predict maxLabelSize up front using predetermined label sizes
            string longestLabel = (dateFormat) ? "2019-08-20\n20:42:17" : "-8888";
            maxLabelSize = settings.gfxData.MeasureString(longestLabel, settings.ticks.font);

            double low, high, tickSpacing;
            int maxTickCount;

            if (verticalAxis)
            {
                low = settings.axes.y.min - settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.y.max + settings.yAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(settings.dataSize.Height / maxLabelSize.Height);
                tickSpacing = (settings.ticks.manualSpacingY != 0) ? settings.ticks.manualSpacingY : GetIdealTickSpacing(low, high, maxTickCount);
            }
            else
            {
                low = settings.axes.x.min - settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                high = settings.axes.x.max + settings.xAxisUnitsPerPixel; // add an extra pixel to capture the edge tick
                maxTickCount = (int)(settings.dataSize.Width / maxLabelSize.Width * 1.2);
                tickSpacing = (settings.ticks.manualSpacingX != 0) ? settings.ticks.manualSpacingX : GetIdealTickSpacing(low, high, maxTickCount);
            }

            // now that tick spacing is known, populate the list of ticks and labels
            double firstTickOffset = low % (double)tickSpacing;
            List<double> positions = new List<double>();
            for (double position = low - firstTickOffset; position < high; position += (double)tickSpacing)
            {
                if ((low < position) && (high > position))
                    positions.Add(position);
                if (positions.Count > 999)
                    break;
            }
            tickPositionsMajor = positions.ToArray();

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

        private static double GetIdealTickSpacing(double low, double high, int maxTickCount)
        {
            double range = high - low;
            int exponent = (int)Math.Log10(range);
            List<double> tickSpacings = new List<double>() { Math.Pow(10, exponent) };
            tickSpacings.Add(tickSpacings.Last());
            tickSpacings.Add(tickSpacings.Last());

            int divisions = 0;
            double[] divBy = new double[] { 2, 2, 2.5 }; // dividing from 10 yields 5, 2.5, and 1.

            while (true)
            {
                tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
                int tickCount = (int)(range / tickSpacings.Last());
                if ((tickCount > maxTickCount) || (tickSpacings.Count > 1000))
                    break;
            }

            return tickSpacings[tickSpacings.Count - 3];
        }

        private string FormatLocal(double value, CultureInfo culture, int maximumDecimalPlaces = 5)
        {
            value = Math.Round(value, maximumDecimalPlaces);
            bool isRoundNumber = ((int)value == value);
            bool isLargeNumber = (value > 1000);
            string stringFormat = (isRoundNumber || isLargeNumber) ? "N0" : "G";
            string label = value.ToString(stringFormat, culture);
            return label;
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
            double lowerBound = majorTicks.First() - majorTickSpacing;
            double upperBound = majorTicks.Last() + majorTickSpacing;

            List<double> minorTicks = new List<double>();
            for (double pos = lowerBound; pos < upperBound; pos += minorTickSpacing)
                if ((pos > lowerLimit) && (pos < upperLimit))
                    minorTicks.Add(pos);

            return minorTicks.ToArray();
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
