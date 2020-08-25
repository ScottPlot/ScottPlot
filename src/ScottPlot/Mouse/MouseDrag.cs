namespace ScottPlot.Mouse
{
    public class MouseDrag : IMouseState, IMouseDrag
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float DeltaX { get { return X2 - X; } }
        public float DeltaY { get { return Y2 - Y; } }

        public Plottable PlottableBeingDragged { get; set; }

        public bool IsCtrlPressed { get; set; }
        public bool IsAltPressed { get; set; }
        public bool IsShiftPressed { get; set; }
    }
}
