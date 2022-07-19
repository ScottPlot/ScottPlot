using ScottPlot.Control.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class helps wire-up mouse buttons to mouse events
    /// </summary>
    public class MouseButtonPanel
    {
        private MouseDownEventArgs? CurrentMouse1Down = null;
        private MouseDownEventArgs? CurrentMouse2Down = null;
        private MouseDownEventArgs? CurrentMouse3Down = null;

        public IEnumerable<MouseButton> GetPressedButtons()
        {
            if (CurrentMouse1Down is not null)
                yield return MouseButton.Mouse1;
            if (CurrentMouse2Down is not null)
                yield return MouseButton.Mouse2;
            if (CurrentMouse3Down is not null)
                yield return MouseButton.Mouse3;
        }

        public MouseDownEventArgs? GetMouseInteractionForButton(MouseButton button)
        {
            return button switch
            {
                MouseButton.Mouse1 => CurrentMouse1Down,
                MouseButton.Mouse2 => CurrentMouse2Down,
                MouseButton.Mouse3 => CurrentMouse3Down,
                _ => throw new ArgumentException($"Unknown button: {button}"),
            };
        }

        public void SetMouseInteractionForButton(MouseButton button, MouseDownEventArgs? value)
        {
            switch (button)
            {
                case MouseButton.Mouse1:
                    CurrentMouse1Down = value;
                    break;
                case MouseButton.Mouse2:
                    CurrentMouse2Down = value;
                    break;
                case MouseButton.Mouse3:
                    CurrentMouse3Down = value;
                    break;
                default:
                    throw new ArgumentException($"Unknown button: {button}");
            }
        }
    }
}
