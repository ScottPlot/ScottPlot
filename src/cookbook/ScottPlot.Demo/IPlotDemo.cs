using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public interface IPlotDemo
    {
        string name { get; }
        string description { get; }

        string classPath { get; }
        string sourceFile { get; }
        string categoryMajor { get; }
        string categoryMinor { get; }
        string categoryClass { get; }

        string id { get; }

        void Render(Plot plt);
        string GetSourceCode(string pathDemoFolder);
    }
}
