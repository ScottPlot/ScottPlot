namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes what happens when the mouse button is lifted after 
    /// middle-click-dragging a rectangle to zoom into. The coordinates of that rectangle
    /// are calculated, and the plot's axis limits are adjusted accordingly.
    /// </summary>
    public class ApplyZoomRectangleEvent : IUIEvent
    {
        private float x;
        private float y;
        private Configuration config;
        private Settings settings;
        private Plot plt;

        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public ApplyZoomRectangleEvent(float x, float y, Configuration config, Settings settings, Plot plt)
        {
            this.x = x;
            this.y = y;
            this.config = config;
            this.settings = settings;
            this.plt = plt;
        }


        public void ProcessEvent()
        {
            settings.RecallAxisLimits();

            var originalLimits = plt.GetAxisLimits();

            settings.MouseZoomRect(x, y, finalize: true);

            if (config.LockHorizontalAxis)
                plt.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (config.LockVerticalAxis)
                plt.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);
        }
    }
}
