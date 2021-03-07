namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes represents interactive panning.
    /// It is assume the plot has already been reset to the pre-mouse-interaction state,
    /// and processing of this event pans the plot on the axes according to the distance
    /// the mouse has moved.
    /// This is typically called on MouseMove events when the left button is held down.
    /// </summary>
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
