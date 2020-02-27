using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public interface IPlotDemo
    {
        string name { get; }
        string description { get; }
        void Render(Plot plt);
    }
}
