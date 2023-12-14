using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.DataSources
{
    public interface IFunctionSource
    {
        CoordinateRange RangeX { get; }
        CoordinateRange GetYRange(CoordinateRange rangeX);
        double Get(double x);
    }
}
