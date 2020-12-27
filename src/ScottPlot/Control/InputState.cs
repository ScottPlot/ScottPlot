using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class InputState
    {
        public float X = float.NaN;
        public float Y = float.NaN;
        public bool LeftWasJustPressed = false;
        public bool RightWasJustPressed = false;
        public bool MiddleWasJustPressed = false;
        public bool ButtonDown => LeftWasJustPressed || RightWasJustPressed || MiddleWasJustPressed;
        public bool ShiftDown = false;
        public bool CtrlDown = false;
        public bool AltDown = false;
    }
}
