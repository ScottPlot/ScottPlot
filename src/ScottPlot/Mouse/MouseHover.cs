namespace ScottPlot.Mouse
{
    public class MouseHover : IMouseState
    {
        public float X { get; set; }
        public float Y { get; set; }

        public bool IsCtrlPressed { get; set; }
        public bool IsAltPressed { get; set; }
        public bool IsShiftPressed { get; set; }
    }
}
