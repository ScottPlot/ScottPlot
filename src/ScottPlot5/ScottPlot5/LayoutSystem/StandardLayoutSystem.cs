using ScottPlot.Axis;
using System.Security.Cryptography;

namespace ScottPlot.LayoutSystem;

public class StandardLayoutSystem : ILayoutSystem
{
    public PixelRect AutoSizeDataArea(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        RegenerateTicksForAllAxes(figureRect, xAxes, yAxes);
        RemeasureAllAxes(figureRect, xAxes, yAxes);
        PixelRect dataArea = GetTotalAxisPadding(figureRect, xAxes, yAxes);
        return dataArea;
    }

    private void RegenerateTicksForAllAxes(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        foreach (IXAxis xAxis in xAxes)
        {
            xAxis.TickGenerator.Regenerate(xAxis.Range, figureRect.Width);
        }
        foreach (IYAxis yAxis in yAxes)
        {
            yAxis.TickGenerator.Regenerate(yAxis.Range, figureRect.Width);
        }
    }

    private void RemeasureAllAxes(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        foreach (IXAxis xAxis in xAxes)
        {
            xAxis.Measure();
        }
        foreach (IYAxis yAxis in yAxes)
        {
            yAxis.Measure();
        }
    }

    private PixelRect GetTotalAxisPadding(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        PixelPadding axisSizeByEdge = new();

        float bottomOffset = 0;
        foreach (IXAxis xAxis in xAxes.Where(x => x.Edge == Edge.Bottom))
        {
            axisSizeByEdge.Bottom += xAxis.PixelHeight;
            xAxis.Offset = bottomOffset;
            bottomOffset += xAxis.PixelHeight;
        }

        float topOffset = 0;
        foreach (IXAxis xAxis in xAxes.Where(x => x.Edge == Edge.Top))
        {
            axisSizeByEdge.Top += xAxis.PixelHeight;
            xAxis.Offset = topOffset;
            topOffset += xAxis.PixelHeight;
        }

        float leftOffset = 0;
        foreach (IYAxis yAxis in yAxes.Where(x => x.Edge == Edge.Left))
        {
            axisSizeByEdge.Left += yAxis.PixelWidth;
            yAxis.Offset = leftOffset;
            leftOffset += yAxis.PixelWidth;
        }

        float rightOffset = 0;
        foreach (IYAxis yAxis in yAxes.Where(x => x.Edge == Edge.Right))
        {
            axisSizeByEdge.Right += yAxis.PixelWidth;
            yAxis.Offset = rightOffset;
            rightOffset += yAxis.PixelWidth;
        }

        // TODO: manual reduction remove this
        axisSizeByEdge.Right += 20;
        axisSizeByEdge.Top += 20;

        return figureRect.Contract(axisSizeByEdge);
    }
}
