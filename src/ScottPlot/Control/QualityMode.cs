using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public enum QualityMode
    {
        /// <summary>
        /// Anti-aliasing always off
        /// </summary>
        Low,


        /// <summary>
        /// Anti-aliasing off while dragging (more responsive) but on otherwise
        /// </summary>
        LowWhileDragging,

        /// <summary>
        /// Anti-aliasing always on
        /// </summary>
        High,
    }
}
