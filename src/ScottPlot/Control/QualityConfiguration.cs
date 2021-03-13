using ScottPlot.Control.EventProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class defines the quality to use for renders after different interactive events occur.
    /// Programmatically-triggered events typically use high quality mode (anti-aliasing enabled).
    /// Real-time mouse-interactive events like zooming and panning typically use low quality mode.
    /// It is possible to automatically render using high quality after a period of inactivity.
    /// </summary>
    public class QualityConfiguration
    {
        // immediate high quality is typically preferred for these non-interactive events
        public RenderType BenchmarkToggle = RenderType.HQOnly;
        public RenderType AutoAxis = RenderType.HQOnly;

        // low quality followed by automatic re-rendering with high quality is typically preferred
        // for real-time interactive mouse events: pan, zoom, zoom rectangle, and plottable drag
        public RenderType MouseInteractiveDragged = RenderType.LQOnly;
        public RenderType MouseInteractiveDropped = RenderType.HQOnly;

        // scroll wheel events typically used a delayed high quality render
        public RenderType MouseWheelScrolled = RenderType.HQAfterLQDelayed;
    }
}
