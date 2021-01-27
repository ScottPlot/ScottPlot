using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control
{
    public class DPICorrectedInputState : IInputState
    {
        public InputState input { private get; set; }
        public float DPIFactor { private get; set; }

        public DPICorrectedInputState(InputState input, float DPIFactor)
        {
            this.input = input;
            this.DPIFactor = DPIFactor;
        }


        public float X => input.X * DPIFactor;

        public float Y => input.Y * DPIFactor;

        public bool LeftWasJustPressed => input.LeftWasJustPressed;

        public bool RightWasJustPressed => input.RightWasJustPressed;

        public bool MiddleWasJustPressed => input.MiddleWasJustPressed;

        public bool ButtonDown => input.ButtonDown;

        public bool ShiftDown => input.ShiftDown;

        public bool CtrlDown => input.CtrlDown;

        public bool AltDown => input.AltDown;

        public bool WheelScrolledUp => input.WheelScrolledUp;

        public bool WheelScrolledDown => input.WheelScrolledDown;
    }
}
