using ScottPlot.Plottable;
using System.Collections.Generic;

namespace ScottPlot.Control.EventProcess.Factories
{
    abstract public class UIEventFactoryDecorator : IUIEventFactory
    {
        public Configuration config { get => source.config; set => source.config = value; }
        public Plot plt { get => source.plt; set => source.plt = value; }
        public Settings settings { get => source.settings; set => source.settings = value; }
        private IUIEventFactory source;
        public Dictionary<EventType, RenderType> renderConfig { get; protected set; }

        public UIEventFactoryDecorator(IUIEventFactory source)
        {
            this.source = source;
        }

        public IUIEvent CreateBenchmarkToggle()
        {
            var result = source.CreateBenchmarkToggle();
            result.RenderOrder = renderConfig[EventType.BenchmarkToggle];
            return result;
        }
        public IUIEvent CreateApplyZoomRectangleEvent(float x, float y)
        {
            var result = source.CreateApplyZoomRectangleEvent(x, y);
            result.RenderOrder = renderConfig[EventType.ApplyZoomRectangle];
            return result;
        }

        public IUIEvent CreateMouseAutoAxis()
        {
            var result = source.CreateMouseAutoAxis();
            result.RenderOrder = renderConfig[EventType.MouseAutoAxis];
            return result;
        }

        public IUIEvent CreateMouseMovedToZoomRectangle(float x, float y)
        {
            var result = source.CreateMouseMovedToZoomRectangle(x, y);
            result.RenderOrder = renderConfig[EventType.MouseMovedToZoomRectangle];
            return result;
        }

        public IUIEvent CreateMousePan(InputState input)
        {
            var result = source.CreateMousePan(input);
            result.RenderOrder = renderConfig[EventType.MousePan];
            return result;
        }

        public IUIEvent CreateMouseScroll(float x, float y, bool scroolUp)
        {
            var result = source.CreateMouseScroll(x, y, scroolUp);
            result.RenderOrder = renderConfig[EventType.MouseScroll];
            return result;
        }

        public IUIEvent CreateMouseUpClearRender()
        {
            var result = source.CreateMouseUpClearRender();
            result.RenderOrder = renderConfig[EventType.MouseupClearRender];
            return result;
        }

        public IUIEvent CreateMouseZoom(InputState input)
        {
            var result = source.CreateMouseZoom(input);
            result.RenderOrder = renderConfig[EventType.MouseZoom];
            return result;
        }

        public IUIEvent CreatePlottableDrag(float x, float y, bool shiftDown, IDraggable draggable)
        {
            var result = source.CreatePlottableDrag(x, y, shiftDown, draggable);
            result.RenderOrder = renderConfig[EventType.PlottableDrag];
            return result;
        }
    }
}
