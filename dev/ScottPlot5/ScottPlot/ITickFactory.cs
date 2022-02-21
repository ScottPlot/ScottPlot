using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public interface ITickFactory
    {
        public Edge Edge { get; }
        public Tick[] GenerateTicks(PlotConfig info);
    }
}
