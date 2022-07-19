using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control.EventArgs
{
    public class MouseDragEventArgs : BaseEventArgs
    {
        public MouseDownEventArgs MouseDown { get; }
        public Pixel From => MouseDown.Position;
        public IReadOnlyCollection<Key> PressedKeys => MouseDown.PressedKeys;
        public Pixel To { get; }
        public MouseButton Button { get; }
        public float DragDistance => (MouseDown.Position - To).Hypotenuse;

        public MouseDragEventArgs(MouseDownEventArgs mouseDown, Pixel to, MouseButton button)
        {
            MouseDown = mouseDown;
            To = to;
            Button = button;
        }
    }
}
