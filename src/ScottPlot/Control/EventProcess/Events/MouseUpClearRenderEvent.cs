namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event is called after the mouse button is lifted.
    /// It assumes all the axis manipulation (panning/zooming) has already been performed,
    /// and the purpose of this event is only to request a render.
    /// </summary>
    public class MouseUpClearRenderEvent : IUIEvent
    {
        public RenderType RenderOrder { get; set; } = RenderType.HQOnly;

        public void ProcessEvent()
        {
        }
    }
}
