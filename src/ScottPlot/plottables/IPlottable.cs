using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    // TODO: Work toward strangling the Plottable class by impementing this interface 
    //       as plottables are refactored. Once no more plottables inherit from Plottable,
    //       move its abstract methods into this interface.
    public interface IPlottable
    {
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);

        bool IsValidData(bool deepValidation = false);
        string ValidationErrorMessage { get; }
    }
}
