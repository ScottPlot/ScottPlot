namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event is called after the mouse button is lifted (typically following panning and zooming).
    /// It assumes all the axis manipulation (panning/zooming) has already been performed,
    /// and the purpose of this event is only to request an immediate high quality render.
    /// </summary>
    public class MouseUpClearRenderEvent : IUIEvent
    {
        private readonly Configuration Configuration;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDropped;

        public MouseUpClearRenderEvent(Configuration config)
        {
            Configuration = config;
        }

        public void ProcessEvent()
        {
        }
    }
}
