namespace ScottPlot.AxisRules;

public class SquareZoomOut(IXAxis xAxis, IYAxis yAxis) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the DataRect must wait for the layout to occur
        if (beforeLayout)
            return;

        double unitsPerPxX = XAxis.Width / rp.DataRect.Width;
        double unitsPerPxY = YAxis.Height / rp.DataRect.Height;
        double maxUnitsPerPx = Math.Max(unitsPerPxX, unitsPerPxY);

        double halfHeight = rp.DataRect.Height / 2 * maxUnitsPerPx;
        double yMin = YAxis.Range.Center - halfHeight;
        double yMax = YAxis.Range.Center + halfHeight;

        var invertedY = YAxis.Min > YAxis.Max;
        if (invertedY)
            YAxis.Range.Set(yMax, yMin);
        else
            YAxis.Range.Set(yMin, yMax);

        double halfWidth = rp.DataRect.Width / 2 * maxUnitsPerPx;
        double xMin = XAxis.Range.Center - halfWidth;
        double xMax = XAxis.Range.Center + halfWidth;

        var invertedX = XAxis.Min > XAxis.Max;
        if (invertedX)
            XAxis.Range.Set(xMax, xMin);
        else
            XAxis.Range.Set(xMin, xMax);
    }
}
