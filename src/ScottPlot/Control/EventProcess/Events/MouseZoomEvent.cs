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
        private readonly InputState Input;
        private readonly Configuration Configuration;
        private readonly Settings Settings;
        private readonly Plot Plot;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDragged;

        public MouseZoomEvent(InputState input, Configuration config, Settings settings, Plot plt)
        {
            Input = input;
            Configuration = config;
            Settings = settings;
            Plot = plt;
        }

        public void ProcessEvent()
        {
            var originalLimits = Plot.GetAxisLimits();

            if (Input.ShiftDown && Input.CtrlDown)
            {
                float dx = Input.X - Settings.MouseDownX;
                float dy = Settings.MouseDownY - Input.Y;
                float delta = Math.Max(dx, dy);
                Settings.MouseZoom(Settings.MouseDownX + delta, Settings.MouseDownY - delta);
            }
            else
            {
                Settings.RecallAxisLimits();

                float dx = Settings.MouseDownX - Input.X;
                float dy = Input.Y - Settings.MouseDownY;

                double xFrac = 1 - (dx / 95);
                double yFrac = 1 - (dy / 95);

                if (Input.CtrlDown) yFrac = 1;
                if (Input.ShiftDown) xFrac = 1;

                Settings.AxesZoomTo(xFrac <= 0 ? 0.1 : xFrac, yFrac <= 0 ? 0.1 : yFrac, Settings.MouseDownX, Settings.MouseDownY);
            }

            if (Configuration.LockHorizontalAxis)
                Plot.SetAxisLimitsX(originalLimits.XMin, originalLimits.XMax);

            if (Configuration.LockVerticalAxis)
                Plot.SetAxisLimitsY(originalLimits.YMin, originalLimits.YMax);
        }
    }
}
