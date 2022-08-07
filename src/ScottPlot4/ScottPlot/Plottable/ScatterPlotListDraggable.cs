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

        public ISnap DragSnapX { get; set; } = new Smooth();

        public ISnap DragSnapY { get; set; } = new Smooth();

        public Func<List<double>, List<double>, int, Coordinate, Coordinate> MovePointFunc { get; set; } = (xs, ys, index, moveTo) => moveTo;

        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled || IndexUnderMouse < 0)
                return;

            coordinateX = DragSnapX.Snap(coordinateX);
            coordinateY = DragSnapY.Snap(coordinateY);

            Coordinate requested = new(coordinateX, coordinateY);
            Coordinate actual = MovePointFunc(Xs, Ys, IndexUnderMouse, requested);
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
