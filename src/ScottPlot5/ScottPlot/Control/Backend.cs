using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    public class Backend
    {
        private MouseInteraction? CurrentMouse1Down = null;
        private MouseInteraction? CurrentMouse2Down = null;
        private MouseInteraction? CurrentMouse3Down = null;
        private Pixel? LastMousePosition = null;

        private readonly Plot plot;
        private readonly object EventSender;
        private readonly Action requestRender;

        public delegate void MouseDownHandler(object sender, MouseInteraction e);
        public delegate void MouseMoveHandler(object sender, MouseInteraction e);
        public delegate void MouseUpHandler(object sender, MouseInteraction e);
        public delegate void MouseDragHandler(object sender, MouseDragInteraction e);

        public event MouseDownHandler MouseDown = delegate { };
        public event MouseMoveHandler MouseMove = delegate { };
        public event MouseUpHandler MouseUp = delegate { };
        public event MouseDragHandler MouseDrag = delegate { };
        public Backend(object sender, Plot plot, Action requestRender)
        {
            EventSender = sender;
            this.plot = plot;
            this.requestRender = requestRender;

            MouseDown += (object sender, MouseInteraction e) => DefaultEventHandlers.MouseDown(plot, e, requestRender);
            MouseUp += (object sender, MouseInteraction e) => DefaultEventHandlers.MouseUp(plot, e, requestRender);
            MouseMove += (object sender, MouseInteraction e) => DefaultEventHandlers.MouseMove(plot, e, requestRender);
            MouseDrag += (object sender, MouseDragInteraction e) => DefaultEventHandlers.MouseDrag(plot, e, requestRender);
        }

        public IEnumerable<MouseButton> GetPressedButtons()
        {
            if (CurrentMouse1Down.HasValue)
                yield return MouseButton.Mouse1;
            if (CurrentMouse2Down.HasValue)
                yield return MouseButton.Mouse2;
            if (CurrentMouse3Down.HasValue)
                yield return MouseButton.Mouse3;

            yield break;
        }

        public Coordinate? MouseCoordinates => LastMousePosition.HasValue ? plot.GetCoordinate(LastMousePosition.Value) : null;

        private MouseInteraction? GetMouseInteractionForButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Mouse1:
                    return CurrentMouse1Down;
                case MouseButton.Mouse2:
                    return CurrentMouse2Down;
                case MouseButton.Mouse3:
                    return CurrentMouse3Down;
                default:
                    throw new ArgumentException($"Unknown button: {button}");
            }
        }

        private void SetMouseInteractionForButton(MouseButton button, MouseInteraction? value)
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

        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            SetMouseInteractionForButton(button, new()
            {
                Position = position,
                Button = button,
                AxisLimits = plot.GetAxisLimits()
            });

            MouseDown?.Invoke(EventSender, new() { Position = position, Button = button });
        }

        public void TriggerMouseUp(Pixel position, MouseButton button)
        {
            SetMouseInteractionForButton(button, null);
            MouseUp?.Invoke(EventSender, new() { Position = position, Button = button });
        }

        public void TriggerMouseMove(Pixel position)
        {
            LastMousePosition = position;
            MouseMove?.Invoke(EventSender, new() { Position = position });
            
            for(MouseButton button = MouseButton.Mouse1; button <= MouseButton.Mouse3; button++)
            {
                var interaction = GetMouseInteractionForButton(button);
                if (interaction.HasValue)
                {
                    var lastMouseDown = interaction.Value;
                    if (IsDrag(lastMouseDown.Position, position))
                    {
                        TriggerMouseDrag(lastMouseDown, position, button);
                    }
                }
            }
        }

        private void TriggerMouseDrag(MouseInteraction MouseDown, Pixel to, MouseButton button)
        {
            LastMousePosition = to;
            MouseDrag?.Invoke(EventSender, new() { MouseDown = MouseDown, To = to, Button = button });
        }

        private bool IsDrag(Pixel from, Pixel to)
        {
            const int minDragDistance = 5;
            Pixel difference = from - to;

            return difference.X * difference.X + difference.Y * difference.Y > minDragDistance * minDragDistance;
        }
    }
}
