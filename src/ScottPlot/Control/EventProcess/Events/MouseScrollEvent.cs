namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes a zoom operation performed by scrolling the mouse wheel.
    /// </summary>
    public class MouseScrollEvent : IUIEvent
    {
        private float x;
        private float y;
        private bool scrolledUp;
        private Configuration config;
        private Settings settings;

        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public MouseScrollEvent(float x, float y, bool scrolledUp, Configuration config, Settings settings)
        {
            this.x = x;
            this.y = y;
            this.scrolledUp = scrolledUp;
            this.config = config;
            this.settings = settings;
        }

        public void ProcessEvent()
        {
            double xFrac = scrolledUp ? 1.15 : 0.85;
            double yFrac = scrolledUp ? 1.15 : 0.85;

            if (config.LockHorizontalAxis)
                xFrac = 1;
            if (config.LockVerticalAxis)
                yFrac = 1;

            settings.AxesZoomTo(xFrac, yFrac, x, y);
        }
    }
}
