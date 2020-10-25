using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot
{
    public interface IPlottable // TODO: inherit IRenderable
    {
        bool IsVisible { get; set; } // TODO: move into IRenderable
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false); // TODO: inherit from IRenderable
        int PointCount { get; }
        LegendItem[] LegendItems { get; } // TODO: interface segregation
        AxisLimits2D GetLimits();
        string ErrorMessage(bool deepValidation = false); // TODO: interface segregation
    }
}
