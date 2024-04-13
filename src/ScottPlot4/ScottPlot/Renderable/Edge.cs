namespace ScottPlot.Renderable;

public enum Edge { Left, Right, Bottom, Top };

public static class EdgeExtensions
{
    public static bool IsVertical(this Edge edge) => edge == Edge.Left || edge == Edge.Right;
    public static bool IsHorizontal(this Edge edge) => edge == Edge.Bottom || edge == Edge.Top;
}
