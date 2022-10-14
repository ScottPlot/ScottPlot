using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public interface IPosition
    {
        /// <summary>
        /// Gets the position of the top left corner of a box with given size, within a given PixelRect
        /// </summary>
        /// <param name="bounds">The rectangle relative to which the box is positioned</param>
        /// <param name="size">The size of the box. Use a size of (0, 0) for a point</param>
        /// <returns>The position of the top left corner</returns>
        Pixel GetPosition(PixelRect bounds, PixelSize size);
    }
}
