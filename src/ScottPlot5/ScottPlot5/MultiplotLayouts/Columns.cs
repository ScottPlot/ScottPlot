namespace ScottPlot.MultiplotLayouts;

public class Columns : IMultiplotLayout
{
    public PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[subplots.Count];

        double fractionPerColumn = 1.0 / subplots.Count;
        for (int i = 0; i < subplots.Count; i++)
        {
            FractionRect fr = new(fractionPerColumn * i, 0, fractionPerColumn, 1);
            rectangles[i] = fr.GetPixelRect(figureRect);
        }

        return rectangles;
    }
}
