using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public interface IExportable
    {
        void SaveCSV(string filePath);
    }
}
