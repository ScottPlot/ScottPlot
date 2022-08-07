using System;
using System.Collections.Generic;
using ScottPlot.SnapLogic;

namespace ScottPlot.Plottable
{
    public class ScatterPlotListDraggable : ScatterPlotList<double>, IDraggable
    {
        private int IndexUnderMouse { get; set; } = -1;
        public bool DragEnabled { get; set; } = true;

        public Cursor DragCursor => Cursor.Hand;

        public event EventHandler Dragged = delegate { };

        public ISnap2D DragSnap { get; set; } = new NoSnap2D();

        /// <summary>
        /// Assign custom the logic here to control where individual points can be moved.
        /// This logic occurs after snapping.
        /// </summary>
        public Func<List<double>, List<double>, int, Coordinate, Coordinate> MovePointFunc { get; set; } = (xs, ys, index, moveTo) => moveTo;

        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled || IndexUnderMouse < 0)
                return;

            Coordinate requested = new(coordinateX, coordinateY);
            Coordinate snapped = DragSnap.Snap(requested);
            Coordinate actual = MovePointFunc(Xs, Ys, IndexUnderMouse, snapped);
            Xs[IndexUnderMouse] = actual.X;
            Ys[IndexUnderMouse] = actual.Y;

            Dragged(this, EventArgs.Empty);
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            for (int i = 0; i < Count; i++)
            {
                double dX = Math.Abs(NumericConversion.GenericToDouble(Xs, i) - coordinateX);
                double dY = Math.Abs(NumericConversion.GenericToDouble(Ys, i) - coordinateY);

                if (dX <= snapX && dY <= snapY)
                {
                    IndexUnderMouse = i;
                    return true;
                }
            }

            IndexUnderMouse = -1;
            return false;
        }
    }
}
