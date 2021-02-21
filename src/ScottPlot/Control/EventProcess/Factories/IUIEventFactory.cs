using ScottPlot.Plottable;

namespace ScottPlot.Control.EventProcess.Factories
{
    public interface IUIEventFactory
    {
        Configuration config { get; set; }
        Plot plt { get; set; }
        Settings settings { get; set; }

        IUIEvent CreateApplyZoomRectangleEvent(float x, float y);
        IUIEvent CreateBenchmarkToggle();
        IUIEvent CreateMouseAutoAxis();
        IUIEvent CreateMouseMovedToZoomRectangle(float x, float y);
        IUIEvent CreateMousePan(InputState input);
        IUIEvent CreateMouseScroll(float x, float y, bool scroolUp);
        IUIEvent CreateMouseUpClearRender();
        IUIEvent CreateMouseZoom(InputState input);
        IUIEvent CreatePlottableDrag(float x, float y, bool shiftDown, IDraggable draggable);
    }
}
