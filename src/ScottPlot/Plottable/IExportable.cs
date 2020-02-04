using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    /// <summary>
    /// describes Plottable objects whose data can be exported as a text file
    /// </summary>
    public interface IExportable
    {
        void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n");
        string GetCSV(string delimiter = ", ", string separator = "\n");
    }
}
