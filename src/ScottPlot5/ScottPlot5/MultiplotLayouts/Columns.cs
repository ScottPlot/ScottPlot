namespace ScottPlot.MultiplotLayouts;

public class Columns : IMultiplotLayout
{
    public PixelRect[] GetSubplotRectangles(Multiplot multiplot, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[multiplot.Count];

        double fractionPerColumn = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.Count; i++)
        {
            FractionRect fr = new(fractionPerColumn * i, 0, fractionPerColumn, 1);
            rectangles[i] = fr.GetPixelRect(figureRect);
        }

        return rectangles;
    }
}
