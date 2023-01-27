using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Axis
{
    public interface IDateAxis : IAxis
    {
        IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates);
    }
}
