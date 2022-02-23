using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Shade the region between two Y values
    /// </summary>
    public class VSpan : AxisSpan
    {
        public double Y1 { get => Position1; set => Position1 = value; }
        public double Y2 { get => Position2; set => Position2 = value; }
        public VSpan() : base(false) { }
        public override string ToString() => $"Vertical span between X1={Y1} and X2={Y2}";
    }
}
