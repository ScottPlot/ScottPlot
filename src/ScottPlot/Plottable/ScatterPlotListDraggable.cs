using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class ScatterPlotListDraggable : ScatterPlotList<double>, IDraggable
    {
        private int IndexUnderMouse = -1;
        public bool DragEnabled { get; set; } = true;

        public Cursor DragCursor => Cursor.Hand;

        public event EventHandler Dragged = delegate { };

        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled || IndexUnderMouse < 0)
                return;

            Xs[IndexUnderMouse] = coordinateX;
            Ys[IndexUnderMouse] = coordinateY;

            Dragged(this, EventArgs.Empty);
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            for (int i = 0; i < Count; i++)
            {
                double dX = Math.Abs(Convert.ToDouble(Xs[i]) - coordinateX);
                double dY = Math.Abs(Convert.ToDouble(Ys[i]) - coordinateY);

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
