namespace ScottPlot;

public interface IMultiplotLayout
{
    PixelRect[] GetSubplotRectangles(Multiplot multiplot, PixelRect figureRect);
}
