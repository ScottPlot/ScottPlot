using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Experimental
{
    /// <summary>
    /// Represents a single series of bar graphs. Values from a BarSeries can be grouped (by value index) and displayed with other BarSeries.
    /// </summary>
    public class BarSet
    {
        public double[] values;
        public string label;

        public BarSet(double[] values, string label = null)
        {
            this.values = values;
            this.label = label;
        }
    }
}
