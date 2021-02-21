using ScottPlot.Control.EventProcess.Events;
using ScottPlot.Plottable;

namespace ScottPlot.Control.EventProcess.Factories
{
    public class UIEventFactory : IUIEventFactory
    {
        public Plot plt { get; set; }
        public Configuration config { get; set; }
        public Settings settings { get; set; }

        public UIEventFactory(Configuration config, Settings settings, Plot plt)
        {
            this.config = config;
            this.settings = settings;
            this.plt = plt;
        }

        public IUIEvent CreateBenchmarkToggle()
        {
            return new BenchmarkToggleEvent(plt);
        }
        public IUIEvent CreateApplyZoomRectangleEvent(float x, float y)
        {
            return new ApplyZoomRectangleEvent(x, y, config, settings, plt);
        }

        public IUIEvent CreateMouseAutoAxis()
        {
            return new MouseAxisAutoEvent(config, settings, plt);
        }

        public IUIEvent CreateMouseMovedToZoomRectangle(float x, float y)
        {
            return new MouseMovedToZoomRectangle(x, y, settings);
        }

        public IUIEvent CreateMousePan(InputState input)
        {
            return new MousePanEvent(input, config, settings);
        }

        public IUIEvent CreateMouseScroll(float x, float y, bool scroolUp)
        {
            return new MouseScrollEvent(x, y, scroolUp, config, settings);
        }

        public IUIEvent CreateMouseUpClearRender()
        {
            return new MouseUpClearRenderEvent();
        }

        public IUIEvent CreateMouseZoom(InputState input)
        {
            return new MouseZoomEvent(input, config, settings, plt);
        }

        public IUIEvent CreatePlottableDrag(float x, float y, bool shiftDown, IDraggable draggable)
        {
            return new PlottableDragEvent(x, y, shiftDown, draggable, plt);
        }
    }
}
