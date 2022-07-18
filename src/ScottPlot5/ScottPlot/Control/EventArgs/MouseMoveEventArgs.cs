using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class MouseMoveEventArgs : BaseEventArgs
    {
        public Pixel Position { get; }
        public IReadOnlyCollection<Key> PressedKeys {get; }

        public MouseMoveEventArgs(Pixel position, IReadOnlyCollection<Key> pressedKeys)
        {
            Position = position;
            PressedKeys = pressedKeys;
        }
    }
}
