using ScottPlot.Axis.StandardAxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Axis
{
    public class DateXAxis : XAxisBase, IXAxis, IDateAxis
    {
        public override Edge Edge { get; } = Edge.Bottom;
        public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) => dates.Select(dt => dt.ToOADate());

        public DateXAxis()
        {
            TickGenerator = new TickGenerators.DateAutomatic();
        }
    }
}
