namespace ScottPlot.Control.EventProcess.Events
{
    public class MouseUpClearRenderEvent : IUIEvent
    {
        public RenderType RenderOrder { get; set; } = RenderType.HQOnly;

        public void ProcessEvent()
        {
        }
    }
}
