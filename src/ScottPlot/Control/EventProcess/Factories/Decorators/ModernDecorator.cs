using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories
{
    public class ModernDecorator : UIEventFactoryDecorator
    {
        public ModernDecorator(IUIEventFactory source) : base(source)
        {
            renderConfig = new Dictionary<EventType, RenderType>
            {
                { EventType.BenchmarkToggle, RenderType.HQOnly},
                { EventType.ApplyZoomRectangle, RenderType.HQOnly},
                { EventType.MouseAutoAxis, RenderType.HQOnly},
                { EventType.MouseMovedToZoomRectangle, RenderType.HQAfterLQDelayed},
                { EventType.MousePan, RenderType.HQAfterLQDelayed},
                { EventType.MouseScroll, RenderType.HQAfterLQDelayed},
                { EventType.MouseupClearRender, RenderType.HQOnly},
                { EventType.MouseZoom, RenderType.HQAfterLQDelayed},
                { EventType.PlottableDrag, RenderType.HQAfterLQDelayed},
            };
        }
    }
}
