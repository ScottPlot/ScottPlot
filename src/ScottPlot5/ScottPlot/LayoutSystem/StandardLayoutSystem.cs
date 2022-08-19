namespace ScottPlot.LayoutSystem;

public class StandardLayoutSystem : ILayoutSystem
{
    public PixelRect GetDataAreaRect(PixelRect figureRect, IEnumerable<Axis.IXAxis> xAxes, IEnumerable<Axis.IYAxis> yAxes)
    {
        PixelPadding padding = new();

        foreach (Axis.IXAxis xAxis in xAxes)
        {
            xAxis.TickGenerator.Regenerate(xAxis.Range, figureRect.Width);
            float yPx = xAxis.Measure();

            if (xAxis.Edge == Edge.Bottom)
            {
                padding.Bottom += yPx;
            }
            else if (xAxis.Edge == Edge.Top)
            {
                padding.Top += yPx;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported edge: {xAxis.Edge}");
            }
        }

        foreach (Axis.IYAxis yAxis in yAxes)
        {
            yAxis.TickGenerator.Regenerate(yAxis.Range, figureRect.Height);
            float xPx = yAxis.Measure();

            if (yAxis.Edge == Edge.Left)
            {
                padding.Left += xPx;
            }
            else if (yAxis.Edge == Edge.Right)
            {
                padding.Right += xPx;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported edge: {yAxis.Edge}");
            }
        }

        padding.Right += 20;
        padding.Top += 20;

        return figureRect.Contract(padding);
    }
}
