using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Plottables can interface this if they have data which should be considered when determining automatic axis limits.
    /// </summary>
    public interface IHasAxisLimits
    {
        /// <summary>
        /// Returns the limits of the data contained in a plottable.
        /// If an axis has no data its min and max may be Double.NaN.
        /// </summary>
        /// <returns></returns>
        AxisLimits GetAxisLimits();
    }
}
