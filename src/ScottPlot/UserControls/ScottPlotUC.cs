using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// code in this file reduces the pain associated with renaming the ScottPlotUC user control (in version 3.1.0)
// https://github.com/swharden/ScottPlot/issues/96

namespace ScottPlot
{
    [System.ComponentModel.ToolboxItem(false)]
    [Obsolete("ScottPlotUC has been renamed to FormsPlot", true)]
    public partial class ScottPlotUC : FormsPlot { }

    [System.ComponentModel.ToolboxItem(false)]
    [Obsolete("ScottPlotWPF has been renamed to WpfPlot", true)]
    public partial class ScottPlotWPF : WpfPlot { }
}
