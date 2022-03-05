namespace ScottPlot.Control.EventProcess
{
    public interface IUIEvent
    {
        public RenderType RenderType { get; }
        void ProcessEvent();
    }
}
