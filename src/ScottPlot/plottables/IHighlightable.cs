using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.plottables
{
    interface IHighlightable
    {
        void HighlightPoint(int index);
        void HighlightPointNearestX(double x);
        void HighlightPointNearest(double x, double y);
        void HighlightClear();
    }
}
