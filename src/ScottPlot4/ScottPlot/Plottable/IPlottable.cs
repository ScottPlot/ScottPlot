namespace ScottPlot.Plottable
{
    public interface IPlottable
    {
        bool IsVisible { get; set; }
        void Render(PlotDimensions dims, System.Drawing.Bitmap bmp, bool lowQuality = false);

        int XAxisIndex { get; set; }
        int YAxisIndex { get; set; }

        /// <summary>
        /// Returns items to show in the legend. Most plottables return a single item. in this array will appear in the legend.
        /// Plottables which never appear in the legend should return an empty array (not null).
        /// </summary>
        LegendItem[] GetLegendItems();

        /// <summary>
        /// Return min and max of the horizontal and vertical data contained in this plottable.
        /// Double.NaN is used for axes not containing data.
        /// </summary>
        /// <returns></returns>
        AxisLimits GetAxisLimits();

        /// <summary>
        /// Throw InvalidOperationException if ciritical variables are null or have incorrect sizes. 
        /// Deep validation is slower but also checks every value for NaN and Infinity.
        /// </summary>
        void ValidateData(bool deep = false);
    }
}
