namespace ScottPlot.MultiplotLayouts;

public class Rows : IMultiplotLayout
{
    public PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[subplots.Count];

        double fractionPerRow = 1.0 / subplots.Count;
        for (int i = 0; i < subplots.Count; i++)
        {
            FractionRect fr = new(0, fractionPerRow * i, 1, fractionPerRow);
            rectangles[i] = fr.GetPixelRect(figureRect);
        }

        return rectangles;
    }
}
