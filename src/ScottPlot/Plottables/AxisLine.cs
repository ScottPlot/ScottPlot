using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Plottables
{
    // An AxisLine is just an AxisSpan where only position 1 is used
    public abstract class AxisLine : AxisSpan
    {
        public double position
        {
            get { return position1; }
            set { position1 = value; }
        }
    }
}