using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Ticks
{
    public interface ITickGenerator
    {
        List<Tick> Ticks { get; }
        double MaxTickCount { get; set; }
        void Recalculate(double min, double max);
        void Recalculate(double min, double max, double fixedSpacingMajor, double fixedSpacingMinor);
    }
}
