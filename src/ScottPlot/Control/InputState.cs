using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class InputState
    {
        public float X = float.NaN;
        public float Y = float.NaN;
        public bool LeftDown = false;
        public bool RightDown = false;
        public bool MiddleDown = false;
        public bool ButtonDown => LeftDown || RightDown || MiddleDown;
        public bool ShiftDown = false;
        public bool CtrlDown = false;
        public bool AltDown = false;
    }
}
