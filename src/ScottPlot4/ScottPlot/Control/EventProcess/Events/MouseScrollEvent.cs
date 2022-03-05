namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes a zoom operation performed by scrolling the mouse wheel.
    /// </summary>
    public class MouseScrollEvent : IUIEvent
    {
        private readonly float X;
        private readonly float Y;
        private readonly bool ScrolledUp;
        private readonly Configuration Configuration;
        private readonly Settings Settings;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseWheelScrolled;

        public MouseScrollEvent(float x, float y, bool scrolledUp, Configuration config, Settings settings)
        {
            X = x;
            Y = y;
            ScrolledUp = scrolledUp;
            Configuration = config;
            Settings = settings;
        }

        public void ProcessEvent()
        {
            double increment = 1.0 + Configuration.ScrollWheelZoomFraction;
            double decrement = 1.0 - Configuration.ScrollWheelZoomFraction;

            double xFrac = ScrolledUp ? increment : decrement;
            double yFrac = ScrolledUp ? increment : decrement;

            if (Configuration.LockHorizontalAxis)
                xFrac = 1;
            if (Configuration.LockVerticalAxis)
                yFrac = 1;

            Settings.AxesZoomTo(xFrac, yFrac, X, Y);
        }
    }
}
