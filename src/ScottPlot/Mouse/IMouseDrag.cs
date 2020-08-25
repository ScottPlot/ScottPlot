namespace ScottPlot.Mouse
{
    public interface IMouseDrag
    {
        float X2 { get; set; }
        float Y2 { get; set; }

        float DeltaX { get; }
        float DeltaY { get; }
    }
}
