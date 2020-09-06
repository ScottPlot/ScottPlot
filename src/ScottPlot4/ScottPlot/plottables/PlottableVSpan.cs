﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableVSpan : Plottable, IDraggable
    {
        public double position1;
        public double position2;
        public Brush brush;
        public Color color;
        public string label;

        public PlottableVSpan(double position1, double position2, Color color, double alpha, string label,
            bool draggable, bool dragFixedSize, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            brush = new SolidBrush(this.color);

            DragEnabled = draggable;
            DragFixedSize = dragFixedSize;

            SetLimits(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableVSpan{label} from Y={position1} to Y={position2}";
        }

        public override AxisLimits2D GetLimits()
        {
            // TODO: use real numbers (and double.NaN)
            return new AxisLimits2D();
        }

        public override void Render(Settings settings)
        {
            PointF topLeft, lowerRight;

            double positionMin = Math.Min(position1, position2);
            double positionMax = Math.Max(position1, position2);

            topLeft = settings.GetPixel(settings.axes.x.min, positionMin);
            lowerRight = settings.GetPixel(settings.axes.x.max, positionMax);
            if (topLeft.Y > settings.bmpData.Height)
                topLeft.Y = settings.bmpData.Height;
            if (lowerRight.Y < 0)
                lowerRight.Y = 0;

            float width = lowerRight.X - topLeft.X + 1;
            float height = topLeft.Y - lowerRight.Y + 1;
            float x = topLeft.X - 1;
            float y = lowerRight.Y - 1;

            settings.gfxData.FillRectangle(brush, x, y, width, height);
        }

        public bool DragEnabled { get; set; }
        public bool DragFixedSize { get; set; }

        private double dragLimitY1 = double.NegativeInfinity;
        private double dragLimitY2 = double.PositiveInfinity;
        public void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            if (y1 != null) dragLimitY1 = (double)y1;
            if (y2 != null) dragLimitY2 = (double)y2;
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateY) <= snapY)
                edgeUnderMouse = Edge.Edge1;
            else if (Math.Abs(position2 - coordinateY) <= snapY)
                edgeUnderMouse = Edge.Edge2;
            else
                edgeUnderMouse = Edge.Neither;

            return (edgeUnderMouse == Edge.Neither) ? false : true;
        }

        public void DragTo(double coordinateX, double coordinateY, bool isShiftDown = false, bool isAltDown = false, bool isCtrlDown = false)
        {
            if (DragEnabled)
            {
                if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;

                double sizeBeforeDrag = position2 - position1;
                if (edgeUnderMouse == Edge.Edge1)
                {
                    position1 = coordinateY;
                    if (DragFixedSize || isShiftDown)
                        position2 = position1 + sizeBeforeDrag;
                }
                else if (edgeUnderMouse == Edge.Edge2)
                {
                    position2 = coordinateY;
                    if (DragFixedSize || isShiftDown)
                        position1 = position2 - sizeBeforeDrag;
                }
                else
                {
                    Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
                }
            }
        }

        public override int GetPointCount()
        {
            return 1;
        }

        public Cursor DragCursor => Cursor.NS;

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new Config.LegendItem(label, color, markerSize: 0, lineWidth: 10);
            return new LegendItem[] { singleLegendItem };
        }
    }
}
