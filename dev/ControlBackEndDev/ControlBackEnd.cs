/* This file describes the ScottPlot back-end control module.
 * 
 *  Goals for this module:
 *    - handle interact with the Plot object so controls don't have to
 *    - single location for mouse interaction logic so controls don't have to implement it
 *    - use events to tell controls when to update the image or change the mouse cursor
 *    - TODO: render calls should be non-blocking so GUI/controls aren't slowed by render requests
 *    - TODO: move this module into the ScottPlot project
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
    public class MouseState
    {
        public float X = float.NaN;
        public float Y = float.NaN;
        public bool LeftDown = false;
        public bool RightDown = false;
        public bool MiddleDown = false;
        public bool ButtonDown => LeftDown || RightDown || MiddleDown;
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

        public void MouseDown(MouseState mouse)
        {
            Settings.MouseDown(mouse.X, mouse.Y);
        }

        public void MouseMove(MouseState mouse)
        {
            if (mouse.ButtonDown)
            {
                if (mouse.LeftDown)
                    Settings.MousePan(mouse.X, mouse.Y);
                else if (mouse.RightDown)
                    Settings.MouseZoom(mouse.X, mouse.Y);
                else if (mouse.MiddleDown)
                    Settings.MouseZoomRect(mouse.X, mouse.Y);

                Render(lowQuality: true);
            }
        }

        public void MouseUp(MouseState mouse)
        {
            if (mouse.MiddleDown)
            {
                Settings.RecallAxisLimits();

                if (Settings.MouseHasMoved(mouse.X, mouse.Y) == false)
                {
                    MiddleClickAutoAxis();
                    return;
                }

                Settings.MouseZoomRect(mouse.X, mouse.Y, finalize: true);
                Render(false);
            }
        }

        private void MiddleClickAutoAxis()
        {
            Settings.ZoomRectangle.Clear();
            Plot.AxisAuto();
            Render(false);
        }

        public void MouseWheel(MouseState mouse, bool wheelUp)
        {
            double xFrac = wheelUp ? 1.15 : 0.85;
            double yFrac = wheelUp ? 1.15 : 0.85;
            Settings.AxesZoomTo(xFrac, yFrac, mouse.X, mouse.Y);
            Render(false);
        }

        public void DoubleClick()
        {
            Plot.BenchmarkToggle();
            Render(false);
        }
    }
}
