using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    // This is an EXPERIMENTAL version of the TickCollection which has as little code possible.
    // It assesses the range, generates the ticks, and handles a corner offset and multiplier.

    public class TickCollectionMinimal
    {
        public double[] tickPositions;
        public string[] tickLabels;
        public string cornerLabel;

        public TickCollectionMinimal(double low, double high, double? tickSpacing = null)
        {
            if (tickSpacing == 0 || tickSpacing == null)
                tickSpacing = GetTickSpacing(low, high);

            tickPositions = GetTickPositions(low, high, (double)tickSpacing);
            tickLabels = new string[tickPositions.Length];

            double largestTickValue = Math.Max(Math.Abs(tickPositions.Last()), Math.Abs(tickPositions.First()));

            Console.WriteLine(largestTickValue);

            int exponent = (int)Math.Log10(largestTickValue);
            if (Math.Abs(exponent) <= 3)
                exponent = 0;
            double multiplier = Math.Pow(10, exponent);

            for (int i = 0; i < tickPositions.Length; i++)
                tickLabels[i] = (tickPositions[i] / multiplier).ToString();
            if (exponent != 0)
                cornerLabel = $"e{exponent}";
        }

        public override string ToString()
        {
            return $"Tick Collection (minimal): [{string.Join(", ", tickLabels)}] {cornerLabel}";
        }

        private static double GetTickSpacing(double low, double high, int tickCount = 5)
        {
            double axisSpan = high - low;
            for (int tickSpacingPower = 1000; tickSpacingPower > -1000; tickSpacingPower--)
            {
                double tickSpacing = Math.Pow(10, tickSpacingPower);
                if (tickSpacing > axisSpan)
                    continue;
                double tickCount = axisSpan / tickSpacing;
                if (tickCount >= tickCount)
                {
                    if (tickCount >= tickCount * 5)
                        tickSpacing *= 5;
                    else if (tickCount >= tickCount * 2)
                        tickSpacing *= 2;
                    return tickSpacing;
                }
            }
            return int.MaxValue;
        }

        private static double[] GetTickPositions(double low, double high, double tickSpacing)
        {
            double axisSpan = high - low;
            double tickOffset = low % tickSpacing;
            List<double> positions = new List<double>();
            for (double position = low - tickOffset; position < high; position += tickSpacing)
                if ((low < position) && (high > position))
                    positions.Add(position);
            return positions.ToArray();
        }
    }
}
