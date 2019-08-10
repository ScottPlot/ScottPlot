using System;
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

        public double[] tickPositions;
        public string[] tickLabels;
        public string cornerLabel;

        public TickCollection(Settings settings, bool verticalAxis = false)
        {
            double low, high, tickSpacing;
            int maxTickCount;

            SizeF maxTickLabelSize = settings.gfxData.MeasureString("-9999", settings.tickFont);
            if (verticalAxis)
            {
                low = settings.axis[2];
                high = settings.axis[3];
                maxTickCount = (int)(settings.dataSize.Height / maxTickLabelSize.Height);
                tickSpacing = (settings.tickSpacingX != 0) ? settings.tickSpacingY : GetIdealTickSpacing(low, high, maxTickCount);
            }
            else
            {
                low = settings.axis[0];
                high = settings.axis[1];
                maxTickCount = (int)(settings.dataSize.Width / maxTickLabelSize.Width);
                tickSpacing = (settings.tickSpacingX != 0) ? settings.tickSpacingX : GetIdealTickSpacing(low, high, maxTickCount);
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
            tickPositions = positions.ToArray();
            GetPrettyTickLabels(tickPositions, out tickLabels, out cornerLabel);
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
                if (tickCount > maxTickCount)
                    break;
            }

            return tickSpacings[tickSpacings.Count - 3];
        }

        public void GetPrettyTickLabels(double[] positions, out string[] labels, out string cornerLabel)
        {
            // given positions returns nicely-formatted labels (with offset and multiplier)

            labels = new string[positions.Length];
            cornerLabel = "";
            if (positions.Length <= 1)
                return;

            double range = positions.Last() - positions.First();

            double exponent = (int)(Math.Log10(range));
            double multiplier = 1;
            if ((exponent < -2) || (exponent > 3))
                multiplier = Math.Pow(10, exponent);

            double offset = positions.First();
            if (Math.Abs(offset / range) < 1000)
                offset = 0;

            for (int i = 0; i < positions.Length; i++)
            {
                double adjustedPosition = (positions[i] - offset) / multiplier;
                labels[i] = Math.Round(adjustedPosition, 5).ToString();
            }

            if (multiplier != 1)
                cornerLabel += $"e{exponent}";
            if (offset != 0)
                cornerLabel += $" +{offset}";
            cornerLabel = cornerLabel.Replace("E", "e");
            cornerLabel = cornerLabel.Replace("e+", "e");
        }

    }
}
