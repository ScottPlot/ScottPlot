namespace ScottPlot.Interfaces;

public interface IInteractivePlottable
{
    public InteractiveNode? GetNode(CoordinateRect rect);
    public void MouseDown(InteractiveNode node);
    public void MouseMove(InteractiveNode node, Coordinates point);
    public void MouseUp(InteractiveNode node);
}
