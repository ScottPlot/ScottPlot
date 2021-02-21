using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories.Decorators
{
    public class HQOnlyDecorator : UIEventFactoryDecorator
    {
        public HQOnlyDecorator(IUIEventFactory source) : base(source)
        {
            renderConfig = new Dictionary<EventType, RenderType>
            {
                { EventType.BenchmarkToggle, RenderType.HQOnly},
                { EventType.ApplyZoomRectangle, RenderType.HQOnly},
                { EventType.MouseAutoAxis, RenderType.HQOnly},
                { EventType.MouseMovedToZoomRectangle, RenderType.HQOnly},
                { EventType.MousePan, RenderType.HQOnly},
                { EventType.MouseScroll, RenderType.HQOnly},
                { EventType.MouseupClearRender, RenderType.HQOnly},
                { EventType.MouseZoom, RenderType.HQOnly},
                { EventType.PlottableDrag, RenderType.HQOnly},
            };
        }
    }
}
