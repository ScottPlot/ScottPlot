using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public interface IColormap
    {
        string Name { get; }
        /// <summary>
        /// Returns a color for the given intensity and intensityRange. Returns Colors.Transparent for an intensity of NaN.
        /// </summary>
        /// <param name="intensity">The intensity</param>
        /// <param name="intensityRange">The range of the scale from which the intensity is drawn, or Range.UnitRange if null.</param>
        /// <returns>Color</returns>
        Color GetColor(double intensity, Range? intensityRange);
    }
}
