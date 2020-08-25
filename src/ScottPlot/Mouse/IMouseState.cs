namespace ScottPlot.Mouse
{
    public interface IMouseState
    {
        float X { get; set; }
        float Y { get; set; }

        bool IsCtrlPressed { get; set; }
        bool IsAltPressed { get; set; }
        bool IsShiftPressed { get; set; }
    }
}
