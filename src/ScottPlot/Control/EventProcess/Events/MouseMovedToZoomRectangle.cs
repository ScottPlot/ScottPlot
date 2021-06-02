namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event occurs when the user is actively middle-click-dragging to zoom.
    /// A zoom window is drawn on the screen, but axis limits have not yet been changed.
    /// </summary>
    public class MouseMovedToZoomRectangle : IUIEvent
    {
        private readonly float X;
        private readonly float Y;
        private readonly Settings Settings;
        private readonly Configuration Configuration;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDragged;

        public MouseMovedToZoomRectangle(float x, float y, Settings settings, Configuration configuration)
        {
            X = x;
            Y = y;
            Settings = settings;
            Configuration = configuration;
        }

        public void ProcessEvent()
        {
            Settings.MouseZoomRect(X, Y);
        }
    }
}
