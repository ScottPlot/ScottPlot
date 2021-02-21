namespace ScottPlot.Control.EventProcess.Events
{
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
