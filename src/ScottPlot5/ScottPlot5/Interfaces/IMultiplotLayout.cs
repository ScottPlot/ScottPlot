namespace ScottPlot;

public interface IMultiplotLayout
{
    PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect);
}
