using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot
{
    public interface IPlottable
    {
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);
        bool IsValidData(bool deepValidation = false);
        string ValidationErrorMessage { get; }
        bool visible { get; set; }
        string ToString();
        Config.AxisLimits2D GetLimits();
        int GetPointCount();
        Config.LegendItem[] GetLegendItems();
    }
}
