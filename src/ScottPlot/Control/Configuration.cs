using System;

namespace ScottPlot.Control
{
    public class Configuration
    {
        /// <summary>
        /// Control whether panning is enabled
        /// </summary>
        public bool Pan { get => LeftClickDragPan; set => LeftClickDragPan = value; }

        /// <summary>
        /// Control whether zooming is enabled (via left-click-drag, middle-click-drag, and scrollwheel)
        /// </summary>
        public bool Zoom
        {
            get => RightClickDragZoom;
            set => (RightClickDragZoom, MiddleClickDragZoom, ScrollWheelZoom) = (value, value, value);
        }

        /// <summary>
        /// Manual override to set anti-aliasing (high quality) behavior for all renders.
        /// Refer to the QualityConfiguration field for more control over quality in response to specific interactions.
        /// </summary>
        public QualityMode Quality = QualityMode.LowWhileDragging;

        /// <summary>
        /// This module customizes anti-aliasing (high quality) behavior in response to interactive events.
        /// </summary>
        public readonly QualityConfiguration QualityConfiguration = new();

        /// <summary>
        /// Control whether left-click-drag panning is enabled
        /// </summary>
        public bool LeftClickDragPan = true;

        /// <summary>
        /// Control whether right-click-drag zooming is enabled
        /// </summary>
        public bool RightClickDragZoom = true;

        /// <summary>
        /// Control whether scroll wheel zooming is enabled
        /// </summary>
        public bool ScrollWheelZoom = true;

        private double _scrollWheelZoomFraction = 0.15;

        /// <summary>
        /// Fractional amount to zoom in or out when the mouse wheel is scrolled.
        /// Value must be between 0 and 1 (default is 0.15).
        /// </summary>
        public double ScrollWheelZoomFraction
        {
            get => _scrollWheelZoomFraction;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ScrollWheelZoomFraction", "must be positive");
                if (value >= 1)
                    throw new ArgumentOutOfRangeException("ScrollWheelZoomFraction", "must be less than 1");

                _scrollWheelZoomFraction = value;
            }
        }

        /// <summary>
        /// Number of milliseconds after low quality scroll wheel zoom to re-render using high quality
        /// </summary>
        public double ScrollWheelZoomHighQualityDelay = 500;

        /// <summary>
        /// Control whether middle-click-drag zooming to a rectangle is enabled
        /// </summary>
        public bool MiddleClickDragZoom = true;

        /// <summary>
        /// Control whether middle-click can be used to reset axis limits
        /// </summary>
        public bool MiddleClickAutoAxis = true;

        /// <summary>
        /// Horizontal margin between the edge of the data and the edge of the plot when middle-click AutoAxis is called
        /// </summary>
        public double MiddleClickAutoAxisMarginX = .05;

        /// <summary>
        /// Vertical margin between the edge of the data and the edge of the plot when middle-click AutoAxis is called
        /// </summary>
        public double MiddleClickAutoAxisMarginY = .1;

        /// <summary>
        /// If enabled, double-clicking the plot will toggle benchmark visibility
        /// </summary>
        public bool DoubleClickBenchmark = true;

        /// <summary>
        /// If enabled, the vertical axis limits cannot be modified by mouse actions
        /// </summary>
        public bool LockVerticalAxis = false;

        /// <summary>
        /// If enabled, the horizontal axis limits cannot be modified by mouse actions
        /// </summary>
        public bool LockHorizontalAxis = false;

        /// <summary>
        /// Controls whether or not a render event will be triggered if a change in the number of plottables is detected
        /// </summary>
        public bool RenderIfPlottableCountChanges = true;

        /// <summary>
        /// Controls whether or not a render event will be triggered if a change in the axis limits is detected
        /// </summary>
        public bool AxesChangedEventEnabled = true;

        /// <summary>
        /// Permitting dropped frames makes interactive mouse manipulation feel faster
        /// </summary>
        public bool AllowDroppedFramesWhileDragging = true;

        /// <summary>
        /// If true, control interactions will be non-blocking and renders will occur after interactions.
        /// If false, control interactions will be blocking while renders are drawn.
        /// </summary>
        public bool UseRenderQueue = false;

        /// <summary>
        /// Distance (in pixels) the mouse can travel with a button held-down for it to be treated as a click (not a drag).
        /// A number slightly above zero allows middle-click to call AxisAuto() even if it was draged a few pixels by accident.
        /// </summary>
        public int IgnoreMouseDragDistance = 5;
    }
}
