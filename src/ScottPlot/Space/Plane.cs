using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// A 2D plane holds two 1D axes
    /// </summary>
    public class Plane
    {
        public LinearAxis X { get; set; } = new LinearAxis();
        public LinearAxis Y { get; set; } = new LinearAxis();
    }
}
