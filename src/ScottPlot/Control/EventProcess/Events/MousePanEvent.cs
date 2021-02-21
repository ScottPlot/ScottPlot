namespace ScottPlot.Control.EventProcess.Events
{
    public class MousePanEvent : IUIEvent
    {
        private InputState input;
        private Configuration config;
        private Settings settings;
        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public MousePanEvent(InputState input, Configuration config, Settings settings)
        {
            this.input = input;
            this.config = config;
            this.settings = settings;
        }

        public void ProcessEvent()
        {
            float x = (input.ShiftDown || config.LockHorizontalAxis) ? settings.MouseDownX : input.X;
            float y = (input.CtrlDown || config.LockVerticalAxis) ? settings.MouseDownY : input.Y;
            settings.MousePan(x, y);
        }
    }
}
