namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes what happens when the mouse button is lifted after 
    /// middle-click-dragging a rectangle to zoom into. The coordinates of that rectangle
    /// are calculated, and the plot's axis limits are adjusted accordingly.
    /// </summary>
    public class ApplyZoomRectangleEvent : IUIEvent
    {
        private readonly float X;
        private readonly float Y;
        private readonly Configuration Configuration;
        private readonly Settings Settings;
        private readonly Plot Plot;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDropped;

        public ApplyZoomRectangleEvent(float x, float y, Configuration config, Settings settings, Plot plt)
        {
            X = x;
            Y = y;
            Configuration = config;
            Settings = settings;
            Plot = plt;
        }

        public void ProcessEvent()
        {
            Settings.RecallAxisLimits();

            var originalLimits = Plot.GetAxisLimits();

            Settings.MouseZoomRect(X, Y, finalize: true);

            if (Configuration.LockHorizontalAxis)
                Plot.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (Configuration.LockVerticalAxis)
                Plot.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);
        }
    }
}
