using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    public interface IInputState
    {
        public float X { get; }
        public float Y { get; }
        public bool LeftWasJustPressed { get; }
        public bool RightWasJustPressed { get; }
        public bool MiddleWasJustPressed { get; }
        public bool ButtonDown { get; }
        public bool ShiftDown { get; }
        public bool CtrlDown { get; }
        public bool AltDown { get; }
        public bool WheelScrolledUp { get; }
        public bool WheelScrolledDown { get; }
    }
}
