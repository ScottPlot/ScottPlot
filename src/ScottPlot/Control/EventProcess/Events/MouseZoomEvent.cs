using System;

namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event describes represents interactive zooming.
    /// It is assume the plot has already been reset to the pre-mouse-interaction state,
    /// and processing of this event zooms the plot on the axes according to the distance
    /// the mouse has moved.
    /// This is typically called on MouseMove events when the right button is held down.
    /// </summary>
    public class MouseZoomEvent : IUIEvent
    {
        private InputState input;
        private Configuration config;
        private Settings settings;
        private Plot plt;
        public RenderType RenderOrder { get; set; } = RenderType.HQAfterLQDelayed;

        public MouseZoomEvent(InputState input, Configuration config, Settings settings, Plot plt)
        {
            this.input = input;
            this.config = config;
            this.settings = settings;
            this.plt = plt;
        }

        public void ProcessEvent()
        {
            var originalLimits = plt.GetAxisLimits();

            if (input.ShiftDown && input.CtrlDown)
            {
                float dx = input.X - settings.MouseDownX;
                float dy = settings.MouseDownY - input.Y;
                float delta = Math.Max(dx, dy);
                settings.MouseZoom(settings.MouseDownX + delta, settings.MouseDownY - delta);
            }
            else
            {
                float x = input.ShiftDown ? settings.MouseDownX : input.X;
                float y = input.CtrlDown ? settings.MouseDownY : input.Y;
                settings.MouseZoom(x, y);
            }

            if (config.LockHorizontalAxis)
                plt.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (config.LockVerticalAxis)
                plt.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);
        }
    }
}
