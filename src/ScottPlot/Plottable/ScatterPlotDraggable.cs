using System;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The scatter plot renders X/Y pairs as points and/or connected lines.
    /// Scatter plots can be extremely slow for large datasets, so use Signal plots in these situations.
    /// </summary>
    public class ScatterPlotDraggable : ScatterPlot, IDraggable
    {
        public int CurrentIndex { get; set; } = 0;
        public double[] XsSorted { get; private set; }
        public double[] YsSorted { get; private set; }
        /// <summary>
        /// Indicates whether scatter points are draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = false;

        /// <summary>
        /// Indicates whether scatter points are horizontally draggable in user controls.
        /// </summary>
        public bool DragEnabledX { get; set; } = true;

        /// <summary>
        /// Indicates whether scatter points are vertically draggable in user controls.
        /// </summary>
        public bool DragEnabledY { get; set; } = true;

        /// <summary>
        /// Cursor to display while hovering over the scatter points if dragging is enabled.
        /// </summary>
        public ScottPlot.Cursor DragCursor { get; set; } = ScottPlot.Cursor.Crosshair;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax = double.PositiveInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax = double.PositiveInfinity;

        /// <summary>
        /// Indicates whether scatter point horizontal drag is restricted to the space between the two adjacent X coordinates in user controls.
        /// </summary>
        public bool DragBoxedX { get; set; } = false;

        /// <summary>
        /// Indicates whether scatter point vertical drag is restricted to the space between the two adjacent Y coordinates in user controls.
        /// </summary>
        public bool DragBoxedY { get; set; } = false;

        /// <summary>
        /// This event is invoked after the plot is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// Move a scatter point to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">new X position</param>
        /// <param name="coordinateY">new Y position</param>
        /// <param name="fixedSize">This argument is ignored</param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;
            if (CurrentIndex > PointCount - 1) CurrentIndex = PointCount - 1;
            if (coordinateX < DragXLimitMin) coordinateX = DragXLimitMin;
            if (coordinateX > DragXLimitMax) coordinateX = DragXLimitMax;
            if (coordinateX < DragYLimitMin) coordinateY = DragYLimitMin;
            if (coordinateX > DragYLimitMax) coordinateY = DragYLimitMax;
            if (DragEnabled)
            {
                int SortedCurrentIndex = Array.IndexOf(XsSorted, Xs[CurrentIndex]);
                if (DragEnabledX && DragBoxedX)
                {
                    double lower_bound = SortedCurrentIndex == 0 ? XsSorted[0] : XsSorted[SortedCurrentIndex - 1];
                    double upper_bound = SortedCurrentIndex == PointCount - 1 ? XsSorted[PointCount - 1] : XsSorted[SortedCurrentIndex + 1];
                    coordinateX = CompareToBox(coordinateX, lower_bound, upper_bound);
                }
                if (DragEnabledY && DragBoxedY)
                {
                    double lower_bound = SortedCurrentIndex == 0 ? YsSorted[0] : YsSorted[SortedCurrentIndex - 1];
                    double upper_bound = SortedCurrentIndex == PointCount - 1 ? YsSorted[PointCount - 1] : YsSorted[SortedCurrentIndex + 1];
                    coordinateY = CompareToBox(coordinateY, lower_bound, upper_bound);
                }
                if (DragEnabledX)
                {
                    Xs[CurrentIndex] = coordinateX;
                    XsSorted[SortedCurrentIndex] = coordinateX;
                }
                if (DragEnabledY)
                {
                    Ys[CurrentIndex] = coordinateY;
                    YsSorted[SortedCurrentIndex] = coordinateY;
                }
            }

            Dragged(this, EventArgs.Empty);
        }

        private (double, double) SwapBoundsIfNeeded(double lower_bound, double upper_bound)
        {
            if (upper_bound < lower_bound)
            {
                return (upper_bound, lower_bound);
            }
            else
            {
                return (lower_bound, upper_bound);
            }
        }

        private double CompareToBox(double coordinate, double lower_bound, double upper_bound)
        {
            (lower_bound, upper_bound) = SwapBoundsIfNeeded(lower_bound, upper_bound);
            if (coordinate >= upper_bound) return upper_bound;
            if (coordinate <= lower_bound) return lower_bound;
            return coordinate;
        }

        /// <summary>
        /// Return True if a scatter point is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            bool test = false;
            for (int i = 0; i < PointCount; i++)
            {
                test = Math.Abs(Ys[i] - coordinateY) <= snapY && Math.Abs(Xs[i] - coordinateX) <= snapX;
                if (test)
                {
                    CurrentIndex = i;
                    return test;
                }
            }
            return test;
        }

        public ScatterPlotDraggable(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null) : base(xs, ys, errorX, errorY)
        {
            int[] permutationinds = Enumerable.Range(0, PointCount).ToArray();
            Array.Sort<int>(permutationinds, (a, b) => Xs[a].CompareTo(Xs[b]));
            XsSorted = new double[PointCount];
            YsSorted = new double[PointCount];
            for (int i = 0; i < PointCount; i++)
            {
                XsSorted[i] = Xs[permutationinds[i]];
                YsSorted[i] = Ys[permutationinds[i]];
            }
        }
    }
}
