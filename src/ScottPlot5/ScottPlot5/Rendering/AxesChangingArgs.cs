using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class AxesChangingArgs(AxisLimits oldAxisLimits, AxisLimits newAxisLimits) : EventArgs
    {
        public AxisLimits OldAxisLimits { get; } = oldAxisLimits;
        public AxisLimits NewAxisLimits { get; } = newAxisLimits;
    }
}
