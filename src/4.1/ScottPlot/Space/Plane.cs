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
        public IAxis X { get; private set; }
        public IAxis Y { get; private set; }

        public Plane(IAxis x, IAxis y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"X=[{X.Min}, {X.Max}], Y=[{Y.Min}, {Y.Max}]";
        }
    }
}
