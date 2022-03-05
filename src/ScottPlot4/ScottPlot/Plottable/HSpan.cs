using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Shaded horizontal region between two X values
    /// </summary>
    public class HSpan : AxisSpan
    {
        public double X1 { get => Position1; set => Position1 = value; }
        public double X2 { get => Position2; set => Position2 = value; }
        public HSpan() : base(true) { }
        public override string ToString() => $"Horizontal span between Y1={X1} and Y2={X2}";
    }
}
