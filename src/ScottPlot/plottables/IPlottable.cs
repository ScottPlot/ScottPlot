using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot
{
    [Obsolete("Use IPlottable", true)]
    public abstract class Plottable { }

    public interface IPlottable
    {
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);
        string ErrorMessage(bool deepValidation = false);
        bool IsVisible { get; set; }
        AxisLimits2D GetLimits();
        int GetPointCount();
        LegendItem[] GetLegendItems();
    }
}
