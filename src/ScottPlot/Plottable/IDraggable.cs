namespace ScottPlot.Plottable
{
    public interface IDraggable
    {
        bool DragEnabled { get; set; }
        Ticks.Cursor DragCursor { get; }
        bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY);
        void DragTo(double coordinateX, double coordinateY, bool isShiftDown, bool isAltDown, bool isCtrlDown);
    }
}
