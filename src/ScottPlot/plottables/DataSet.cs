using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    /// <summary>
    /// Represents a series of data values with a common name. Values from several DataSets can be grouped (by value index).
    /// </summary>
    public class DataSet
    {
        public double[] values;
        public string label;

        public DataSet(double[] values, string label = null)
        {
            this.values = values;
            this.label = label;
        }
    }
}
