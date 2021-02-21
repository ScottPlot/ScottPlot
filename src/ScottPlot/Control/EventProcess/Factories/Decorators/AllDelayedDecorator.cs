using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories.Decorators
{
    public class AllDelayedDecorator : UIEventFactoryDecorator
    {
        public AllDelayedDecorator(IUIEventFactory source) : base(source)
        {
            renderConfig = new Dictionary<EventType, RenderType>
            {
                { EventType.BenchmarkToggle, RenderType.HQAfterLQDelayed},
                { EventType.ApplyZoomRectangle, RenderType.HQAfterLQDelayed},
                { EventType.MouseAutoAxis, RenderType.HQAfterLQDelayed},
                { EventType.MouseMovedToZoomRectangle, RenderType.HQAfterLQDelayed},
                { EventType.MousePan, RenderType.HQAfterLQDelayed},
                { EventType.MouseScroll, RenderType.HQAfterLQDelayed},
                { EventType.MouseupClearRender, RenderType.HQAfterLQDelayed},
                { EventType.MouseZoom, RenderType.HQAfterLQDelayed},
                { EventType.PlottableDrag, RenderType.HQAfterLQDelayed},
            };
        }
    }
}
