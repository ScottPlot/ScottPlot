namespace ScottPlot.Control.EventProcess
{
    public interface IUIEvent
    {
        public RenderType RenderOrder { get; set; }
        void ProcessEvent();
    }
}
