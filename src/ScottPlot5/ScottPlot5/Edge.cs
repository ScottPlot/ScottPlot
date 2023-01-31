namespace ScottPlot;

public enum Edge
{
    Left,
    Right,
    Bottom,
    Top
}

public static class EdgeExtensions
{
    /// <summary>
    /// True for bottom and top axes
    /// </summary>
    public static bool IsHorizontal(this Edge edge) => edge switch
    {
        Edge.Left => false,
        Edge.Right => false,
        Edge.Bottom => true,
        Edge.Top => true,
        _ => throw new NotImplementedException(edge.ToString())
    };

    /// <summary>
    /// True for left and right axes
    /// </summary>
    public static bool IsVertical(this Edge edge) => edge switch
    {
        Edge.Left => true,
        Edge.Right => true,
        Edge.Bottom => false,
        Edge.Top => false,
        _ => throw new NotImplementedException(edge.ToString())
    };
}
