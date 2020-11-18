using System;

namespace ScottPlot
{
    partial class Plot
    {
        /// <summary>
        /// Set padding around the data area by defining the minimum size and padding for all axes
        /// </summary>
        public void Layout(float? left = null, float? right = null, float? bottom = null, float? top = null, float? padding = 5)
        {
            foreach (var axis in settings.Axes)
            {
                if (axis.Edge == Renderable.Edge.Left) axis.PixelSizeMinimum = left ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Right) axis.PixelSizeMinimum = right ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Bottom) axis.PixelSizeMinimum = bottom ?? axis.PixelSizeMinimum;
                if (axis.Edge == Renderable.Edge.Top) axis.PixelSizeMinimum = top ?? axis.PixelSizeMinimum;
                axis.PixelSizePadding = padding ?? axis.PixelSizePadding;
            }
        }

        /// <summary>
        /// Disable visibility of all axes and set their size and padding to zero so the data area covers the whole figure
        /// </summary>
        public void LayoutFrameless()
        {
            foreach (var axis in settings.Axes)
            {
                axis.IsVisible = false;
                axis.PixelSizeMinimum = 0;
                axis.PixelSizeMaximum = 0;
                axis.PixelSizePadding = 0;
            }
        }

        /// <summary>
        /// Automatically adjust layout based on axis label and tick label size
        /// </summary>
        [Obsolete("This function is no longer required. Use Layout() to manualy define padding.", true)]
        public void TightenLayout(int? padding = null) { }

        /// <summary>
        /// Set padding around the data area
        /// </summary>
        [Obsolete("Use the other Layout() method", true)]
        public void Layout(
                double? yLabelWidth = null,
                double? yScaleWidth = null,
                double? y2LabelWidth = null,
                double? y2ScaleWidth = null,
                double? titleHeight = null,
                double? xLabelHeight = null,
                double? xScaleHeight = null
            )
        { }
    }
}
