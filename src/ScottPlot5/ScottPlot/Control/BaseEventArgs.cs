using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class BaseEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}
