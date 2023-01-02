using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    public struct ContextMenuItem
    {
        public string Header { get; set; }
        public Action OnInvoke { get; set; }
    }
}
