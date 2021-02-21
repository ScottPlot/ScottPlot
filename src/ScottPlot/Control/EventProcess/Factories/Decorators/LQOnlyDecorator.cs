using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories.Decorators
{
    public class LQOnlyDecorator : UIEventFactoryDecorator
    {
        public LQOnlyDecorator(IUIEventFactory source) : base(source)
        {
            renderConfig = new Dictionary<EventType, RenderType>
            {
                { EventType.BenchmarkToggle, RenderType.LQOnly},
                { EventType.ApplyZoomRectangle, RenderType.LQOnly},
                { EventType.MouseAutoAxis, RenderType.LQOnly},
                { EventType.MouseMovedToZoomRectangle, RenderType.LQOnly},
                { EventType.MousePan, RenderType.LQOnly},
                { EventType.MouseScroll, RenderType.LQOnly},
                { EventType.MouseupClearRender, RenderType.LQOnly},
                { EventType.MouseZoom, RenderType.LQOnly},
                { EventType.PlottableDrag, RenderType.LQOnly},
            };
        }
    }
}
