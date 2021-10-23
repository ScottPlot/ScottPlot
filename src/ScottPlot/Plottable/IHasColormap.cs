using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Describes plottables with a Colormap that may use a Colorbar
    /// </summary>
    public interface IHasColormap
    {
        /// <summary>
        /// Value representing the lowest color on the colormap
        /// </summary>
        double ColormapMin { get; set; }

        /// <summary>
        /// Value representing the highest color on the colormap
        /// </summary>
        double ColormapMax { get; set; }

        /// <summary>
        /// Colormap to use for translating values to colors
        /// </summary>
        Drawing.Colormap Colormap { get; set; }
    }
}
