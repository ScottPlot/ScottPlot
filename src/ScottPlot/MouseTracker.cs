﻿using System;
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
        private Plottable plottableBeingDragged;
        public bool lowQualityWhileInteracting = true;
        public int mouseWheelHQRenderDelay = 500;
        public int mouseUpHQRenderDelay = 300;
        public bool enablePanning = true;
        public bool enableZooming = true;

        private readonly Settings settings;
        public MouseTracker(Settings settings)
        {
            this.settings = settings;
        }

        public bool IsDraggingSomething()
        {
            if (settings.mouse.isPanning)
                return true;
            if (settings.mouse.isZooming)
                return true;
            if (plottableBeingDragged != null)
                return true;
            else
                return false;
        }

        public bool ctrlIsDown()
        {
            bool leftCtrl = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl);
            bool rightCtrl = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl);
            return (leftCtrl || rightCtrl);
        }

        public bool altIsDown()
        {
            bool leftAlt = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt);
            bool rightAlt = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightAlt);
            return (leftAlt || rightAlt);
        }

        public bool shiftIsDown()
        {
            bool leftShift = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift);
            bool rightShift = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift);
            return (leftShift || rightShift);
        }

        public void MouseDown(System.Windows.Point mousePoint)
        {
            Point eLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            MouseDown(eLocation);
        }

        public bool MouseHasMoved()
        {
            if (settings.mouse.downLoc.X != Cursor.Position.X)
                return true;
            else if (settings.mouse.downLoc.Y != Cursor.Position.Y)
                return true;
            else
                return false;
        }

        public void MouseDown(Point eLocation)
        {
            plottableBeingDragged = PlottableUnderCursor(eLocation);
            if (plottableBeingDragged == null)
            {
                mouseDownStopwatch.Restart();
                if ((Control.MouseButtons == MouseButtons.Left) && (enablePanning))
                    settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, panning: true);
                else if ((Control.MouseButtons == MouseButtons.Right) && (enableZooming))
                    settings.MouseDown(Cursor.Position.X, Cursor.Position.Y, zooming: true);
            }

            if (Control.MouseButtons == MouseButtons.Middle)
                settings.mouse.downMiddle = eLocation;
        }

        public void MouseMove(System.Windows.Point mousePoint)
        {
            Point eLocation = new Point((int)mousePoint.X, (int)mousePoint.Y);
            MouseMove(eLocation);
        }

        public void MouseMove(Point eLocation)
        {
            if (plottableBeingDragged == null)
            {
                if ((Control.MouseButtons == MouseButtons.Left) || (Control.MouseButtons == MouseButtons.Right))
                {
                    // left-click-dragging or right-click-dragging
                    settings.MouseMoveAxis(Cursor.Position.X, Cursor.Position.Y, ctrlIsDown(), altIsDown());
                }
                else if (Control.MouseButtons == MouseButtons.Middle)
                {
                    // middlg-click-dragging
                    settings.MouseZoomRectMove(eLocation);
                }
            }
            else
            {
                PointF newPosition = settings.GetLocation(eLocation.X, eLocation.Y);
                if (plottableBeingDragged is PlottableAxLine axLine)
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
            if (plottableBeingDragged == null)
            {
                settings.MouseMoveAxis(Cursor.Position.X, Cursor.Position.Y, ctrlIsDown(), altIsDown());
                settings.MouseUpAxis();
            }
            else
            {
                PointF newPosition = settings.GetLocation(eLocation.X, eLocation.Y);
                plottableBeingDragged = null;
            }

            if (settings.mouse.rectangleIsHappening)
            {
                int[] xs = new int[] { settings.mouse.downMiddle.X, settings.mouse.currentLoc.X };
                int[] ys = new int[] { settings.mouse.downMiddle.Y, settings.mouse.currentLoc.Y };
                var lowerLeft = settings.GetLocation(xs.Min(), ys.Max());
                var upperRight = settings.GetLocation(xs.Max(), ys.Min());
                settings.axes.Set(lowerLeft.X, upperRight.X, lowerLeft.Y, upperRight.Y);
                settings.mouse.rectangleIsHappening = false;
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

        public bool MouseIsOverHorizontalAxis(Point loc)
        {
            int x1 = settings.dataOrigin.X;
            int x2 = x1 + settings.dataSize.Width;
            int y1 = settings.dataOrigin.Y + settings.dataSize.Height;
            int y2 = y1 + (int)settings.ticks.y.maxLabelSize.Height;
            if ((loc.X < x1) || (loc.X > x2))
                return false;
            if ((loc.Y < y1) || (loc.Y > y2))
                return false;
            return true;
        }

        public bool MouseIsOverVerticalAxis(Point loc)
        {
            int x1 = settings.dataOrigin.X - (int)settings.ticks.y.maxLabelSize.Width;
            int x2 = settings.dataOrigin.X;
            int y1 = settings.dataOrigin.Y;
            int y2 = settings.dataOrigin.Y + settings.dataSize.Height;
            if ((loc.X < x1) || (loc.X > x2))
                return false;
            if ((loc.Y < y1) || (loc.Y > y2))
                return false;
            return true;
        }

        public bool MouseIsOverDataArea(Point loc)
        {
            int x1 = settings.dataOrigin.X;
            int x2 = settings.dataOrigin.X + settings.dataSize.Width;
            int y1 = settings.dataOrigin.Y;
            int y2 = settings.dataOrigin.Y + settings.dataSize.Height;
            if ((loc.X < x1) || (loc.X > x2))
                return false;
            if ((loc.Y < y1) || (loc.Y > y2))
                return false;
            return true;
        }

        public void MiddleButtonClicked()
        {
            if (!settings.mouse.rectangleIsHappening)
                settings.AxisAuto();
        }

        public void MouseIs(bool panning, bool zooming)
        {
            settings.mouse.isPanning = panning;
            settings.mouse.isZooming = zooming;
        }
    }
}
