﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class TickCollection
    {
        // This class creates pretty tick labels (with offset and exponent) uses graph settings
        // to inspect the tick font and ensure tick labels will not overlap. 
        // It also respects manually defined tick spacing settings set via plt.Grid().

        public double[] tickPositionsMajor;
        public double[] tickPositionsMinor;
        public string[] tickLabels;
        public string cornerLabel;
        public SizeF maxLabelSize;

        public TickCollection(Settings settings, bool verticalAxis = false, bool dateFormat = false)
        {

            double low, high, tickSpacing;
            int maxTickCount;

            string longestLabel = (dateFormat) ? "2019-08-20\n20:42:17" : "-8888";
            maxLabelSize = settings.gfxFigure.MeasureString(longestLabel, settings.ticks.font);
            if (verticalAxis)
            {
                low = settings.axes.y.min;
                high = settings.axes.y.max;
                maxTickCount = (int)(settings.dataSize.Height / maxLabelSize.Height);
                tickSpacing = (settings.ticks.manualSpacingX != 0) ? settings.ticks.manualSpacingY : GetIdealTickSpacing(low, high, maxTickCount);
            }
            else
            {
                low = settings.axes.x.min;
                high = settings.axes.x.max;
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
                tickLabels = GetDateLabels(tickPositionsMajor);
                tickPositionsMinor = null;
            }
            else
            {
                GetPrettyTickLabels(tickPositionsMajor, out tickLabels, out cornerLabel,
                    settings.ticks.useMultiplierNotation, settings.ticks.useOffsetNotation, settings.ticks.useExponentialNotation);
                tickPositionsMinor = MinorFromMajor(tickPositionsMajor, 5, low, high);
            }
        }

        public override string ToString()
        {
            return $"Tick Collection: [{string.Join(", ", tickLabels)}] {cornerLabel}";
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

        public void GetPrettyTickLabels(double[] positions, out string[] labels, out string cornerLabel,
            bool useMultiplierNotation, bool useOffsetNotation, bool useExponentialNotation)
        {
            // given positions returns nicely-formatted labels (with offset and multiplier)

            labels = new string[positions.Length];
            cornerLabel = "";
            if (positions.Length <= 1)
                return;

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
                labels[i] = Math.Round(adjustedPosition, 5).ToString();
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
                    cornerLabel += multiplier.ToString("F99").TrimEnd('0');
                if (offset != 0)
                    cornerLabel += " +" + offset.ToString("F99").TrimEnd('0');
                cornerLabel = cornerLabel.Replace("+-", "-");
            }
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

        public static string[] GetDateLabels(double[] ticksOADate)
        {

            TimeSpan dtTickSep;
            string dtFmt = null;

            try
            {
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
                DateTime dt;
                try
                {
                    dt = DateTime.FromOADate(ticksOADate[i]);
                    string lbl = string.Format(dtFmt, dt);
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
