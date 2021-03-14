using ScottPlot.Control.EventProcess.Events;
using ScottPlot.Plottable;

namespace ScottPlot.Control.EventProcess
{
    /// <summary>
    /// This class takes details about interactions and builds them into event objects which can 
    /// be passed into the event processor for processing/rendering when the render queue is free.
    /// </summary>
    public class UIEventFactory
    {
        public readonly Plot Plot;
        public readonly Configuration Configuration;
        public readonly Settings Settings;

        public UIEventFactory(Configuration config, Settings settings, Plot plt)
        {
            Configuration = config;
            Settings = settings;
            Plot = plt;
        }

        public IUIEvent CreateBenchmarkToggle() =>
            new BenchmarkToggleEvent(Plot, Configuration);

        public IUIEvent CreateApplyZoomRectangleEvent(float x, float y) =>
            new ApplyZoomRectangleEvent(x, y, Configuration, Settings, Plot);

        public IUIEvent CreateMouseAutoAxis() =>
            new MouseAxisAutoEvent(Configuration, Settings, Plot);

        public IUIEvent CreateMouseMovedToZoomRectangle(float x, float y) =>
             new MouseMovedToZoomRectangle(x, y, Settings, Configuration);

        public IUIEvent CreateMousePan(InputState input) =>
            new MousePanEvent(input, Configuration, Settings);

        public IUIEvent CreateMouseScroll(float x, float y, bool scroolUp) =>
            new MouseScrollEvent(x, y, scroolUp, Configuration, Settings);

        public IUIEvent CreateMouseUpClearRender() =>
            new MouseUpClearRenderEvent(Configuration);

        public IUIEvent CreateMouseZoom(InputState input) =>
            new MouseZoomEvent(input, Configuration, Settings, Plot);

        public IUIEvent CreatePlottableDrag(float x, float y, bool shiftDown, IDraggable draggable) =>
             new PlottableDragEvent(x, y, shiftDown, draggable, Plot, Configuration);

        public IUIEvent CreateManualLowQualityRender() => new RenderLowQuality();

        public IUIEvent CreateManualHighQualityRender() => new RenderHighQuality();

        public IUIEvent CreateManualDelayedHighQualityRender() => new RenderDelayedHighQuality();
    }
}
