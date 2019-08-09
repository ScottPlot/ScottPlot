using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class TickCollection
    {
        double[] tickPositions;
        string[] tickLabels;
        string cornerLabel;

        public TickCollection(double low, double high)
        {
            // TODO: refactor this part
            tickPositions = ScottPlot.TicksExperimental.GetTicks(low, high);
            ScottPlot.TicksExperimental.GetMantissasExponentOffset(tickPositions, out double[] tickPositionsMantissas, out int tickPositionsExponent, out double offset);
            string multiplierString = ScottPlot.TicksExperimental.GetMultiplierString(offset, tickPositionsExponent);
            tickLabels = new string[tickPositions.Length];
            for (int i = 0; i < tickPositions.Length; i++)
                tickLabels[i] = tickPositionsMantissas[i].ToString();
            cornerLabel = $"{offset} & 10^{tickPositionsExponent}";
        }

        public TickCollection(DateTime low, DateTime high)
        {
            // TODO: refactor this part
            tickPositions = ScottPlot.TicksExperimental.GetTicksForTime(low, high);
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
