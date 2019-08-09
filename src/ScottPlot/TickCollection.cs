using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * The TickCollection class is responsible for determining ideal tick positions
 * and creating their labels and a corner label (containing an offset, mantissa, 
 * and base-10 exponent).
 * 
 */

namespace ScottPlot
{
    public class TickCollection
    {
        public double[] tickPositions;
        public string[] tickLabels;
        public string cornerLabel;

        public TickCollection(double low, double high, double? tickSpacing = null)
        {
            if (tickSpacing == 0)
                tickSpacing = null;

            // TODO: refactor this part
            tickPositions = ScottPlot.TickCalculator.GetTicks(low, high);

            if (tickSpacing != null)
                throw new NotImplementedException("manual tick spacing not yet supported");

            ScottPlot.TickCalculator.GetMantissasExponentOffset(tickPositions, out double[] tickPositionsMantissas, out int tickPositionsExponent, out double offset);
            string multiplierString = ScottPlot.TickCalculator.GetMultiplierString(offset, tickPositionsExponent);
            tickLabels = new string[tickPositions.Length];
            for (int i = 0; i < tickPositions.Length; i++)
                tickLabels[i] = tickPositionsMantissas[i].ToString();
            string offsetLabel = offset.ToString();
            if (offset > 0)
                offsetLabel = "+" + offsetLabel;
            offsetLabel = offsetLabel.Replace("E+", "E");

            cornerLabel = "";

            if (tickPositionsExponent != 0)
                cornerLabel = $"1E{tickPositionsExponent}";
            if (offset != 0)
                cornerLabel += $" {offsetLabel}";
        }

        public TickCollection(DateTime low, DateTime high)
        {
            // TODO: refactor this part
            tickPositions = ScottPlot.TickCalculator.GetTicksForTime(low, high);
            tickLabels = new string[tickPositions.Length];
            for (int i = 0; i < tickPositions.Length; i++)
                tickLabels[i] = tickPositions[i].ToString();
            cornerLabel = $"time multiplier"; // make this say minutes, seconds, etc
        }

        public override string ToString()
        {
            return $"Tick Collection: [{string.Join(", ", tickLabels)}] {cornerLabel}";
        }
    }
}
