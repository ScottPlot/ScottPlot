using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class BaseEventArgs : System.EventArgs
    {
        public bool Handled { get; set; }
    }
}
