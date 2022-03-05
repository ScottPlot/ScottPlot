namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event calls AxisAuto() on all axes.
    /// This is typically called after middle-clicking.
    /// </summary>
    public class MouseAxisAutoEvent : IUIEvent
    {
        private readonly Configuration Configuration;
        private readonly Settings Settings;
        private readonly Plot Plot;
        public RenderType RenderType => Configuration.QualityConfiguration.AutoAxis;

        public MouseAxisAutoEvent(Configuration config, Settings settings, Plot plt)
        {
            Configuration = config;
            Settings = settings;
            Plot = plt;
        }

        public void ProcessEvent()
        {
            Settings.ZoomRectangle.Clear();

            if (Configuration.LockVerticalAxis)
                return;

            Plot.AxisAuto();
        }
    }
}
