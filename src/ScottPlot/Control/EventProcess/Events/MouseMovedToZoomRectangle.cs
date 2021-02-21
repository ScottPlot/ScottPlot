namespace ScottPlot.Control.EventProcess.Events
{
    public class MouseMovedToZoomRectangle : IUIEvent
    {
        private float x;
        private float y;
        private Settings settings;
        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public MouseMovedToZoomRectangle(float x, float y, Settings settings)
        {
            this.x = x;
            this.y = y;
            this.settings = settings;
        }

        public void ProcessEvent()
        {
            settings.MouseZoomRect(x, y);
        }
    }
}
