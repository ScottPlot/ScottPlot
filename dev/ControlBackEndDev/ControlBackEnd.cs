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
        public readonly ScottPlot.Plot Plot;
        public readonly ScottPlot.Settings Settings;
        private Bitmap Bmp;
        private readonly List<Bitmap> OldBitmaps = new List<Bitmap>();

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

        public void MouseDown(InputState input)
        {
            Settings.MouseDown(input.X, input.Y);
        }

        public void MouseMove(InputState input)
        {
            bool isPanning = input.LeftDown && !input.AltDown;
            bool isZooming = input.RightDown;
            bool isZoomingRectangle = input.MiddleDown || (input.LeftDown && input.AltDown);

            bool needsRender = isPanning || isZooming || isZoomingRectangle;
            if (needsRender == false)
                return;

            float x = input.X;
            float y = input.Y;

            if (isPanning)
            {
                x = input.ShiftDown ? Settings.MouseDownX : x;
                y = input.CtrlDown ? Settings.MouseDownY : y;
                Settings.MousePan(x, y);
            }
            else if (isZooming)
            {
                if (input.ShiftDown && input.CtrlDown)
                {
                    float px = Math.Max(x - Settings.MouseDownX, -(y - Settings.MouseDownY));
                    Settings.MouseZoom(Settings.MouseDownX + px, Settings.MouseDownY - px);
                }
                else
                {
                    x = input.ShiftDown ? Settings.MouseDownX : x;
                    y = input.CtrlDown ? Settings.MouseDownY : y;
                    Settings.MouseZoom(x, y);
                }
            }
            else if (isZoomingRectangle)
            {
                Settings.MouseZoomRect(input.X, input.Y);
            }

            Render(lowQuality: true);
        }

        public void MouseUp(InputState input)
        {
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
