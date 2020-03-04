using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public interface IPlotDemo
    {
        string name { get; }
        string id { get; }
        string classPath { get; }
        string sourceCode { get; }
        string description { get; }
        void Render(Plot plt);
    }
}
