using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    public class DraggableMarkerPlot : MarkerPlot, IDraggable
    {
        /// <summary>
        /// Indicates whether this marker is draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = true;

        /// <summary>
        /// Cursor to display while hovering over this marker if dragging is enabled.
        /// </summary>
        public Cursor DragCursor { get; set; } = Cursor.Hand;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax = double.PositiveInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the marker cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the marker is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// Move the marker to a new coordinate in plot space.
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
            X = coordinateX;
            Y = coordinateY;
            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if the marker is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY) => Math.Abs(Y - coordinateY) <= snapY && Math.Abs(X - coordinateX) <= snapX;
    }

    public class DraggableMarkerPlotInVector : ScatterPlot, IDraggable
    {
        public new double[] Xs { get; private set; }
        public new double[] Ys { get; private set; }
        public int CurrentIndex { get; set; } = 0;

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

        /// <summary>
        /// The lower bound of the axis line.
        /// </summary>
        public double Min = double.NegativeInfinity;

        /// <summary>
        /// The upper bound of the axis line.
        /// </summary>
        public double Max = double.PositiveInfinity;

        public new void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                PointF point = new PointF(dims.GetPixelX(Xs[CurrentIndex]), dims.GetPixelY(Ys[CurrentIndex]));

                MarkerTools.DrawMarker(gfx, point, MarkerShape.cross, (float)MarkerSize * 10, Color.Red);
            }
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

        public DraggableMarkerPlotInVector(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null) : base(xs, ys, errorX, errorY)
        {
            this.Xs = xs;
            this.Ys = ys;
            this.XError = errorX;
            this.YError = errorY;
        }
    }
}
