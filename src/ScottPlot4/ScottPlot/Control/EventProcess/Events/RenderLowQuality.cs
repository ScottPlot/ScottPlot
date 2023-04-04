namespace ScottPlot.Control.EventProcess.Events;

class RenderLowQuality : IUIEvent
{
    public RenderType RenderType => RenderType.LowQuality;

    public void ProcessEvent()
    {
    }
}
