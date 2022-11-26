using ScottPlot.Axis;
using System.Security.Cryptography;

namespace ScottPlot.LayoutSystem;

public class StandardLayoutSystem : ILayoutSystem
{
    public PixelRect AutoSizeDataArea(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        RegenerateTicksForAllAxes(figureRect, xAxes, yAxes);
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

    private PixelRect GetTotalAxisPadding(PixelRect figureRect, IEnumerable<IXAxis> xAxes, IEnumerable<IYAxis> yAxes)
    {
        PixelPadding axisSizeByEdge = new();

        float bottomOffset = 0;
        foreach (IXAxis xAxis in xAxes.Where(x => x.Edge == Edge.Bottom))
        {
            var pxHeight = xAxis.Measure();
            
            axisSizeByEdge.Bottom += pxHeight;
            xAxis.Offset = bottomOffset;
            bottomOffset += pxHeight;
        }

        float topOffset = 0;
        foreach (IXAxis xAxis in xAxes.Where(x => x.Edge == Edge.Top))
        {
            var pxHeight = xAxis.Measure();

            axisSizeByEdge.Top += pxHeight;
            xAxis.Offset = topOffset;
            topOffset += pxHeight;
        }

        float leftOffset = 0;
        foreach (IYAxis yAxis in yAxes.Where(x => x.Edge == Edge.Left))
        {
            var pxWidth = yAxis.Measure();

            axisSizeByEdge.Left += pxWidth;
            yAxis.Offset = leftOffset;
            leftOffset += pxWidth;
        }

        float rightOffset = 0;
        foreach (IYAxis yAxis in yAxes.Where(x => x.Edge == Edge.Right))
        {
            var pxWidth = yAxis.Measure();
            
            axisSizeByEdge.Right += pxWidth;
            yAxis.Offset = rightOffset;
            rightOffset += pxWidth;
        }

        // TODO: manual reduction remove this
        axisSizeByEdge.Right += 20;
        axisSizeByEdge.Top += 20;

        return figureRect.Contract(axisSizeByEdge);
    }
}
