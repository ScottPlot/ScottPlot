namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event calls AxisAuto() on all axes.
    /// This is typically called after middle-clicking.
    /// </summary>
    public class MouseAxisAutoEvent : IUIEvent
    {
        private Configuration config;
        private Settings settings;
        private Plot plt;
        public RenderType RenderOrder { get; set; } = RenderType.HQOnly;

        public MouseAxisAutoEvent(Configuration config, Settings settings, Plot plt)
        {
            this.config = config;
            this.settings = settings;
            this.plt = plt;
        }

        public void ProcessEvent()
        {
            settings.ZoomRectangle.Clear();

            if (config.LockVerticalAxis == false)
                plt.AxisAutoY(config.MiddleClickAutoAxisMarginY);

            if (config.LockHorizontalAxis == false)
                plt.AxisAutoX(config.MiddleClickAutoAxisMarginX);
        }
    }
}
