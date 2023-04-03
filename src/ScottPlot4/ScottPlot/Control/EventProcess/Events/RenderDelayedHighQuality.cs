namespace ScottPlot.Control.EventProcess.Events
{
    class RenderDelayedHighQuality : IUIEvent
    {
        public RenderType RenderType => RenderType.LowQualityThenHighQualityDelayed;

        public void ProcessEvent()
        {
        }
    }
}
