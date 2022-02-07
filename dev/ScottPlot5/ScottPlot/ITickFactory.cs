using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public interface ITickFactory
    {
        public Tick[] GenerateTicks(PlotInfo info, Edge edge);
    }
}
