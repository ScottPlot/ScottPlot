using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Ticks
{
    class NumericTickGenerator : ITickGenerator
    {
        public List<Tick> Ticks { get; private set; } = new List<Tick>();
        public double MaxTickCount { get; set; } = 10;

        public void Recalculate(double min, double max)
        {
            double fixedSpacingMajor = TickTools.GetIdealTickSpacing(min, max, (int)MaxTickCount);
            double fixedSpacingMinor = fixedSpacingMajor / 5;
            Recalculate(min, max, fixedSpacingMajor, fixedSpacingMinor);
        }

        public void Recalculate(double min, double max, double fixedSpacingMajor, double fixedSpacingMinor)
        {
            Ticks.Clear();

            double firstMajorTickPosition = min - (min % fixedSpacingMajor);
            if (min > 0)
                firstMajorTickPosition += fixedSpacingMajor;

            for (double i = firstMajorTickPosition; i <= max; i += fixedSpacingMajor)
                Ticks.Add(new Tick(i, Math.Round(i, 8).ToString(), true));
            double[] majorTickPositions = Ticks.Select(x => x.Position).ToArray();


            double firstMinorTickPosition = min - (min % fixedSpacingMinor);
            if (min > 0)
                firstMinorTickPosition += fixedSpacingMinor;

            for (double i = firstMinorTickPosition; i <= max; i += fixedSpacingMinor)
                if (!majorTickPositions.Contains(i))
                    Ticks.Add(new Tick(i));
        }
    }
}
