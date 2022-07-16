﻿using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{

    /// <summary>
    /// This plot type displays a marker at a point that can be dragged with the mouse,
    /// but when dragged it "snaps" to specific X/Y coordinates defined by two arrays of values.
    /// </summary>
    public class DraggableMarkerPlotInVector : IDraggable, IPlottable, IHasMarker
    {
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        /// <summary>
        /// Horizontal position in coordinate space
        /// </summary>
        public double[] Xs { get; set; }

        /// <summary>
        /// Vertical position in coordinate space
        /// </summary>
        public double[] Ys { get; set; }

        public int PointCount => Xs.Length;

        public int CurrentIndex { get; set; } = 0;
        /// <summary>
        /// Marker to draw at this point
        /// </summary>
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Size of the marker in pixel units
        /// </summary>
        public float MarkerSize { get; set; } = 10;

        /// <summary>
        /// Color of the marker to display at this point
        /// </summary>
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.Black;

        /// <summary>
        /// Color of the marker to display at this point
        /// </summary>
        public System.Drawing.Color MarkerColor { get => Color; set { Color = value; } }

        /// <summary>
        /// Width of the marker lines in pixel units
        /// </summary>
        public float MarkerLineWidth { get; set; } = 1;

        /// <summary>
        /// Text to appear in the legend (if populated)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Indicates whether this marker on the scatter plot is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = true;

        /// <summary>
        /// Cursor to display while hovering over this marker on the scatter plot if dragging is enabled.
        /// </summary>
        public Cursor DragCursor => Cursor.Crosshair;

        /// <summary>
        /// If dragging is enabled the marker on the scatter plot cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker on the scatter plot cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax = double.PositiveInfinity;

        /// <summary>
        /// If dragging is enabled the marker on the scatter plot cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker on the scatter plot cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the marker on the scatter plot is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements(nameof(Xs), Xs);
            Validate.AssertHasElements(nameof(Ys), Ys);
            Validate.AssertEqualLength($"{nameof(Xs)} and {nameof(Ys)}", Xs, Ys);
            if (deep)
            {
                Validate.AssertAllReal(nameof(Xs), Xs);
                Validate.AssertAllReal(nameof(Ys), Ys);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using var gfx = GDI.Graphics(bmp, dims, lowQuality);

            PointF point = new(dims.GetPixelX(Xs[CurrentIndex]), dims.GetPixelY(Ys[CurrentIndex]));

            MarkerTools.DrawMarker(gfx, point, MarkerShape, (float)MarkerSize, Color, MarkerLineWidth);
        }

        /// <summary>
        /// Move the line to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">new X position</param>
        /// <param name="coordinateY">new Y position</param>
        /// <param name="fixedSize">This argument is ignored</param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;

            if (coordinateX < DragXLimitMin) coordinateX = DragXLimitMin;
            if (coordinateX > DragXLimitMax) coordinateX = DragXLimitMax;
            if (coordinateX < DragYLimitMin) coordinateY = DragYLimitMin;
            if (coordinateX > DragYLimitMax) coordinateY = DragYLimitMax;

            var distancessq = new double[PointCount];
            for (int i = 0; i < PointCount; i++)
            {
                distancessq[i] = Math.Pow(Xs[i] - coordinateX, 2) + Math.Pow(Ys[i] - coordinateY, 2);
            }
            for (int i = 0; i < PointCount; i++)
            {
                if (distancessq[i] < distancessq[CurrentIndex])
                {
                    CurrentIndex = i;
                }
            }

            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if the line is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY) => Math.Abs(Ys[CurrentIndex] - coordinateY) <= snapY && Math.Abs(Xs[CurrentIndex] - coordinateX) <= snapX;

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                markerShape = MarkerShape,
                markerSize = MarkerSize,
                color = Color,
            };
            return new LegendItem[] { singleItem };
        }
    }
}
