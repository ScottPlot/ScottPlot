using ScottPlot.Axis;
using System.Linq;

namespace ScottPlot.Stylers;

public class AxisStyler
{
    readonly Plot Plot;

    public AxisStyler(Plot plot)
    {
        Plot = plot;
    }

    public IAxis[] GetAxes(Edge edge)
    {
        return Plot.XAxes.Select(x => (IAxis)x)
            .Concat(Plot.YAxes)
            .Where(x => x.Edge == edge)
            .ToArray();
    }

    public IXAxis[] GetBottomAxes() => GetAxes(Edge.Bottom).Cast<IXAxis>().ToArray();
    public IXAxis[] GetTopAxes() => GetAxes(Edge.Top).Cast<IXAxis>().ToArray();
    public IYAxis[] GetLeftAxes() => GetAxes(Edge.Left).Cast<IYAxis>().ToArray();
    public IYAxis[] GetRightAxes() => GetAxes(Edge.Right).Cast<IYAxis>().ToArray();

    public void ClearAxes(Edge edge)
    {
        foreach (IAxis axis in GetAxes(edge).ToArray())
        {
            if (axis is IXAxis xAxis)
                Plot.XAxes.Remove(xAxis);

            if (axis is IYAxis yAxis)
                Plot.YAxes.Remove(yAxis);
        }
    }

    public void DateTimeTicks(Edge edge)
    {
        // replace the old axis with a new DateTime axis
        ClearAxes(edge);

        IXAxis dateAxis = edge switch
        {
            Edge.Left => throw new NotImplementedException(),
            Edge.Right => throw new NotImplementedException(),
            Edge.Bottom => new Axis.DateTimeAxes.DateTimeXAxis(),
            Edge.Top => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };

        Plot.XAxes.Add(dateAxis);

        foreach (IGrid grid in Plot.Grids)
        {
            grid.Replace(dateAxis);
        }
    }
}
