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
        private readonly InputState Input;
        private readonly Configuration Configuration;
        private readonly Settings Settings;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDragged;

        public MousePanEvent(InputState input, Configuration config, Settings settings)
        {
            Input = input;
            Configuration = config;
            Settings = settings;
        }

        public void ProcessEvent()
        {
            float x = (Input.ShiftDown || Configuration.LockHorizontalAxis) ? Settings.MouseDownX : Input.X;
            float y = (Input.CtrlDown || Configuration.LockVerticalAxis) ? Settings.MouseDownY : Input.Y;
            Settings.MousePan(x, y);
        }
    }
}
