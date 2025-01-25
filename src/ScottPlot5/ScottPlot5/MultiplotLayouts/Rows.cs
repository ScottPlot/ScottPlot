namespace ScottPlot.MultiplotLayouts;

public class Rows : IMultiplotLayout
{
    public PixelRect[] GetSubplotRectangles(Multiplot multiplot, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[multiplot.Count];

        double fractionPerRow = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.Count; i++)
        {
            FractionRect fr = new(0, fractionPerRow * i, 1, fractionPerRow);
            rectangles[i] = fr.GetPixelRect(figureRect);
        }

        return rectangles;
    }
}
