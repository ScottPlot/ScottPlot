using ScottPlot.Axis.StandardAxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Axis
{
    public class DateXAxis : XAxisBase, IXAxis
    {
        public override Edge Edge { get; } = Edge.Bottom;

        public DateXAxis()
        {
            TickGenerator = new TickGenerators.DateAutomatic();
        }
    }
}
