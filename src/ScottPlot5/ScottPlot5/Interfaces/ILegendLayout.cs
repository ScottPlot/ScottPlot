namespace ScottPlot;

public interface ILegendLayout
{
    LegendLayout GetLayout(Legend legend, LegendItem[] items, PixelSize maxSize);
}
