using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class KeyDownEventArgs : BaseEventArgs
    {
        public Key Key { get; }

        public KeyDownEventArgs(Key key)
        {
            Key = key;
        }
    }
}
