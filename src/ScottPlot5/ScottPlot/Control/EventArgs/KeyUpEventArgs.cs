using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class KeyUpEventArgs : BaseEventArgs
    {
        public Key Key { get; }

        public KeyUpEventArgs(Key key)
        {
            Key = key;
        }
    }
}
