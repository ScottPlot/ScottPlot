namespace ScottPlot.Plottable
{
    /// <summary>
    /// Every plottable object must implement this interface.
    /// Additional features are provided by adjacent interfaces.
    /// </summary>
    public interface IPlottable
    {
        /// <summary>
        /// Controls whether the plot will be rendered and contribute to automatic axis limit detection
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Index of the horizontal axis this plottable will use for coordinate/pixel conversions.
        /// 0 is the bottom axis, 1 is the top axis, and higher numbers are additional custom axes.
        /// </summary>
        int XAxisIndex { get; set; }

        /// <summary>
        /// Index of the vertical axis this plottable will use for coordinate/pixel conversions.
        /// 0 is the left axis, 1 is the right axis, and higher numbers are additional custom axes.
        /// </summary>
        int YAxisIndex { get; set; }

        /// <summary>
        /// This is called when it is time to draw the plottable on the canvas.
        /// </summary>
        /// <param name="dims">Spatial information about the plot and all axes to assist with coordinate/pixel conversions.</param>
        /// <param name="bmp">The image on which this plottable will be drawn.</param>
        /// <param name="lowQuality">If true, disable anti-aliased lines and text to achieve faster rendering.</param>
        void Render(PlotDimensions dims, System.Drawing.Bitmap bmp, bool lowQuality = false);
    }
}
