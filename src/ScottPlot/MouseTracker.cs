using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public class MouseTracker
    {
        public Stopwatch mouseDownStopwatch = new Stopwatch();
        private Plottable draggingObject;

        private readonly Settings settings;
        public MouseTracker(Settings settings)
        {
            this.settings = settings;
        }

        public bool IsDraggingSomething()
        {
            if (settings.mouseIsPanning)
                return true;
            if (settings.mouseIsZooming)
                return true;
            if (draggingObject != null)
                return true;
            else
                return false;
        }

        public void MouseDown(System.Windows.Point mousePoint)
        {
            Point eLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            MouseDown(eLocation);
        }

        public void MouseDown(Point eLocation)
        {
            draggingObject = PlottableUnderCursor(eLocation);
            if (draggingObject == null)
            {
                mouseDownStopwatch.Restart();
                if (Control.MouseButtons == MouseButtons.Left)
                    settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, panning: true);
                else if (Control.MouseButtons == MouseButtons.Right)
                    settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, zooming: true);
            }
            else
            {
                PointF newPosition = settings.GetLocation(eLocation.X, eLocation.Y);
            }
        }

        public void MouseMove(System.Windows.Point mousePoint)
        {
            Point eLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            MouseMove(eLocation);
        }

        public void MouseMove(Point eLocation)
        {
            if (draggingObject == null)
            {
                settings.MouseMoveAxis(Cursor.Position.X, Cursor.Position.Y);
            }
            else
            {
                PointF newPosition = settings.GetLocation(eLocation.X, eLocation.Y);
                if (draggingObject is PlottableAxLine axLine)
                {
                    if (axLine.vertical)
                        axLine.position = newPosition.X;
                    else
                        axLine.position = newPosition.Y;

                    if (axLine.position < axLine.dragLimitLower)
                        axLine.position = axLine.dragLimitLower;
                    else if (axLine.position > axLine.dragLimitUpper)
                        axLine.position = axLine.dragLimitUpper;
                }
            }
        }

        public void MouseUp(System.Windows.Point mousePoint)
        {
            Point eLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            MouseUp(eLocation);
        }

        public void MouseUp(Point eLocation)
        {
            if (draggingObject == null)
            {
                settings.MouseMoveAxis(Cursor.Position.X, Cursor.Position.Y);
                settings.MouseUpAxis();
            }
            else
            {
                PointF newPosition = settings.GetLocation(eLocation.X, eLocation.Y);
                draggingObject = null;
            }
        }

        public Plottable PlottableUnderCursor(Point eLocation)
        {
            // adjust pixel location to correspond to data frame
            eLocation.X -= settings.dataOrigin.X;
            eLocation.Y -= settings.dataOrigin.Y;

            for (int i = 0; i < settings.plottables.Count; i++)
            {
                if (settings.plottables[i] is PlottableAxLine axLine)
                {
                    if (axLine.draggable == false)
                        continue;

                    if (axLine.vertical == true)
                    {
                        PointF linePosPx = settings.GetPixel(axLine.position, 0);
                        if (Math.Abs(linePosPx.X - eLocation.X) < 5)
                            return axLine;
                    }
                    else
                    {
                        PointF linePosPx = settings.GetPixel(0, axLine.position);
                        if (Math.Abs(linePosPx.Y - eLocation.Y) < 5)
                            return axLine;
                    }
                }
            }

            return null;

        }
    }
}
