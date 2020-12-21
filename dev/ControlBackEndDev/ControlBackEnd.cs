/* This file describes the ScottPlot back-end control module.
 * 
 *  Goals for this module:
 *    - handle interact with the Plot object so controls don't have to
 *    - single location for mouse interaction logic so controls don't have to implement it
 *    - use events to tell controls when to update the image or change the mouse cursor
 *    - TODO: render calls should be non-blocking so GUI/controls aren't slowed by render requests
 *    - TODO: move this module into the ScottPlot project
 *    - TODO: a timer should ask for a high quality render after mouse interaction stops
 *   
 *   Default Controls:
 *   
 *    - Left-click-drag: pan
 *    - Right-click-drag: zoom
 *    - Middle-click-drag: zoom region
 *    - ALT+Left-click-drag: zoom region
 *    - Scroll wheel: zoom to cursor
 *   
 *    - Right-click: show menu
 *    - Middle-click: auto-axis
 *    - Double-click: show benchmark
 *   
 *    - CTRL+Left-click-drag to pan horizontally
 *    - SHIFT+Left-click-drag to pan vertically
 *    - CTRL+Right-click-drag to zoom horizontally
 *    - SHIFT+Right-click-drag to zoom vertically
 *    - CTRL+SHIFT+Right-click-drag to zoom evenly
 *    - SHIFT+click-drag draggables for fixed-size dragging
 *   
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBackEndDev
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

    public class ControlBackEnd
    {
        public event EventHandler BitmapUpdated = delegate { };
        public event EventHandler BitmapChanged = delegate { };
        public event EventHandler CursorChanged = delegate { };

        public readonly ScottPlot.Plot Plot;
        public readonly ScottPlot.Settings Settings;
        private Bitmap Bmp;
        private readonly List<Bitmap> OldBitmaps = new List<Bitmap>();
        public ScottPlot.Cursor Cursor { get; private set; } = ScottPlot.Cursor.Arrow;

        public ControlBackEnd(float width, float height)
        {
            Plot = new ScottPlot.Plot();
            Settings = Plot.GetSettings(false);
            Resize(width, height);
            NewBitmap(600, 400);
        }

        public Bitmap GetLatestBitmap()
        {
            foreach (Bitmap bmp in OldBitmaps)
                bmp?.Dispose();
            OldBitmaps.Clear();
            return Bmp;
        }

        private void NewBitmap(float width, float height)
        {
            if (width < 1 || height < 1)
                return;

            // Disposing a Bitmap the GUI is displaying will cause an exception.
            // Keep track of old bitmaps so they can be disposed of later.
            OldBitmaps.Add(Bmp);
            Bmp = new Bitmap((int)width, (int)height);
            BitmapChanged(this, EventArgs.Empty);
        }

        public void Render(bool lowQuality)
        {
            Plot.Render(Bmp, lowQuality);
            BitmapUpdated(null, EventArgs.Empty);
        }

        public void Resize(float width, float height)
        {
            NewBitmap(width, height);
            Render(false);
        }

        private ScottPlot.Plottable.IDraggable PlottableBeingDragged = null;
        public void MouseDown(InputState input)
        {
            PlottableBeingDragged = Plot.GetDraggableUnderMouse(input.X, input.Y);
            Settings.MouseDown(input.X, input.Y);
        }

        public void MouseMove(InputState input)
        {
            if (PlottableBeingDragged != null)
                MouseMovedToDragPlottable(input);
            else if (input.LeftDown && !input.AltDown)
                MouseMovedToPan(input);
            else if (input.RightDown)
                MouseMovedToZoom(input);
            else if (input.MiddleDown || (input.LeftDown && input.AltDown))
                MouseMovedToZoomRectangle(input);
            else
                MouseMovedWithoutInteraction(input);
        }

        private void MouseMovedToDragPlottable(InputState input)
        {
            double x = Plot.GetCoordinateX(input.X);
            double y = Plot.GetCoordinateY(input.Y);
            PlottableBeingDragged.DragTo(x, y, fixedSize: input.ShiftDown);
            Render(lowQuality: true);
        }

        private void MouseMovedToPan(InputState input)
        {
            float x = input.ShiftDown ? Settings.MouseDownX : input.X;
            float y = input.CtrlDown ? Settings.MouseDownY : input.Y;
            Settings.MousePan(x, y);
            Render(lowQuality: true);
        }

        private void MouseMovedToZoom(InputState input)
        {
            if (input.ShiftDown && input.CtrlDown)
            {
                float dx = input.X - Settings.MouseDownX;
                float dy = Settings.MouseDownY - input.Y;
                float delta = Math.Max(dx, dy);
                Settings.MouseZoom(Settings.MouseDownX + delta, Settings.MouseDownY - delta);
            }
            else
            {
                float x = input.ShiftDown ? Settings.MouseDownX : input.X;
                float y = input.CtrlDown ? Settings.MouseDownY : input.Y;
                Settings.MouseZoom(x, y);
            }
            Render(lowQuality: true);
        }

        private void MouseMovedToZoomRectangle(InputState input)
        {
            Settings.MouseZoomRect(input.X, input.Y);
            Render(lowQuality: true);
        }

        private void MouseMovedWithoutInteraction(InputState input)
        {
            UpdateCursor(input);
        }

        private void UpdateCursor(InputState input)
        {
            var draggableUnderCursor = Plot.GetDraggableUnderMouse(input.X, input.Y);
            Cursor = (draggableUnderCursor is null) ? ScottPlot.Cursor.Arrow : draggableUnderCursor.DragCursor;
            CursorChanged(null, EventArgs.Empty);
        }

        public void MouseUp(InputState input)
        {
            PlottableBeingDragged = null;

            bool isZoomingRectangle = input.MiddleDown || (input.LeftDown && input.AltDown);
            if (isZoomingRectangle)
            {
                if (Settings.MouseHasMoved(input.X, input.Y))
                {
                    Settings.RecallAxisLimits();
                    Settings.MouseZoomRect(input.X, input.Y, finalize: true);
                }
                else
                {
                    MiddleClickAutoAxis();
                }
            }

            Render(false);
            UpdateCursor(input);
        }

        private void MiddleClickAutoAxis()
        {
            Settings.ZoomRectangle.Clear();
            Plot.AxisAuto();
            Render(false);
        }

        public void MouseWheel(InputState input, bool wheelUp)
        {
            double xFrac = wheelUp ? 1.15 : 0.85;
            double yFrac = wheelUp ? 1.15 : 0.85;
            Settings.AxesZoomTo(xFrac, yFrac, input.X, input.Y);
            Render(false);
        }

        public void DoubleClick()
        {
            Plot.BenchmarkToggle();
            Render(false);
        }
    }
}
