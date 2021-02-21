using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories
{
    public class ClassicDecorator : UIEventFactoryDecorator
    {
        public ClassicDecorator(IUIEventFactory source) : base(source)
        {
            renderConfig = new Dictionary<EventType, RenderType>
            {
                { EventType.BenchmarkToggle, RenderType.HQOnly},
                { EventType.ApplyZoomRectangle, RenderType.HQOnly},
                { EventType.MouseAutoAxis, RenderType.HQOnly},
                { EventType.MouseMovedToZoomRectangle, RenderType.LQOnly},
                { EventType.MousePan, RenderType.LQOnly},
                { EventType.MouseScroll, RenderType.HQAfterLQDelayed},
                { EventType.MouseupClearRender, RenderType.HQOnly},
                { EventType.MouseZoom, RenderType.LQOnly},
                { EventType.PlottableDrag, RenderType.LQOnly},
            };
        }
    }
}
