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
        public string label;
        public double[] values;
        public double[] errors;

        public DataSet(string label, double[] values, double[] errors = null)
        {
            this.values = values;
            this.label = label;
            this.errors = errors;

            if (errors != null && errors.Length != values.Length)
                throw new ArgumentException("values and errors must have identical length");
        }
    }
}
