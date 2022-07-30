namespace ScottPlot.Control;

public struct LockedAxes
{
    public bool X = false;
    public bool Y = false;

    public LockedAxes(bool lockedX, bool lockedY)
    {
        X = lockedX;
        Y = lockedY;
    }
}
