using ScottPlot.Plottable;

namespace ScottPlot.Control.EventProcess.Events
{
    public class PlottableDragEvent : IUIEvent
    {
        private float x;
        private float y;
        private IDraggable plottable;
        private Plot plt;
        private bool shiftDown;
        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public PlottableDragEvent(float x, float y, bool shiftDown, IDraggable plottable, Plot plt)
        {
            this.x = x;
            this.y = y;
            this.shiftDown = shiftDown;
            this.plt = plt;
            this.plottable = plottable;
        }

        public void ProcessEvent()
        {
            double xCoord = plt.GetCoordinateX(x);
            double yCoord = plt.GetCoordinateY(y);
            plottable.DragTo(xCoord, yCoord, fixedSize: shiftDown);
        }
    }
}
