namespace ScottPlot.Control.EventProcess
{
    public enum RenderType
    {
        LQOnly, // LQ Render Only
        HQOnly, // HQ Render Only
        HQAfterLQImmediately, // LQ render, if LastEvent - immediately HQ
        HQAfterLQDelayed, // LQ render, if last event HQ render after delay (if new Events not added)
        None, // Only process math, no render at all, if it last, previous event behavior will be used
    }
}
