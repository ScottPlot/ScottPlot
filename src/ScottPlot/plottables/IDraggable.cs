using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public interface IDraggable
    {
        void DragEnable(bool enable);
        void DragLimit(double? x1, double? x2, double? y1, double? y2);

        bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);

        void DragStart(double coordinateX, double coordinateY);
        void DragTo(double coordinateX, double coordinateY);
        void DragEnd(double coordinateX, double coordinateY);
        bool IsBeingDragged();

        Config.Cursor GetDragCursor();
    }
}
