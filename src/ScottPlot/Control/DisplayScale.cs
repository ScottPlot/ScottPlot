using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class detects and stores display scale.
    /// The scale ratio stored here is used to translate mouse coordinates to real pixel coordinates 
    /// in user controls on systems that use display scaling.
    /// </summary>
    public class DisplayScale
    {
        public float ScaleRatio { get; private set; }

        public DisplayScale()
        {
            Measure();
        }

        /// <summary>
        /// Updates ScaleRatio to represent the scale factor of the active display.
        /// Call this from your control if you suspect screen scaling has changed.
        /// </summary>
        public void Measure() => ScaleRatio = ScottPlot.Drawing.GDI.GetScaleRatio();
    }
}
