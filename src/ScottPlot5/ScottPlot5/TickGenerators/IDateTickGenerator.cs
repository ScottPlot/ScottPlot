using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.TickGenerators
{
    public interface IDateTickGenerator : ITickGenerator
    {
        IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates);
    }
}
