using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.TickFactories
{
    internal class EmptyTickFactory : ITickFactory
    {
        public Edge Edge { get; private set; }

        public EmptyTickFactory(Edge edge)
        {
            Edge = edge;
        }

        public Tick[] GenerateTicks(PlotConfig info)
        {
            return new Tick[0];
        }
    }
}
