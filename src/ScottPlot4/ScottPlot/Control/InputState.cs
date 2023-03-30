using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class InputState
    {
        public float X { get; set; } = float.NaN;
        public float Y { get; set; } = float.NaN;
        public bool LeftWasJustPressed { get; set; } = false;
        public bool RightWasJustPressed { get; set; } = false;
        public bool MiddleWasJustPressed { get; set; } = false;
        public bool ButtonDown => LeftWasJustPressed || RightWasJustPressed || MiddleWasJustPressed;
        public bool ShiftDown { get; set; } = false;
        public bool CtrlDown { get; set; } = false;
        public bool AltDown { get; set; } = false;
        public bool WheelScrolledUp { get; set; } = false;
        public bool WheelScrolledDown { get; set; } = false;

        public static InputState Empty => new();
    }
}
